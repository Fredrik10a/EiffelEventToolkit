# Eiffel Event Toolkit

The **Eiffel Event Toolkit** is a .NET Standard 2.0 library designed to create, manage, validate, and distribute messages that adhere to the [Eiffel Protocol](https://github.com/eiffel-community).  
It simplifies message handling according to the Eiffel standard, using RabbitMQ or GraphQL for communication.

## Table of contents

### Users
- [How to use](./EiffelEventToolkit/README.md)

### Contributors
- [Project overview](./Docs/PROJECT.md)
- [Debug guide](./Docs/DEBUG.md)
- [License](./Docs/LICENSE.txt)

## Features

- Create and manage **Eiffel** event messages using strongly-typed .NET model templates.
- Validate event messages against official JSON schemas before publishing.
- Publish events to a RabbitMQ bus.
- Query and publish events using a GraphQL API.
