using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Eiffel.Services.Validation
{
    public class SchemaValidator
    {
        public async Task<(bool Success, string Message)> ValidateAsync(object message)
        {
            bool success = false;
            string resultMessage = "Message failed to validate against schema.";

            try
            {
                var messageJson = JsonConvert.SerializeObject(message);
                var messageObject = JsonConvert.DeserializeObject<JObject>(messageJson);
                var type = messageObject["meta"]?["type"]?.ToString();
                var version = messageObject["meta"]?["version"]?.ToString();

                if (type == null || version == null)
                {
                    return (false, "Message does not contain valid 'type' or 'version' in 'meta'.");
                }

                var schemaResourceName = $"Schemas.{type}.{version}.json";
                var schema = await LoadSchemaFromEmbeddedResourceAsync(schemaResourceName);
                if (schema == null)
                {
                    return (false, $"Schema not found in embedded resources: {schemaResourceName}");
                }

                var validationErrors = schema.Validate(messageJson);
                if (validationErrors.Count == 0)
                {
                    success = true;
                    resultMessage = "Message validated successfully against schema.";
                }
                else
                {
                    resultMessage = $"Validation failed. Errors: {string.Join("; ", validationErrors)}";
                }
            }
            catch (JsonException ex)
            {
                resultMessage = $"JSON error: {ex.Message}";
            }
            catch (Exception ex)
            {
                resultMessage = $"Unexpected error during validation: {ex.Message}";
            }

            return (success, resultMessage);
        }

        private async Task<JsonSchema> LoadSchemaFromEmbeddedResourceAsync(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceFullName = $"{assembly.GetName().Name}.{resourceName}";

            // Example: EiffelEventToolkit.Schemas.EiffelTestCaseFinishedEvent.2.0.0.json
            var stream = assembly.GetManifestResourceStream(resourceFullName);
            if (stream == null)
                return null;

            using (stream)
            using (var reader = new StreamReader(stream))
            {
                var schemaJson = await reader.ReadToEndAsync();
                return await JsonSchema.FromJsonAsync(schemaJson);
            }
        }
    }
}
