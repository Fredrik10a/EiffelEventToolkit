public class Program
{
    public static async Task Main(string[] args)
    {
        // Locate the solution directory
        var solutionDirectory = FindSolutionDirectory();
        if (solutionDirectory == null)
        {
            Console.WriteLine("Solution directory not found.");
            return;
        }

        // Define the main project directory and output directory
        string mainProjectDirectory = Path.Combine(solutionDirectory, "EiffelEventToolkit");

        string outputDirectory = Path.Combine(mainProjectDirectory, "Models");
        string schemaRootPath = Path.Combine(mainProjectDirectory, "Schemas");

        // Define the schema directory and namespace
        string baseNamespace = "Eiffel.Models";

        // Run the generator
        await JsonSchemaToCSharpGenerator.GenerateCSharpClasses(schemaRootPath, outputDirectory, baseNamespace);

        Console.WriteLine("Class generation completed.");
    }

    private static string? FindSolutionDirectory()
    {
        var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (currentDir != null && !currentDir.GetFiles("*.sln").Any())
        {
            currentDir = currentDir.Parent;
        }
        return currentDir?.FullName;
    }
}
