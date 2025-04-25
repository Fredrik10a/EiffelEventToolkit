using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Eiffel.Interfaces;
using Eiffel.Services.RabbitMQ;
using Eiffel.Services.Distributors;
using Eiffel.Services.Validation;

namespace Eiffel
{
    public static class EiffelEventToolkitExtensions
    {

        public static IServiceCollection AddEiffelEventToolkitServices(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                var eiffelToolkitConfig = configuration.GetSection("EiffelToolkit");
                /*
                 * Determine the connect target (RabbitMQ, GraphQL, ALL)
                 * NOTE: Empty toolkitTarget will setup EiffelEventToolkit with no distributors!
                 * This to use the EiffelEventToolkit as a Eiffel .NET Model source and schemaValidator.
                 */
                var toolkitTarget = Environment.GetEnvironmentVariable("EIFFEL_CONNECT_TARGET") ?? eiffelToolkitConfig.GetValue<string>("Target");

                services.AddSingleton<SchemaValidator>();

                switch (toolkitTarget?.ToLowerInvariant())
                {
                    case "rabbitmq":
                        ConfigureRabbitMQ(services, eiffelToolkitConfig);
                        break;
                    case "graphql":
                        ConfigureGraphQL(services, eiffelToolkitConfig);
                        break;
                    case "all":
                        ConfigureRabbitMQ(services, eiffelToolkitConfig);
                        ConfigureGraphQL(services, eiffelToolkitConfig);
                        break;
                }

                services.AddScoped(provider =>
                {
                    var schemaValidator = provider.GetRequiredService<SchemaValidator>();
                    if (string.IsNullOrEmpty(toolkitTarget))
                    {
                        return new EiffelEventToolkit(schemaValidator);
                    }
                    else
                    {
                        var graphQLDistributor = provider.GetService<IGraphQLDistributor>();
                        var rabbitMQDistributor = provider.GetService<IRabbitMQDistributor>();
                        return new EiffelEventToolkit(schemaValidator, graphQLDistributor, rabbitMQDistributor);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Build Error] Configuration error: {ex.Message}. Please check configuration settings.");
                throw;
            }

            return services;
        }

        private static void ConfigureRabbitMQ(IServiceCollection services, IConfiguration eiffelToolkitConfig)
        {
            var rabbitMqSettings = eiffelToolkitConfig.GetSection("RabbitMQ");
            var authMethod = rabbitMqSettings.GetValue<string>("AuthMethod") ?? "UserPassword"; // Default to "UserPassword"
            services.AddSingleton(provider =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = Environment.GetEnvironmentVariable("EIFFEL_RABBITMQ_HOSTNAME") ?? rabbitMqSettings["HostName"],
                    Port = int.Parse(Environment.GetEnvironmentVariable("EIFFEL_RABBITMQ_PORT") ?? rabbitMqSettings["Port"]),
                    VirtualHost = Environment.GetEnvironmentVariable("EIFFEL_RABBITMQ_VHOST") ?? rabbitMqSettings["VirtualHost"]
                };

                if (authMethod == "Token")
                {
                    // TODO: Not implemented yet and or Verified!
                    /*
                    var accessToken = Environment.GetEnvironmentVariable("EIFFEL_RABBITMQ_TOKEN");
                    if (string.IsNullOrEmpty(accessToken))
                    {
                        throw new InvalidOperationException("RabbitMQ token is not set in the environment variables.");
                    }

                    // Configure ConnectionFactory to use token-based (Bearer) authentication
                    factory.AuthMechanisms = new IAuthMechanismFactory[] { new ExternalMechanismFactory() };
                    factory.UserName = accessToken; // Typically, the token is passed as the username
                    */
                }
                else if (authMethod == "UserPassword")
                {
                    // Use username and password authentication
                    factory.UserName = Environment.GetEnvironmentVariable("EIFFEL_RABBITMQ_USER") ?? rabbitMqSettings["UserName"];
                    factory.Password = Environment.GetEnvironmentVariable("EIFFEL_RABBITMQ_PASSWORD") ?? rabbitMqSettings["Password"];
                }
                else
                {
                    throw new InvalidOperationException("Invalid authentication method specified. Use 'UserPassword' or 'Token'.");
                }

                return factory;
            });

            services.AddSingleton<RabbitMqConnectionManager>();
            services.AddSingleton<IRabbitMQDistributor, RabbitMqDistributor>();
        }

        private static void ConfigureGraphQL(IServiceCollection services, IConfiguration eiffelToolkitConfig)
        {
            var grapQLSettings = eiffelToolkitConfig.GetSection("GraphQL");
            var GraphQLUri = Environment.GetEnvironmentVariable("EIFFEL_GRAPHQL_URI") ?? grapQLSettings["Uri"];
            var BearerToken = Environment.GetEnvironmentVariable("EIFFEL_GRAPHQL_TOKEN") ?? grapQLSettings["Token"];

            services.AddSingleton<IGraphQLDistributor>(sp => new GraphQLApiDistributor(GraphQLUri, BearerToken));
        }
    }
}
