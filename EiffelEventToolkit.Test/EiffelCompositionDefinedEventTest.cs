using Eiffel.Factories;

namespace Eiffel.Tests
{
    public class EiffelCompositionDefinedEventTests
    {
        [Test]
        public void CreateMinimumEvent_ShouldCreateValidEvent()
        {
            // Act
            var eiffelEvent = EiffelCompositionDefinedEventFactory.CreateMinimumEvent();

            // Assert
            Assert.IsNotNull(eiffelEvent);
            Assert.IsNotNull(eiffelEvent.Meta);
            Assert.AreEqual("EiffelCompositionDefinedEvent", eiffelEvent.Meta.Type.ToString());
            Assert.IsNotNull(eiffelEvent.Data.Name);
            Assert.IsNotEmpty(eiffelEvent.Links);
        }
    }
}
