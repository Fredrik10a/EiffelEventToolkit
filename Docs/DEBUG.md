# Debug Guide

This guide helps developers set up and debug the `EiffelEventToolkit` project by using the `EiffelEventToolkit.Debug` console application as a testing environment.

To setup a local debug Environment use the `compose.yml` file which have a local RabbitMQ and Logstash instance defined.

## 1. Start Docker

```sh
cd EiffelEventToolkit.Debug

docker compose up
```

Verify that both RabbitMQ and Logstash started successfuly, you could need to create the Exchange and or the queue manually via the Localhost RabbitMQ UI http://localhost:15672/#/

The Debug Console APP use a Docker RabbitMQ and Logstash instance for message management. The APP simulates a transmission and could be observed in the Console of the Logstash instance.

## 2. Start the project

The easiest is to build and start a debug session in the Visual Studio UI. Before starting the Debug session, be sure to define relevant settings within the appsettings.json file.