using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/*
 * The following code demonstrates how to use the EiffelEventToolkit.
 * Though primarly this console APP is used to debug and test events.
 */

// Needed namespaces for package initialization
using Eiffel; // Mandatory, used to register the EiffelEventToolkit services

// Used in this Debug APP for debugging and testing
using Eiffel.Debug;
using Microsoft.Extensions.Logging;

class Program
{
    static async Task Main(string[] args)
    {
        /*
         * ----------------------------------------
         * Basic setup for the EiffelEventToolkit
         * 
         * EiffelEventToolkit is utilized with Dependancy Injection (DI) using the ServiceCollection.
         */

        // Container for the service registrations.
        var services = new ServiceCollection();

        // Configuration for either useage of the appsettings file or enable ENVs to parse credentials etc.
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ENVIRONMENT")}.json", optional: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();

        // Set up logging
        services.AddLogging(logging =>
        {
            logging.ClearProviders();
        });

        // This will register all the services needed for the EiffelEventToolkit.
        services.AddEiffelEventToolkitServices(config);

        /*
         * This will register the debuggers for RabbitMQ and GraphQL to be used in the console app.
         * Do note that these are not part of the EiffelEventToolkit, but are used for debugging and testing.
         */
        services.AddScoped<RabbitMQDebugger>(); // Test-specifc to show case DI for the EiffelEventToolkit
        services.AddScoped<GraphQLDebugger>(); // Test-specifc to show case DI for the EiffelEventToolkit

        // Build the service container ie. init all the services within the AddEiffelEventToolkitServices
        var serviceProvider = services.BuildServiceProvider();

        /*
         * END
         * ----------------------------------------
         */

        /*
         * This is a simple Console APP to demonstrate some Features of the EiffelEventToolkit.
         * How your specific implementation will look like is up to you.
         */

        // Get the debuggers from the service container
        var _rabbitMQDebugger = serviceProvider.GetRequiredService<RabbitMQDebugger>();
        var _graphQLDebugger = serviceProvider.GetRequiredService<GraphQLDebugger>();

        // Main loop for the Debug console app 
        Console.WriteLine("Choose a number for your action of choice:");
        Console.WriteLine("1. Publish a message");
        Console.WriteLine("2. GET via API");
        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                await _rabbitMQDebugger.PublishEiffelEventAsync();
                break;
            case "2":
                await _graphQLDebugger.GetEiffelEventByBaselineGuidAsync();
                break;
            default:
                Console.WriteLine("Invalid choice. Exiting.");
                break;
        }

    }
}
