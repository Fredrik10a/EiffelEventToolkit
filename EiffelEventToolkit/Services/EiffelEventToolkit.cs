using Newtonsoft.Json;
using Eiffel.Interfaces;
using System.Threading.Tasks;
using Eiffel.Services.Validation;
using Newtonsoft.Json.Linq;
using System;

namespace Eiffel
{
    public class EiffelEventToolkit
    {
        private readonly IGraphQLDistributor _graphQLDistributor;
        private readonly IRabbitMQDistributor _rabbitMQDistributor;
        private readonly SchemaValidator _schemaValidator;

        public EiffelEventToolkit(SchemaValidator schemaValidator, IGraphQLDistributor graphQLDistributor = null, IRabbitMQDistributor rabbitMQDistributor = null)
        {
            _graphQLDistributor = graphQLDistributor;
            _rabbitMQDistributor = rabbitMQDistributor;
            _schemaValidator = schemaValidator;
        }

        // Method to validate an Eiffel event
        public Task<(bool IsValid, string Message)> ValidateOnlyAsync(object eiffelEvent)
        {
            return ValidateEiffelEventAsync(eiffelEvent);
        }

        // Method for RabbitMQ distribution
        public async Task<(bool Verdict, string Message)> PublishEiffelEventAsync(object data, string exchange, string routingKey)
        {
            if (_rabbitMQDistributor == null)
            {
                return (false, "RabbitMQ distributor is not configured.");
            }

            if (string.IsNullOrWhiteSpace(exchange) || string.IsNullOrWhiteSpace(routingKey))
            {
                return (false, "Exchange or routing key is missing.");
            }

            var (isValid, validationMessage) = await ValidateEiffelEventAsync(data);
            if (!isValid)
            {
                return (false, $"Validation failed: {validationMessage}");
            }

            try
            {
                var message = SerializeModel(data);
                return await _rabbitMQDistributor.ProcessEiffelMessageAsync(message, exchange, routingKey);
            }
            catch (Exception ex)
            {
                return (false, $"Exception during RabbitMQ distribution: {ex.Message}");
            }
        }

        // Method for GraphQL distribution
        public async Task<(bool Verdict, string Message)> PublishEiffelEventAsync(object data, string routingKey)
        {
            if (_graphQLDistributor == null)
            {
                return (false, "GraphQL distributor is not configured.");
            }

            var (isValid, validationMessage) = await ValidateEiffelEventAsync(data);
            if (!isValid)
            {
                return (false, $"Validation failed: {validationMessage}");
            }

            try
            {
                var message = SerializeModel(data);
                return await _graphQLDistributor.ProcessEiffelMessageAsync(message, routingKey);
            }
            catch (Exception ex)
            {
                return (false, $"Exception during GraphQL distribution: {ex.Message}");
            }
        }

        private async Task<(bool IsValid, string Message)> ValidateEiffelEventAsync(object data)
        {
            if (data == null)
            {
                return (false, "Validation failed: The provided Eiffel event data is null.");
            }

            return await _schemaValidator.ValidateAsync(data);
        }

        private string SerializeModel<TModel>(TModel model)
        {
            return JsonConvert.SerializeObject(model);
        }

        public async Task<(bool Verdict, string Message, JObject Data)> GetEiffelEventAsync(string query, object variables)
        {
            if (_graphQLDistributor == null)
            {
                return (false, "GraphQL distributor is not configured.", null);
            }

            return await _graphQLDistributor.GetEiffelEventAsync(query, variables);
        }
    }
}
