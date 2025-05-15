using System;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Eiffel.Services.RabbitMQ
{
    public class RabbitMqConnectionManager : IDisposable
    {
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private readonly SemaphoreSlim _connLock = new SemaphoreSlim(1, 1);
        private readonly object _reconnectLock = new object();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _isReconnecting;
        private bool _disposed;

        public RabbitMqConnectionManager(ConnectionFactory factory)
        {
            _factory = factory;
            // kick off initial connect (fire‐and‐forget)
            _ = ConnectAsync();
        }

        public async Task<IChannel> GetChannelAsync()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RabbitMqConnectionManager));

            // ensure the connection is open
            await ConnectAsync().ConfigureAwait(false);

            // enable publisher confirms on this channel
            var channelOptions = new CreateChannelOptions(
                publisherConfirmationsEnabled: true,
                publisherConfirmationTrackingEnabled: false
            );

            // create and return the channel with confirms turned on
            return await _connection
                .CreateChannelAsync(channelOptions, cancellationToken: _cts.Token)
                .ConfigureAwait(false);
        }


        private async Task ConnectAsync()
        {
            await _connLock.WaitAsync(_cts.Token).ConfigureAwait(false);
            try
            {
                if (_connection?.IsOpen == true)
                {
                    return;
                }
                Console.WriteLine($"[Connecting to RabbitMQ] {_factory.HostName}:{_factory.Port} vhost={_factory.VirtualHost}");
                var endpoints = new[] { new AmqpTcpEndpoint(_factory.HostName, _factory.Port) };
                _connection = await _factory.CreateConnectionAsync(endpoints, cancellationToken: _cts.Token).ConfigureAwait(false);
                // subscribe to the async shutdown event
                _connection.ConnectionShutdownAsync += OnConnectionShutdownAsync;
                Console.WriteLine("[Connecting to RabbitMQ] established");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Connecting to RabbitMQ] failed: {ex.Message}");
                _ = ReconnectAsync();
            }
            finally
            {
                _connLock.Release();
            }
        }

        private Task OnConnectionShutdownAsync(object sender, ShutdownEventArgs @event)
        {
            Console.WriteLine("[RabbitMQ Shutdown] connection lost, scheduling reconnect...");
            return ReconnectAsync();
        }

        private async Task ReconnectAsync()
        {
            lock (_reconnectLock)
            {
                if (_isReconnecting) return;
                _isReconnecting = true;
            }

            try
            {
                int delay = 10_000, maxDelay = 300_000, maxRetries = 600, retry = 0;

                while (!_cts.IsCancellationRequested)
                {
                    if (_disposed) break;

                    retry++;
                    if (retry > maxRetries)
                    {
                        Console.WriteLine($"[RabbitMQ Retry #{retry}] max retries reached, giving up");
                        break;
                    }

                    bool shouldLog =
                        retry <= 10
                        || (retry <= 49 && retry % 5 == 0)
                        || (retry > 49 && retry % 10 == 0);

                    if (shouldLog)
                        Console.WriteLine($"[RabbitMQ Retry #{retry}] reconnecting...");

                    try
                    {
                        await ConnectAsync().ConfigureAwait(false);
                        if (_connection?.IsOpen == true)
                        {
                            if (shouldLog)
                                Console.WriteLine($"[RabbitMQ Retry #{retry}] reconnected!");
                            break;
                        }
                    }
                    catch (Exception ex) when (shouldLog)
                    {
                        Console.WriteLine($"[RabbitMQ Retry #{retry}] failed: {ex.Message}");
                        Console.WriteLine($"[RabbitMQ Retry #{retry}] waiting {delay / 1000}s...");
                    }

                    await Task.Delay(delay, _cts.Token).ConfigureAwait(false);
                    delay = Math.Min(maxDelay, delay * 2);
                }
            }
            catch (TaskCanceledException) { /* shutting down */ }
            finally
            {
                lock (_reconnectLock)
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
            _connection?.Dispose();
            _cts.Dispose();
            _connLock.Dispose();
        }
    }
}
