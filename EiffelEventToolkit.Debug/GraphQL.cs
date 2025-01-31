namespace Eiffel.Debug
{
    public class GraphQLDebugger
    {
        private readonly EiffelEventToolkit _eiffelEventToolkit;

        /*
         * EiffelEventToolkit is injected via Dependancy Injection (DI) this showcases one way of setting up a managaer of retriving Eiffel events.
        */
        public GraphQLDebugger(EiffelEventToolkit eiffelEventToolkit)
        {
            _eiffelEventToolkit = eiffelEventToolkit;
        }

        public async Task GetEiffelEventByBaselineGuidAsync()
        {
            /*
             * Showcasing how to interact with the GrapQL API.
             */

            string query = @"
                query($baselineGuid: String!) {
                    getEiffelEventByBaselineGuid(baselineGuid: $baselineGuid)
                }
            ";

            // Random Baseline Guid
            var variables = new { baselineGuid = "8c56378a-630d-4c89-b04a-98439eaae042" };

            // Call the EiffelEventToolkit to get the Eiffel event
            var (verdict, message, data) = await _eiffelEventToolkit.GetEiffelEventAsync(query, variables);

            /*
             *  Output
             */
            Console.WriteLine($"Verdict: {verdict}, Message: {message}, Data: {data}");
            // Access getEiffelEventByBaselineGuid directly
            var eventGuid = data["getEiffelEventByBaselineGuid"]?.ToString();
            Console.WriteLine($"Test data: {eventGuid}");
        }
    }
}
