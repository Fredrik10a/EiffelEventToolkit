# Solution Structure

This solution is organized into multiple projects to separate concerns like core functionality, testing, and tooling.

**EiffelEventToolkit**: The main library project.
- **/Interfaces**: Interface definitions for distribution contracts.
- **/Models**: C# .NET models representing Eiffel events.
- **/Schemas**: Embedded JSON schema definitions for validation.
- **/Services**: Core service implementations for EiffelEventToolkit.
- **/Services/Distributors**: Distribution logic for RabbitMQ and GraphQL.
- **/Services/Distributors/Models**: Custom models used internally when interacting with endpoints.
- **/Services/RabbitMQ**: RabbitMQ connection management.

**EiffelEventToolkit.Debug**: A console application designed for testing and debugging the EiffelEventToolkit functionality.
- **/config**: Configuration files and Docker setup for local debugging.

**EiffelEventToolkit.Test**: NUnit-based unit test project.

**EventModelGenerator**: A separate tool that generates C# models for EiffelEventToolkit based on JSON schemas.
