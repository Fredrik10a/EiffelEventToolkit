using Newtonsoft.Json;

namespace Eiffel.Services.Distributors.Models
{
    public class CreateEiffelEvent
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("reason")]
        public string Reason { get; set; }
    }

    public class RootObject
    {
        [JsonProperty("createEiffelEvent")]
        public CreateEiffelEvent CreateEiffelEvent { get; set; }
    }
}
