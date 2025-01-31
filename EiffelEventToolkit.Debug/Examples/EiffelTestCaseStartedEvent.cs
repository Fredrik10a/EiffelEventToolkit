using Eiffel.Models.EiffelTestCaseStartedEvent.V3_3_0;

namespace Eiffel.Debug.Examples
{
    public static class CreateEiffelTestCaseStartedEvent
    {
        public static EiffelTestCaseStartedEvent CreateValidEvent()
        {
            return new EiffelTestCaseStartedEvent
            {
                // Meta ID & TIME set by default
                Data = new Data
                {
                    Executor = "Valid Test Executor",
                    LiveLogs = new System.Collections.ObjectModel.Collection<LiveLogs>
                    {
                        new LiveLogs
                        {
                            MediaType = "application/json",
                            Name = "Test Log",
                            Uri = "http://example.com/logs/test-log", // Required URI field
                            Tags = new[] { "tag1", "tag2" }
                        }
                    },
                    CustomData = new System.Collections.ObjectModel.Collection<CustomData>
                    {
                        new CustomData { Key = "customKey", Value = "customValue" }
                    }
                },
                Links = new System.Collections.ObjectModel.Collection<Links>
                {
                    new Links
                    {
                        Type = "TEST_CASE_EXECUTION",
                        Target = Guid.NewGuid().ToString() // Valid UUID
                    }
                }
            };
        }

        public static EiffelTestCaseStartedEvent CreateInvalidEvent()
        {
            return new EiffelTestCaseStartedEvent
            {
                Meta = new Meta
                {
                    Id = "12345", // Invalid format, should be a UUID
                    Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                },
                Data = new Data
                {
                    Executor = "Invalid Test Executor",
                    LiveLogs = new System.Collections.ObjectModel.Collection<LiveLogs>
                    {
                        new LiveLogs
                        {
                            MediaType = "application/json",
                            Name = "Test Log"
                            // Missing required "uri" field in LiveLog
                        }
                    }
                },
                Links = new System.Collections.ObjectModel.Collection<Links>
                {
                    new Links
                    {
                        Type = "INVALID_TYPE", // Invalid type, should be "TEST_CASE_EXECUTION"
                        Target = Guid.NewGuid().ToString() // Valid UUID but incorrect type value
                    }
                }
            };
        }
    }
}
