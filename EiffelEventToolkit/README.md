# Eiffel event toolkit

This guide will show you an example of how to use the library.

## Table of Contents
1. [Abstract](#abstract)
2. [Installation](#installation)
3. [Configuration Setup](#configuration-setup)
4. [Eiffel Event Toolkit with RabbitMQ](#eiffel-event-toolkit-with-rabbitmq)
5. [Eiffel Event Toolkit with GraphQL API](#eiffel-event-toolkit-with-graphql-api)

## Abstract

**INFO**: This library uses a ***C# class representation*** of each event where each class has been generated from its specific `.json` schema file.

When `PublishEiffelEventAsync()` is called by the user, the library will serialize the C# class object into a JSON object and publish it.

**The file structure:**
```[EVENT_TYPE]/[VERSION]/[EVENT_TYPE].cs```

**How to know which event to use?** 
Start of by reading the abstract of those events relevant for your use-case. [eiffel-vocabulary](https://github.com/eiffel-community/eiffel/tree/master/eiffel-vocabulary)

**Additional information:**

[Event Categories](https://github.com/eiffel-community/eiffel/blob/master/eiffel-syntax-and-usage/event-categories.md) this is helpful when defining the routingKey.

Some examples of routingKeys (*only applicable when using RabbitMQ as the `connect method`*):
```bash
eiffel.[Category].[Name]._._
eiffel.definition.EiffelCompositionDefinedEvent._._
eiffel.test.EiffelTestCaseStartedEvent._._
eiffel.activity.EiffelActivityStartedEvent._._
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
If you need to both fetch and publish GraphQL or ALL is applicable. If you use `ALL` configuration for both RabbitMQ 
and GraphQL is needed.
```sh
EIFFEL_CONNECT_TARGET = // "RabbitMQ", "GraphQL" or "ALL"
```
Enable a connection to relevant RabbitMQ server.
```sh
EIFFEL_RABBITMQ_HOSTNAME
EIFFEL_RABBITMQ_PORT
EIFFEL_RABBITMQ_VHOST
EIFFEL_RABBITMQ_USER
EIFFEL_RABBITMQ_PASSWORD
```
Enable a connection to GraphQL API.
```sh
EIFFEL_GRAPHQL_URI
EIFFEL_GRAPHQL_TOKEN
```

### The startup.cs or program.cs

To see the latest version of a use-case see the debug application `program.cs`, link at the top of this page as an 'Example'.

# Eiffel event toolkit with RabbitMQ

## Prerequisites
To be able to publish single events toward the RabbitMQ Eiffel bus, RabbitMQ has to be defined as the Connecting method.

## Implementation

```csharp
// An example
object eiffelEvent = null;
string exchange = "mx.eiffel"; // Required for RabbitMQ
string routingKey = "eiffel.[CATEGORY].[EVENT_TYPE]._._";

(bool verdict,string message) = await _eiffelEventToolkit.PublishEiffelEventAsync(eiffelEvent, exchange, routingKey);

// ...
// Handle the Verdict and message 

Console.WriteLine($"Verdict: {verdict}, Message: {message}");
```

# Eiffel event toolkit with GraphQL API

An example on how to use GraphQL API for quering.

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

