using Eiffel.Debug.Examples;
using Microsoft.Extensions.Configuration;

namespace Eiffel.Debug
{
    public class RabbitMQDebugger
    {
        private readonly EiffelEventToolkit _eiffelEventToolkit;

        /*
         * EiffelEventToolkit is injected via Dependancy Injection (DI) this showcases one way of setting up a managaer of publishing Eiffel events.
         */
        public RabbitMQDebugger(EiffelEventToolkit eiffelEventToolkit)
        {
            _eiffelEventToolkit = eiffelEventToolkit;
        }

        public async Task PublishEiffelEventAsync()
        {
            Console.WriteLine("Enter the type of Eiffel event to publish (cd, id, tcs):");
            var eventType = Console.ReadLine();

            // Initialize event and routing key
            object eiffelEvent = null;
            string exchange = "mx.eiffel"; // Required for RabbitMQ
            string routingKey = ""; // Specific to the event type

            switch (eventType?.ToLower())
            {
                case "cd":
                    eiffelEvent = CreateEiffelCompositionDefinedEvent.CreateValidEvent();
                    routingKey = "eiffel.definition.EiffelCompositionDefinedEvent._._";
                    break;
                case "id":
                    eiffelEvent = CreateEiffelIssueDefinedEvent.CreateValidEvent();
                    routingKey = "eiffel.definition.EiffelIssueDefinedEvent._._";
                    break;
                case "tcs":
                    eiffelEvent = CreateEiffelTestCaseStartedEvent.CreateValidEvent();
                    routingKey = "eiffel.test.EiffelTestCaseStartedEvent._._";
                    break;
                default:
                    Console.WriteLine("Unknown event type.");
                    return;
            }

            if (_eiffelEventToolkit != null && eiffelEvent != null)
            {
                Console.WriteLine("RabbitMQ or GraphQL:");
                var target = Console.ReadLine();
                bool verdict;
                string message;

                switch (target)
                {
                    case "RabbitMQ":
                        (verdict, message) = await _eiffelEventToolkit.PublishEiffelEventAsync(eiffelEvent, exchange, routingKey);
                        Console.WriteLine($"Verdict: {verdict}, Message: {message}");
                        break;
                    case "GraphQL":
                        (verdict, message) = await _eiffelEventToolkit.PublishEiffelEventAsync(eiffelEvent, routingKey);
                        Console.WriteLine($"Verdict: {verdict}, Message: {message}");
                        break;
                    default:
                        Console.WriteLine("Unknown delivery method.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("EiffelEventToolkit could not be resolved or event is null.");
            }
        }
    }
}
