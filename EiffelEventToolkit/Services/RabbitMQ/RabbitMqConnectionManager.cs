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
        private bool _isReconnecting = false;
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

                if (_connection == null || !_connection.IsOpen)
                {
                    throw new InvalidOperationException("Cannot create channel: RabbitMQ connection is not established.");
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
                    _ = ReconnectAsync();
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
            lock (_lock)
            {
                if (_isReconnecting)
                    return; // reconnect-loop is already in progress

                _isReconnecting = true;
            }

            try
            {
                int delay = 10000; // Start with 10 seconds
                const int maxDelay = 300000; // 5 minutes
                const int maxRetries = 600; // Max number of retries
                int retryCount = 0;

                while (!_cts.IsCancellationRequested)
                {
                    if (_disposed)
                        break;

                    retryCount++;

                    if (retryCount > maxRetries)
                    {
                        Console.WriteLine($"[Retry #{retryCount}] Maximum retries ({maxRetries}) reached. Stopping reconnect attempts.");
                        break;
                    }

                    bool shouldLog = false;

                    if (retryCount <= 10)
                    {
                        shouldLog = true; // Log every retry for the first 10 attempts
                    }
                    else if (retryCount <= 49)
                    {
                        shouldLog = retryCount % 5 == 0; // Log every 5 retries between 11 and 49
                    }
                    else
                    {
                        shouldLog = retryCount % 10 == 0; // Log every 10 retries after 50
                    }

                    if (shouldLog)
                    {
                        Console.WriteLine($"[Retry #{retryCount}] Attempting to reconnect to RabbitMQ...");
                    }

                    try
                    {
                        Connect(); // Try to connect

                        if (_connection?.IsOpen == true)
                        {
                            Console.WriteLine($"[Retry #{retryCount}] Successfully reconnected to RabbitMQ!");
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (shouldLog)
                        {
                            Console.WriteLine($"[Retry #{retryCount}] Failed to connect: {ex.Message}");
                            Console.WriteLine($"[Retry #{retryCount}] Waiting {delay / 1000} seconds before next retry...");
                        }
                    }

                    try
                    {
                        await Task.Delay(delay, _cts.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        break; // Exited cleanly
                    }

                    delay = Math.Min(maxDelay, delay * 2); // Exponential backoff
                }
            }
            finally
            {
                lock (_lock)
                {
                    _isReconnecting = false;
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
