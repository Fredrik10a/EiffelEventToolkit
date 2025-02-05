# Eiffel event toolkit

EiffelEventToolkit is a .NET Standard 2.0 library designed to simplify the creation, validation, and distribution of Eiffel protocol events via RabbitMQ or GraphQL. It uses C# representations of Eiffel events generated from official JSON schemas and supports both basic and advanced usage patterns.

## Table of Contents
1. [Abstract](#abstract)
2. [Toolkit Modes](#toolkit-modes)
3. [Distribution Methods](#distribution-methods)
4. [Quick Usage](#quick-usage)
5. [Installation](#installation)
6. [Configuration Setup](#configuration-setup)
7. [Using with RabbitMQ](#eiffel-event-toolkit-with-rabbitmq)
8. [Using with GraphQL API](#eiffel-event-toolkit-with-graphql-api)
9. [Implementation Examples](#implementation-examples)

## Abstract

EiffelEventToolkit provides strongly-typed C# models for Eiffel protocol events, automatically generated from the official JSON schema files. It simplifies the creation, validation, and distribution of Eiffel events over RabbitMQ or GraphQL.

When `PublishEiffelEventAsync()` is called by the user, the library will serialize the C# class object into a JSON object and publish it.

**The file structure:**
```[EVENT_TYPE]/[VERSION]/[EVENT_TYPE].cs```

**How to know which event to use?** 
Start off by reading the abstract of those events relevant for your use-case. [eiffel-vocabulary](https://github.com/eiffel-community/eiffel/tree/master/eiffel-vocabulary)

**Additional information:**

[Event Categories](https://github.com/eiffel-community/eiffel/blob/master/eiffel-syntax-and-usage/event-categories.md) this is helpful when defining the routingKey.

Some examples of routingKeys (*only applicable when using RabbitMQ as the `connect method`*):
```bash
eiffel.[Category].[Name]._._
eiffel.definition.EiffelCompositionDefinedEvent._._
eiffel.test.EiffelTestCaseStartedEvent._._
eiffel.activity.EiffelActivityStartedEvent._._
```

## Toolkit Modes

You can use the `EiffelEventToolkit` in three main ways:

1. **Validation and model-only usage (no distribution)**  
   If you don't define any target (neither RabbitMQ nor GraphQL), the toolkit can still be used to:
   - Instantiate strongly typed Eiffel event models
   - Validate them against the correct JSON schema

2. **RabbitMQ mode**  
   Used to publish Eiffel events to a message bus. Requires RabbitMQ configuration.

3. **GraphQL mode**  
   Used to query Eiffel events from an API or distribute via GraphQL mutation. Requires GraphQL configuration.

4. **ALL mode**  
   Enables both RabbitMQ and GraphQL simultaneously.

```json
{
  "EiffelToolkit": {
    "Target": "", // Leave empty to only use model/validation features
  }
}
```

## Distribution Methods

- **RabbitMQ** is used to **publish** Eiffel events to a message bus.
- **GraphQL API** is used to **query** existing Eiffel events or to optionally distribute them via GraphQL endpoints.
- You can configure the toolkit to use **RabbitMQ**, **GraphQL**, or **both** by setting the `Target` to "RabbitMQ", "GraphQL" or "ALL".

## Quick Usage

```csharp
// Register services (Program.cs)
services.AddEiffelEventToolkitServices(configuration);

// Publish an event
var (verdict, message) = await _eiffelEventToolkit.PublishEiffelEventAsync(eiffelEvent, "mx.eiffel", "eiffel.test.EiffelTestCaseFinishedEvent._._");
```

## Installation

### NuGet installation

There are two ways, commandline or within Visual Studio.

#### Commandline
```bash
dotnet nuget add source "./EiffelEventToolkit.A.B.C.nupkg" --name EiffelEventToolkit
dotnet add package EiffelEventToolkit --source EiffelEventToolkit
```
#### VisualStudio

Step 1: Add Your Local Folder as a Package Source
* In Visual Studio, go to Tools > Options.
* In the Options window, navigate to NuGet Package Manager > Package Sources.
* Click the + (Add) button to add a new source.
* Enter a name (e.g., "EiffelEventToolkit").
* For the Source field, browse to the folder on your local drive that contains the .nupkg files for the package(s) you want to add.
* Click OK to save.

Step 2: Install the Package from the Local Source
* Right-click on your project in Solution Explorer and select Manage NuGet Packages.
* In the NuGet Package Manager window, select the Browse tab.
* In the Package source dropdown (top-right), choose the new source you just added (e.g., "LocalPackages").
* You should see your locally available packages. Select the package you want to install.
* Click Install to add it to your project.

Step 3: Verify the Installation
* Once installed, verify the package is added by checking the Dependencies section in Solution Explorer or by viewing the csproj file to see the package reference.

## Configuration setup

## Prerequisites 

Regarding configuration you can use an appsettings.json or environment variables.

### The appsettings.json way
example of an appsettings.json for localhost
```json
{
  "EiffelToolkit": {
    "Target": "", // "RabbitMQ", "GraphQL" or "ALL"
    "GraphQL": {
      "Uri": "",
      "Token": ""
    },
    "RabbitMQ": {
      "HostName": "localhost",
      "AuthMethod": "UserPassword",
      "Port": "5672",
      "UserName": "guest",
      "Password": "guest",
      "VirtualHost": "Eiffel"
    }
  }
}
```

### The Environment variable way
Mandatory if using Environment variables, depending on specified target applicable ENVs can be defined alone.
If you use the `ALL` configuration, both RabbitMQ and GraphQL is needed to be defined.
```sh
EIFFEL_CONNECT_TARGET = // "RabbitMQ", "GraphQL" or "ALL"
```
Configure connection to a RabbitMQ server.
```sh
EIFFEL_RABBITMQ_HOSTNAME
EIFFEL_RABBITMQ_PORT
EIFFEL_RABBITMQ_VHOST
EIFFEL_RABBITMQ_USER
EIFFEL_RABBITMQ_PASSWORD
```
Configure connection to a GraphQL API.
```sh
EIFFEL_GRAPHQL_URI
EIFFEL_GRAPHQL_TOKEN
```

# Eiffel event toolkit with RabbitMQ

## Prerequisites
To be able to publish single events toward the RabbitMQ Eiffel bus, RabbitMQ has to be defined as the Connecting method.

## Implementation

```csharp
// An example
object eiffelEvent = // Your Eiffel event object
string exchange = "mx.eiffel"; // Required for RabbitMQ
string routingKey = "eiffel.[CATEGORY].[EVENT_TYPE]._._";

(bool verdict,string message) = await _eiffelEventToolkit.PublishEiffelEventAsync(eiffelEvent, exchange, routingKey);

// ...
// Handle the Verdict and message 

Console.WriteLine($"Verdict: {verdict}, Message: {message}");
```

# Eiffel event toolkit with GraphQL API

An example of how to use the GraphQL API for quering.

## Prerequisites
To be able to use GraphQL it has to be defined as the Connecting method.
Also applicable endpoints needs to be pre-defined.

**GraphQL Schema**
```csharp
// Example: Fetching an Eiffel event by baseline GUID
string query = @"
    query($baselineGuid: String!) {
        getEiffelEventByBaselineGuid(baselineGuid: $baselineGuid)
    }
";

var variables = new { baselineGuid = "your-baseline-guid" };

// Return types: (bool Verdict, string Message, JObject Data)
var (verdict, message, data) = await _eiffelEventToolkit.GetEiffelEventAsync(query, variables);

// ...
// Handle the Verdict and message 

Console.WriteLine($"Verdict: {success}, Message: {message}, Data: {data}");

// Access getEiffelEventByBaselineGuid directly
var eventGuid = data["getEiffelEventByBaselineGuid"]?.ToString();

Console.WriteLine($"Test object: {eventGuid}");
```

# Implementation examples

An example on how to create an Eiffel event object.

Below will be two examples on how to create an Eiffel event object.
First a simple example and then a more complex example, depending on the design you like the most.

None of these should be seen as the only way to create an Eiffel event object, but as a starting point.
Also the examples are not complete and will need some adjustments to fit your specific use-case.

## Table of Contents
1. [Simple](#simple-example)
2. [Advanced](#advanced-example)

### Simple example
Even if the example is simple, some parts are still required to be filled in.
Like a manager class to handle the creation and publishing of the event object, and 
a setup in the program.cs to register the services.

This shows that the Events are created by using the .NET model representation of the event object.

```csharp
using Eiffel.Models.EiffelCompositionDefinedEvent.V3_3_0;

EiffelCompositionDefinedEvent _eiffelEvent = new EiffelCompositionDefinedEvent
{
    Meta = new Meta
    {
        Source = new Source
        {
            Name = "[NAME_OF_SOURCE]", // Should be static and shared for all event from the data source
            Uri = [URL_TO_THIS_DATA] // Could be used to refer to the data source and or data itself
        }
    },
    Data = new Data
    {
        Name = "[NAME_OF_TYPE]", // Example name of data type
        ...
    },
};
```

### Advanced example

This solution is more complex but supports a more dynamic way of creating event objects.
It utilize a factory and builder pattern to create the Eiffel event.

#### Example setup

##### program.cs
This could be done in many ways, but this is an example of how to setup the Eiffel event toolkit.
```csharp
    using Eiffel;

    // ...

    builder.Services.AddEiffelEventToolkitServices(builder.Configuration); // Register EiffelEventToolkit services
    builder.Services.AddScoped<IRoutingKeyTable, RoutingKeyTable>();
    builder.Services.AddScoped<EiffelCommonSettings>(sp =>
    {
        var config = sp.GetRequiredService<IConfiguration>();
        return new EiffelCommonSettings(
            // Every event should have same a source Name within the same context ex. "[SYSTEM_NAME]", this to identify where the event is sent from
            "SUPER_SYSTEM", // Source Service Name
            "http://my-host.a.b.c.net/subpath"
        );
    });
    builder.Services.AddScoped<TestSuiteStarted>();
    // ... others event types like builder.Services.AddScoped<TestSuiteStarted>(); ...
    builder.Services.AddScoped<IEventCreator>(sp => new EventCreator(
        sp.GetRequiredService<TestSuiteStarted>(),
        ...
    ));
    builder.Services.AddScoped<IEiffelManager>(sp =>
    {
        return new EiffelManager(sp.GetRequiredService<IEventCreator>(), sp.GetRequiredService<EiffelEventToolkit>(), sp.GetRequiredService<ILogger<EiffelManager>>());
        // ...
    });

    // ...
```

##### EiffelManager.cs
```csharp
using Eiffel; // for the EiffelEventToolkit
// NEEDS TO MATCH THE USED ONES IN EACH CREATOR
using Eiffel.Models.EiffelTestSuiteStartedEvent.V3_4_0;

namespace Eiffel
{
    public class EiffelManager : IEiffelManager
    {
        private readonly ILogger<IEiffelManager> _logger;
        private readonly IEventCreator _eventCreator;
        private readonly EiffelEventToolkit _eiffelEventToolkit;
        private readonly string _eiffelRabbitMQExchange = "mx.eiffel"; // This is the RabbitMQ exchange name, used by Eiffel Event Toolkit!

        /* -------------------------------------------------------------------------------------------- */
        // Manager constructor to initialize the EiffelManager with the required dependencies (DI)
        /* -------------------------------------------------------------------------------------------- */
        public EiffelManager(IEventCreator eventCreator, EiffelEventToolkit eiffelEventToolkit, ILogger<IEiffelManager> logger)
        {
            _eventCreator = eventCreator;
            _eiffelEventToolkit = eiffelEventToolkit;
            _logger = logger;
        }

        /* -------------------------------------------------------------------------------------------- */
        // Entry point to start the Eiffel event creation process
        /* -------------------------------------------------------------------------------------------- */
        public async Task<IResult> processEiffel([MY_TYPE] input)
        {
            ... Some logic to manage the input ...
            ... Other events before the TestSuiteStarted event ...

            // TestSuiteStarted event is created to mark the start of the test suite
            var ([A], [B]) = await TestSuiteStarted(input);

            ... Other events after the TestSuiteStarted event ...
        }

        /* -------------------------------------------------------------------------------------------- */
        // Manager methods to create Eiffel events
        // These methods are called to prepare the data for each EventCreator instance.
        /* -------------------------------------------------------------------------------------------- */
        
        ... One of many methods to create an event object ...
        
        private async Task<(A, B)> TestSuiteStarted([TYPE] input)
        {
            // Any object can be passed as data  
            var data = new Dictionary<string, string>
            {
                { "A", "ABC1" },
                { "B", "ABC2" },
            };
            var eventRecord = await _eventCreator.TestSuiteStarted(data);

            await PublishEvent(eventRecord);
        }

        /* -------------------------------------------------------------------------------------------- */
        // Wrapper for publishing Eiffel events to RabbitMQ
        /* -------------------------------------------------------------------------------------------- */
        private async Task PublishEvent(EiffelDeliveryRecord eventRecord)
        {
            if (eventRecord != null)
            {
                // This is the Entrypoint to Eiffel Event Toolkit to publish the event to RabbitMQ!
                (bool verdict, string message) = await _eiffelEventToolkit.PublishEiffelEventAsync(eventRecord.Delivery, _eiffelRabbitMQExchange, eventRecord.RoutingKey);

                if (verdict)
                {
                    _logger.LogInformation("PublishEvent: {message}", message);
                }
                else
                {
                    _logger.LogError("PublishEvent: {message}", message);
                }
            }
        }
    }
}
```

##### EventCreator.cs
```csharp
using Eiffel.Interfaces;

namespace Eiffel
{
    public class EventCreator : IEventCreator
    {
        private readonly ICreateEvent _testSuiteStarted;
        ... others ...

        public EventCreator(
            ICreateEvent testSuiteStarted,
            ... others ...
            )
        {
            _testSuiteStarted = testSuiteStarted;
            ... others ...
        }

        ... others ...
        public Task<EiffelDeliveryRecord> TestSuiteStarted(object data) => CreateEvent(_testSuiteStarted, data);
        ... others ...

        private async Task<EiffelDeliveryRecord> CreateEvent(ICreateEvent eventCreator, object data = null)
        {
            try
            {
                return await eventCreator.CreateEvent(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in {eventCreator.GetType().Name}: {ex.Message}");
                return new EiffelDeliveryRecord(null, string.Empty);
            }
        }
    }
}
```

##### Misc

##### EiffelDeliveryRecord.cs
```csharp
namespace Eiffel.Events.misc
{
    public record EiffelCommonSettings(string sourceName, string sourceURI);
}
```

###### BaseCreateEvent.cs
```csharp
using Eiffel.Interfaces;

namespace Eiffel.Events.misc

    public abstract class BaseCreateEvent<TEvent> : ICreateEvent where TEvent : class, new()
    {
        private readonly IRoutingKeyTable _routingKeyTable;
        private readonly EiffelCommonSettings _commonSettings;

        protected BaseCreateEvent(IRoutingKeyTable routingKeyTable, EiffelCommonSettings commonSettings)
        {
            _routingKeyTable = routingKeyTable;
            _commonSettings = commonSettings;
        }

        protected abstract void CustomizeEvent(TEvent eiffelEvent, EiffelCommonSettings commonSettings, object data);

        public async Task<EiffelDeliveryRecord> CreateEvent(object data)
        {
            try
            {
                // Create a new instance of the event
                TEvent eiffelEvent = new TEvent();

                // Customize the event-specific data
                CustomizeEvent(eiffelEvent, _commonSettings, data);

                // Get the routing key using the event type
                string routingKey = _routingKeyTable.GetRoutingKey(typeof(TEvent).Name);

                // Return the delivery record
                return new EiffelDeliveryRecord(eiffelEvent, routingKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating event: {ex.Message}");
                return null;
            }
        }
    }
}
```

##### RoutingKeyTable.cs
```csharp
using Eiffel.Interfaces;

namespace Eiffel.Events.misc
{
    public class RoutingKeyTable : IRoutingKeyTable
    {
        public RoutingKeyTable()
        { }

        public string GetRoutingKey(string eventType) =>
            eventType switch
            {
                // Activity events
                "EiffelActivityCanceledEvent" or
                "EiffelActivityFinishedEvent" or
                "EiffelActivityStartedEvent" or
                "EiffelActivityTriggeredEvent" =>
                    $"eiffel.activity.{eventType}._._",

                // Artifact events
                "EiffelArtifactCreatedEvent" or
                "EiffelArtifactPublishedEvent" or
                "EiffelArtifactReusedEvent" =>
                    $"eiffel.artifact.{eventType}._._",

                // Definition events
                "EiffelFlowContextDefinedEvent" or
                "EiffelCompositionDefinedEvent" or
                "EiffelEnvironmentDefinedEvent" or
                "EiffelIssueDefinedEvent" =>
                    $"eiffel.definition.{eventType}._._",

                // Notification events
                "EiffelAnnouncementPublishedEvent" =>
                    $"eiffel.notification.{eventType}._._",

                // Source events
                "EiffelSourceChangeCreatedEvent" or
                "EiffelSourceChangeSubmittedEvent" =>
                    $"eiffel.source.{eventType}._._",

                // Test events
                "EiffelTestCaseCanceledEvent" or
                "EiffelTestCaseFinishedEvent" or
                "EiffelTestCaseStartedEvent" or
                "EiffelTestCaseTriggeredEvent" or
                "EiffelTestExecutionRecipeCollectionCreatedEvent" or
                "EiffelTestSuiteFinishedEvent" or
                "EiffelTestSuiteStartedEvent" =>
                    $"eiffel.test.{eventType}._._",

                // Verdict events
                "EiffelConfidenceLevelModifiedEvent" or
                "EiffelIssueVerifiedEvent" =>
                    $"eiffel.verdict.{eventType}._._",

                // Default case
                _ => "#",
            };
    }
}
```

##### TestSuiteStarted.cs
This is an example of how to create an event object using the factory and builder pattern.
More events can be implemented using the same pattern.
```csharp
using Eiffel.Models.EiffelTestSuiteStartedEvent.V3_4_0;
using Eiffel.Events.misc;
using Eiffel.Interfaces;

namespace Eiffel.Events
{
    public class TestSuiteStarted : BaseCreateEvent<EiffelTestSuiteStartedEvent>
    {
        public TestSuiteStarted(IRoutingKeyTable routingKeyTable, EiffelCommonSettings commonSettings) : base(routingKeyTable, commonSettings) { }

        protected override void CustomizeEvent(EiffelTestSuiteStartedEvent eiffelEvent, EiffelCommonSettings commonSettings, object data = null)
        {
            [MY_TYPE] _data = ([MY_TYPE])data;
            eiffelEvent.Meta = new Meta
            {
                Source = new Source
                {
                    Name = commonSettings.sourceName,
                    Uri = commonSettings.sourceURI + "/id/" + [ROUTE_NAME] // Example API path "http://example.com/uniqueId/MyTestSuite"
                }
            };
            eiffelEvent.Data = new Data
            {
                Name = testSuiteName,
                Categories = new Collection<string> { "V6", "Truck" }
            };
            eiffelEvent.Links = new Collection<Links>
            {
                new Links
                {
                    Target = _data.actID,
                    Type = "CONTEXT"
                },
                new Links
                {
                    Target = _data.otherID,
                    Type = "CAUSE"
                }
            };
        }
    }
}
```

#### Interfaces

##### ICreateEvent.cs
```csharp
namespace Eiffel.Interfaces
{
    public interface ICreateEvent
    {
        Task<EiffelDeliveryRecord> CreateEvent(object data);
    }
}
```

##### IEventCreator.cs
```csharp
namespace Eiffel.Interfaces
{
    public interface IEventCreator
    {
        Task<EiffelDeliveryRecord> ActivityTriggered();

        Task<EiffelDeliveryRecord> ActivityStarted(object data);

        Task<EiffelDeliveryRecord> ActivityFinished(object data);

        Task<EiffelDeliveryRecord> TestSuiteStarted(object data);

        Task<EiffelDeliveryRecord> TestCaseTriggered(object data);

        Task<EiffelDeliveryRecord> TestCaseStarted(object data);

        Task<EiffelDeliveryRecord> TestCaseFinished(object data);

        Task<EiffelDeliveryRecord> TestSuiteFinished(object data);
    }
}
```

##### IRoutingKeyTable.cs
```csharp
namespace Eiffel.Interfaces
{
    public interface IRoutingKeyTable
    {
        public string GetRoutingKey(string eventType);
    }
}
```