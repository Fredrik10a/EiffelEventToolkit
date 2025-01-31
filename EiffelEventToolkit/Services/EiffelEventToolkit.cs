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

        public EiffelEventToolkit(IGraphQLDistributor graphQLDistributor = null, IRabbitMQDistributor rabbitMQDistributor = null, SchemaValidator schemaValidator = null)
        {
            _graphQLDistributor = graphQLDistributor;
            _rabbitMQDistributor = rabbitMQDistributor;
            _schemaValidator = schemaValidator ?? throw new ArgumentNullException(nameof(schemaValidator));

            if (_graphQLDistributor == null && _rabbitMQDistributor == null)
            {
                throw new InvalidOperationException("At least one distributor (GraphQL or RabbitMQ) must be provided.");
            }
        }

        // Method for RabbitMQ distribution
        public async Task<(bool Verdict, string Message)> PublishEiffelEventAsync(object data, string exchange, string routingKey)
        {
            if (_rabbitMQDistributor == null)
            {
                return (false, "RabbitMQ distributor is not configured.");
            }

            // Validate the serialized JSON message
            var (isValid, validationMessage) = _schemaValidator.Validate(data);
            if (!isValid)
            {
                return (false, $"Validation failed: {validationMessage}");
            }

            var message = SerializeModel(data);
            return await _rabbitMQDistributor.ProcessEiffelMessageAsync(message, exchange, routingKey);
        }

        // Method for GraphQL distribution
        public async Task<(bool Verdict, string Message)> PublishEiffelEventAsync(object data, string routingKey)
        {
            if (_graphQLDistributor == null)
            {
                return (false, "GraphQL distributor is not configured.");
            }

            // Validate the serialized JSON message
            var (isValid, validationMessage) = _schemaValidator.Validate(data);
            if (!isValid)
            {
                return (false, $"Validation failed: {validationMessage}");
            }

            var message = SerializeModel(data);
            return await _graphQLDistributor.ProcessEiffelMessageAsync(message, routingKey);
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
