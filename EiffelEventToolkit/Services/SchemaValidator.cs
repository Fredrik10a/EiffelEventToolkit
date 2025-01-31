using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.IO;
using System.Reflection;

namespace Eiffel.Services.Validation
{
    public class SchemaValidator
    {
        public (bool Success, string Message) Validate(object message)
        {
            bool success = false;
            string resultMessage = "Message failed to validate against schema.";

            try
            {
                // Serialize the message to JSON and extract type and version
                var messageJson = JsonConvert.SerializeObject(message);
                var messageObject = JsonConvert.DeserializeObject<JObject>(messageJson);
                var type = messageObject["meta"]?["type"]?.ToString();
                var version = messageObject["meta"]?["version"]?.ToString();

                if (type == null || version == null)
                {
                    return (false, "Message does not contain valid 'type' or 'version' in 'meta'.");
                }

                var schemaResourceName = $"Schemas.{type}.{version}.json";

                // Attempt to load the schema as an embedded resource
                var schema = LoadSchemaFromEmbeddedResource(schemaResourceName);
                if (schema == null)
                {
                    return (false, $"Schema not found in embedded resources: {schemaResourceName}");
                }

                // Validate the message against the loaded schema
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

        private JsonSchema LoadSchemaFromEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceFullName = $"{assembly.GetName().Name}.{resourceName}"; // Build the full resource name

            // Example: EiffelEventToolkit.Schemas.EiffelTestCaseFinishedEvent.2.0.0.json
            using (var stream = assembly.GetManifestResourceStream(resourceFullName))
            {
                if (stream == null)
                    return null;

                using (var reader = new StreamReader(stream))
                {
                    var schemaJson = reader.ReadToEnd();
                    return JsonSchema.FromJsonAsync(schemaJson).Result;
                }
            }
        }
    }
}
