using Eiffel.Models.EiffelIssueDefinedEvent.V3_2_0;

namespace Eiffel.Debug.Examples
{
    public static class CreateEiffelIssueDefinedEvent
    {
        public static EiffelIssueDefinedEvent CreateValidEvent()
        {
            return new EiffelIssueDefinedEvent
            {
                // Meta ID & TIME set by default
                Data = new Data
                {
                    Type = DataType.BUG,
                    Tracker = "JIRA",
                    Id = "ISSUE-123",
                    Uri = "http://example.com/issue/ISSUE-123", // Required URI field
                    Title = "Example Bug",
                    CustomData = new System.Collections.ObjectModel.Collection<CustomData>
                    {
                        new CustomData { Key = "Priority", Value = "High" }
                    }
                },
                Links = new System.Collections.ObjectModel.Collection<Links>
                {
                    new Links
                    {
                        Type = "CAUSE",
                        Target = Guid.NewGuid().ToString() // Valid UUID
                    }
                }
            };
        }

        public static EiffelIssueDefinedEvent CreateInvalidEvent()
        {
            return new EiffelIssueDefinedEvent
            {
                Meta = new Meta
                {
                    Id = "12345", // Invalid format, should be a UUID
                    Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                },
                Data = new Data
                {
                    Type = DataType.BUG,
                    Tracker = "JIRA",
                    Id = "ISSUE-123",
                    // Missing required "uri" field in Data
                },
                Links = new System.Collections.ObjectModel.Collection<Links>
                {
                    new Links
                    {
                        Type = "INVALID_LINK_TYPE", // Invalid link type, should follow allowed link types
                        Target = "invalid-uuid" // Invalid format, should be a UUID
                    }
                }
            };
        }
    }
}
