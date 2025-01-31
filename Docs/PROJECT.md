# Solution Structure

**EiffelEventToolkit**: The main `EiffelEventToolkit` project.
- **/Interfaces**: Interface definitions for distributors contracts.
- **/Models**: The Eiffel C# .NET  models.
- **/Schemas**: Json schemas
- **/Services**: Provides service implementation containing the core business logic for `EiffelEventToolkit`.
- **/Services/Distributors**: Types of distribution methods.
- **/Services/Distributors/Models**: Custom Models used upon managing an endpoint.
- **/Services/RabbitMQ**: The RabbitMQ connector.

**EiffelEventToolkit.Debug**: A console application designed for testing and debugging the main `EiffelEventToolkit` project.
- **/config**: Configurations for the debug/test environment for Docker using the console app.

**EiffelEventToolkit.Test**: NuUnit test project

**EventModelGenerator**: Generates C# Model types for the EiffelEventToolkit project using the Json Schema definitions
