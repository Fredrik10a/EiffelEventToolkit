using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Eiffel.Interfaces
{
    public interface IGraphQLDistributor
    {
        Task<(bool Verdict, string Message)> ProcessEiffelMessageAsync(object message, string routingKey);

        Task<(bool Verdict, string Message, JObject Data)> GetEiffelEventAsync(string query, object variables);
    }
}
