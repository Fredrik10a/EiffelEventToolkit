using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using System.Text.RegularExpressions;

public class JsonSchemaToCSharpGenerator
{
    public static async Task GenerateCSharpClasses(string schemaRootPath, string outputDirectory, string baseNamespace)
    {
        // Ensure output directory exists
        Directory.CreateDirectory(outputDirectory);

        // Recursively find all .json files under the schemaRootPath
        foreach (var schemaFilePath in Directory.EnumerateFiles(schemaRootPath, "*.json", SearchOption.AllDirectories))
        {
            var relativePath = GetRelativePath(schemaRootPath, schemaFilePath);
            var eventFolder = Path.GetDirectoryName(relativePath);
            var version = Path.GetFileNameWithoutExtension(schemaFilePath).Replace('.', '_');

            if (eventFolder == null) continue;

            var eventName = Path.GetFileName(eventFolder);
            var namespaceName = $"{baseNamespace}.{eventName}.V{version}";

            // Set the output directory and filename
            var eventOutputDirectory = Path.Combine(outputDirectory, eventName);
            var eventVersionOutputDirectory = Path.Combine(eventOutputDirectory, version);

            var fileName = $"{eventName}.cs";
            var filePath = Path.Combine(eventVersionOutputDirectory, fileName);

            // Generate the class only if it doesn't exist
            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(eventVersionOutputDirectory);
                if (!Directory.Exists(eventVersionOutputDirectory))
                {
                    Console.WriteLine($"Failed to create directory: {eventVersionOutputDirectory}");
                    return;
                }
                await GenerateClassForSchema(schemaFilePath, filePath, namespaceName, eventName);
            }
            else
            {
                Console.WriteLine($"Class file already exists: {filePath}, skipping generation.");
            }
        }
    }

    private static async Task GenerateClassForSchema(string schemaPath, string filePath, string namespaceName, string eventName)
    {
        // Load and parse the JSON schema file
        var schema = await JsonSchema.FromFileAsync(schemaPath);

        // Configure C# code generation settings
        var settings = new CSharpGeneratorSettings
        {
            Namespace = namespaceName,
            ClassStyle = CSharpClassStyle.Poco, // .NET standerd 2.0
            GenerateDataAnnotations = true
        };

        // IMPORTANT: resolver that maps "integer" to "long" as Schema does not support "long" type!
        var resolver = new CustomTypeResolver(settings);

        // Generate C# code from the schema
        var generator = new CSharpGenerator(schema, settings, resolver);
        var code = generator.GenerateFile();
        if (string.IsNullOrWhiteSpace(code))
        {
            Console.WriteLine("Code generation failed: Empty code file.");
            return;
        }

        // Replace firt occurance of Class to the Classname of the Event.
        var regex = new Regex(@"\bclass\s+\w+\b");
        code = regex.Replace(code, $"class {eventName}", 1); // Replace the first occurrence

        // Add Meta constructor for common values
        var appendedContent = $@"
        // Overriden classes for default values
        public partial class Meta
        {{
            public Meta()
            {{
                Id = System.Guid.NewGuid().ToString();
                Time = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }}
        }}".Replace("\r\n", "\n");

        // Insert the partial class before the closing brace of the namespace
        var lastBraceIndex = code.LastIndexOf('}');
        code = code.Insert(lastBraceIndex - 1, appendedContent);

        // Save the generated code to a file
        File.WriteAllText(filePath, code);
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Failed to write file: {filePath}");
            return;
        }

        Console.WriteLine($"C# class generated successfully for {schemaPath} at {filePath}");
    }

    private static string GetRelativePath(string basePath, string fullPath)
    {
        var baseUri = new Uri(basePath.EndsWith(Path.DirectorySeparatorChar.ToString()) ? basePath : basePath + Path.DirectorySeparatorChar);
        var fullUri = new Uri(fullPath);
        return Uri.UnescapeDataString(baseUri.MakeRelativeUri(fullUri).ToString().Replace('/', Path.DirectorySeparatorChar));
    }
}
