using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Eiffel.Interfaces;
using Eiffel.Services.RabbitMQ;
using System.Threading;

namespace Eiffel.Services.Distributors
{
    public class RabbitMqDistributor : IRabbitMQDistributor
    {
        private readonly RabbitMqConnectionManager _connectionManager;

        public RabbitMqDistributor(RabbitMqConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public async Task<(bool Verdict, string Message)> ProcessEiffelMessageAsync(object message, string exchange, string routingKey)
        {
            var success = false;
            var resultMessage = "Failed to send Eiffel message.";

            try
            {
                var channel = await _connectionManager.GetChannelAsync().ConfigureAwait(false);
                // await the channel from your async connection manager
                using (channel)
                {
                    var publishCts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
                    var serialized = JsonConvert.SerializeObject(message);
                    var messageBody = Encoding.UTF8.GetBytes(serialized);

                    var properties = new BasicProperties
                    {
                        DeliveryMode = (DeliveryModes)2
                    };

                    await channel.BasicPublishAsync(
                        exchange,
                        routingKey,
                        mandatory: false,
                        basicProperties: properties,
                        body: messageBody,
                        cancellationToken: publishCts.Token
                    ).ConfigureAwait(false);

                    success = true;
                    resultMessage = "Message published and confirmed by RabbitMQ.";
                }
            }
            catch (Exception ex)
            {
                resultMessage = $"Error publishing message to RabbitMQ: {ex.Message}";
            }

            return await Task.FromResult((success, resultMessage));
        }
    }
}
