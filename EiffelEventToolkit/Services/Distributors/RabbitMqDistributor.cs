using System;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<(bool Verdict, string Message)> ProcessEiffelMessageAsync(object message, string exchange, string routingKey)
        {
            var success = false; // Assume failure
            var resultMessage = "Failed to send Eiffel message."; // Default message

            // Execute the publish operation in a background task to prevent blocking
            await Task.Run(() =>
            {
                using (var channel = _connectionManager.GetChannel())
                {
                    try
                    {
                        // Convert message to byte array
                        var messageBody = Encoding.UTF8.GetBytes(message.ToString());
                        var properties = channel.CreateBasicProperties();
                        properties.DeliveryMode = 2; // Make message persistent

                        // Publish the message to RabbitMQ
                        channel.BasicPublish(exchange, routingKey, properties, messageBody);
                        success = true;
                        resultMessage = "Message published successfully to RabbitMQ.";
                    }
                    catch (Exception ex)
                    {
                        resultMessage = $"Error publishing message to RabbitMQ: {ex.Message}";
                    }
                }
            });

            return (success, resultMessage);
        }

    }
}
