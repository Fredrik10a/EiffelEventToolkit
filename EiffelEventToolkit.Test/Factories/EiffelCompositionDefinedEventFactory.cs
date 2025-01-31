using Eiffel.Models.EiffelCompositionDefinedEvent.V3_3_0;

namespace Eiffel.Factories
{
    public static class EiffelCompositionDefinedEventFactory
    {
        public static EiffelCompositionDefinedEvent CreateMinimumEvent()
        {
            return new EiffelCompositionDefinedEvent
            {
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
    }
}
