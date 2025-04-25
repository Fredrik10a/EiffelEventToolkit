using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Eiffel.Interfaces;
using Eiffel.Services.RabbitMQ;

namespace Eiffel.Services.Distributors
{
    public class RabbitMqDistributor : IRabbitMQDistributor
    {
        private readonly RabbitMqConnectionManager _connectionManager;

        public RabbitMqDistributor(RabbitMqConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public Task<(bool Verdict, string Message)> ProcessEiffelMessageAsync(object message, string exchange, string routingKey)
        {
            var success = false;
            var resultMessage = "Failed to send Eiffel message.";

            try
            {
                using (var channel = _connectionManager.GetChannel())
                {
                    channel.ConfirmSelect();

                    var serialized = JsonConvert.SerializeObject(message);
                    var messageBody = Encoding.UTF8.GetBytes(serialized);

                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // Persistent

                    channel.BasicPublish(exchange, routingKey, properties, messageBody);

                    // Timeout 20 seconds for confirmation
                    bool confirmed = channel.WaitForConfirms(TimeSpan.FromSeconds(20));
                    if (!confirmed)
                    {
                        throw new Exception($"RabbitMQ did not confirm message within 20 seconds. Exchange: {exchange}, RoutingKey: {routingKey}");
                    }

                    success = true;
                    resultMessage = "Message published and confirmed by RabbitMQ.";
                }
            }
            catch (Exception ex)
            {
                resultMessage = $"Error publishing message to RabbitMQ: {ex.Message}";
            }

            return Task.FromResult((success, resultMessage));
        }
    }
}
