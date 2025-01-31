using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Eiffel.Interfaces;
using Eiffel.Services.Distributors.Models;

namespace Eiffel.Services.Distributors
{
    public class GraphQLApiDistributor : IGraphQLDistributor
    {
        private readonly IGraphQLClient _graphQLClient;

        public GraphQLApiDistributor(string baseUri, string bearerToken)
        {
            // Initialize HttpClient and set headers
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUri),
                Timeout = TimeSpan.FromSeconds(20)
            };

            // Set up headers
            if (!string.IsNullOrEmpty(bearerToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(bearerToken);
                httpClient.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate, br");
                httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                httpClient.DefaultRequestHeaders.Connection.ParseAdd("keep-alive");
                httpClient.DefaultRequestHeaders.Add("DNT", "1");
                httpClient.DefaultRequestHeaders.Add("Origin", "http://localhost:4000");
            }
            else
            {
                Console.WriteLine("No token provided for GraphQLApiDistributor. Authentication headers will not be set.");
            }

            // Initialize GraphQLHttpClient
            _graphQLClient = new GraphQLHttpClient(new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(baseUri)
            }, new NewtonsoftJsonSerializer(), httpClient);
        }

        public async Task<(bool Verdict, string Message)> ProcessEiffelMessageAsync(object message, string routingKey)
        {
            var success = false; // Assume failure
            var resultMessage = "Failed to send Eiffel message."; // Default message

            string mutation = @"
            mutation($eiffelEvent: String!, $routingKey: String) {
                createEiffelEvent(eiffelEvent: $eiffelEvent, routingKey: $routingKey) {
                    status
                    message
                    reason
                }
            }";

            var variables = new
            {
                eiffelEvent = message.ToString(),
                routingKey = routingKey
            };

            var request = new GraphQLRequest
            {
                Query = mutation,
                Variables = variables
            };

            try
            {
                var response = await _graphQLClient.SendMutationAsync<dynamic>(request);

                if (response?.Data != null) // Add null check for response.Data
                {
                    var dataResponse = JsonConvert.DeserializeObject<RootObject>(response.Data?.ToString());

                    if (dataResponse?.CreateEiffelEvent != null)
                    {
                        var status = dataResponse.CreateEiffelEvent.Status;
                        var messageDetail = dataResponse.CreateEiffelEvent.Message;
                        var reason = dataResponse.CreateEiffelEvent.Reason;

                        if (!string.IsNullOrEmpty(status) && status == "200")
                        {
                            success = true;
                            resultMessage = messageDetail;
                        }
                        else
                        {
                            resultMessage = $"Failed to send Eiffel message. Status: {status}. Message: {messageDetail}. Reason: {reason}";
                        }
                    }

                    if (response.Errors != null && response.Errors.Any())
                    {
                        resultMessage = "GraphQL errors occurred: ";
                        foreach (var error in response.Errors)
                        {
                            resultMessage += $"Error Message: {error.Message} ";
                            resultMessage += $"Error Extensions: {JsonConvert.SerializeObject(error.Extensions)} ";
                        }
                        success = false;
                    }
                }
            }
            catch (GraphQLHttpRequestException ex)
            {
                resultMessage = $"GraphQL request error: {ex.Message}";
                if (ex.Content != null)
                {
                    resultMessage += $" Error content: {ex.Content}";
                }
            }
            catch (Exception ex)
            {
                resultMessage = $"Error distributing message: {ex.Message}";
            }

            return (success, resultMessage);
        }

        public async Task<(bool Verdict, string Message, JObject Data)> GetEiffelEventAsync(string query, object variables)
        {
            try
            {
                JObject data = null;
                var resultMessage = string.Empty; // Default message
                var request = new GraphQLRequest
                {
                    Query = query,
                    Variables = variables
                };

                // Send the request and await the response
                var response = await _graphQLClient.SendMutationAsync<dynamic>(request);

                if (response.Data != null) 
                {
                    resultMessage = $"Data returned as a JSON object";
                    data = JsonConvert.DeserializeObject<JObject>(response.Data?.ToString());
                    return (true, resultMessage, data);
                }
                else if (response.Errors != null && response.Errors.Any())
                {
                    var errorMessages = string.Join("; ", response.Errors.Select(e => e.Message));
                    resultMessage = $"GraphQL request failed with errors: {errorMessages}";
                    return (false, resultMessage, data);
                } 
                else
                {
                    resultMessage = "Response data from GraphQL is null";
                    return (false, resultMessage, data);
                }
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred while executing the query: {ex.Message}", null);
            }
        }

    }
}
