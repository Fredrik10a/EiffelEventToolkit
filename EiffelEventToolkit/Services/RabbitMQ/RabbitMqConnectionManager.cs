using System;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Eiffel.Services.RabbitMQ
{
    public class RabbitMqConnectionManager : IDisposable
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
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
                    _ = ReconnectAsync(); // fire-and-forget
                }
            }
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("RabbitMQ connection was shutdown, trying to reconnect...");
            _ = ReconnectAsync(); // fire-and-forget
        }

        private async Task ReconnectAsync()
        {
            int delay = 10000; // Start with 10 seconds
            const int maxDelay = 240000; // 4 minutes = 240,000 ms
            while (!_cts.IsCancellationRequested)
            {
                try
                {
                    Connect(); // Try to connect
                    if (_connection?.IsOpen == true)
                    {
                        Console.WriteLine("Reconnected to RabbitMQ.");
                        break; // stop retrying
                    }
                }
                catch
                {
                    Console.WriteLine($"Retrying RabbitMQ connection in {delay / 1000} seconds...");
                    try
                    {
                        await Task.Delay(delay, _cts.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        // Cancel requested — exit early
                        break;
                    }
                    delay = Math.Min(maxDelay, delay * 2); // Exponential backoff, up to 4 mins
                }
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _cts.Cancel();
            _cts.Dispose();
            _connection?.Dispose();
        }
    }
}
