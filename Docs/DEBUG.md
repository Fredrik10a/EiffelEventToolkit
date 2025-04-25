# Debug Guide

This guide helps developers set up and debug the `EiffelEventToolkit` project using the `EiffelEventToolkit.Debug` console application as a local testing environment.

The debug application simulates the publishing of Eiffel events to a locally hosted RabbitMQ and Logstash instance defined in the `compose.yml` file.

---

## 1. Start Docker Services

Navigate to the debug project and start the Docker environment:

```bash
cd EiffelEventToolkit.Debug
docker compose up
```

Ensure that both **RabbitMQ** and **Logstash** containers have started successfully.  
You may need to manually create the **exchange** and/or **queue** via the RabbitMQ Management UI:  
[http://localhost:15672/#/](http://localhost:15672/#/)

The debug console application communicates with the local RabbitMQ and Logstash instances running in Docker.  
It simulates event publishing, and the resulting messages can be observed in the **Logstash container logs**.

---

## 2. Start the Project

The recommended way to start the project is by launching a debug session using **Visual Studio**:

1. Open the `EiffelEventToolkit.Debug` project.
2. Build the solution to restore dependencies and verify configurations.
3. Ensure that the `appsettings.json` file contains correct RabbitMQ settings (host, port, user, password, vhost).
4. Start the debugger (`F5`) to simulate event publishing.

> 🔧 Tip: You can override settings via environment variables if needed for testing multiple configurations.
