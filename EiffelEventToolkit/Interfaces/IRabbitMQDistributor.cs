using System.Threading.Tasks;

namespace Eiffel.Interfaces
{
    public interface IRabbitMQDistributor
    {
        Task<(bool Verdict, string Message)> ProcessEiffelMessageAsync(object message, string exchange, string routingKey);
    }
}
