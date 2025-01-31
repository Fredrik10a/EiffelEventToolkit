using Eiffel.Models.EiffelCompositionDefinedEvent.V3_3_0;

namespace Eiffel.Debug.Examples
{
    public static class CreateEiffelCompositionDefinedEvent
    {
        public static EiffelCompositionDefinedEvent CreateValidEvent()
        {
            return new EiffelCompositionDefinedEvent
            {
                // Meta ID & TIME set by default                
                Data = new Data
                {
                    Name = "Minimal Eiffel Composition Defined Event"
                },
                Links = new System.Collections.ObjectModel.Collection<Links>
                {
                    new Links { Type = "ELEMENT", Target = Guid.NewGuid().ToString() }
                }
            };
        }

        public static EiffelCompositionDefinedEvent CreateInvalidEvent()
        {
            return new EiffelCompositionDefinedEvent
            {
                Meta = new Meta
                {
                    Id = "12345", // Invalid format, should be a UUID
                    Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                },
                Data = new Data
                {
                    Name = "Invalid Eiffel Composition Defined Event"
                },
                Links = new System.Collections.ObjectModel.Collection<Links>
                {
                    new Links { Type = "ELEMENT", Target = "12345" } // Invalid target format, not UUID
                }
            };
        }
    }
}
