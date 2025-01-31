using System;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Eiffel.Services.RabbitMQ
{
    public class RabbitMqConnectionManager : IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private bool _disposed;
        private readonly object _lock = new object();

        public RabbitMqConnectionManager(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            Connect();
        }

        public IModel GetChannel()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(RabbitMqConnectionManager));
            }

            lock (_lock)
            {
                if (_connection == null || !_connection.IsOpen)
                {
                    Connect();
                }
                return _connection.CreateModel();
            }
        }

        private void Connect()
        {
            lock (_lock)
            {
                if (_connection != null && _connection.IsOpen)
                {
                    return;
                }
                try
                {
                    Console.WriteLine($"Connecting to RabbitMQ at {_connectionFactory.HostName}:{_connectionFactory.Port} on VirtualHost {_connectionFactory.VirtualHost}");
                    _connection = _connectionFactory.CreateConnection();
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    Console.WriteLine("RabbitMQ connection established.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to connect to RabbitMQ: {ex.Message}");
                    Reconnect();
                }
            }
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("RabbitMQ connection was shutdown, trying to reconnect...");
            Reconnect();
        }

        private void Reconnect()
        {
            int delay = 10000; // Start with a 10-second delay
            while (true)
            {
                try
                {
                    Connect();
                    if (_connection.IsOpen)
                    {
                        Console.WriteLine("Reconnected to RabbitMQ.");
                        break;
                    }
                }
                catch
                {
                    Console.WriteLine("Retrying RabbitMQ connection...");
                    Task.Delay(delay).Wait();
                    delay = Math.Min(120000, delay * 2); // Exponential backoff up to 2 minutes (120,000 ms)
                }
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _connection?.Dispose();
        }
    }
}
