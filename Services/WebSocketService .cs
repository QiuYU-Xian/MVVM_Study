using System;
using System.Threading.Tasks;
using Websocket.Client;
using Newtonsoft.Json;
using System.Reactive.Linq;
using Microsoft.Extensions.Logging;

namespace MyWpfMvvmApp.Services
{
    public class WebSocketService : IWebSocketService, IDisposable
    {
        private WebsocketClient? _client;
        private readonly ILogger<WebSocketService>? _logger;

        public WebSocketService(ILogger<WebSocketService>? logger = null)
        {
            _logger = logger;
        }

        #region Properties and Events

        public bool IsConnected => _client?.IsRunning ?? false;

        public event EventHandler<bool>? ConnectionStateChanged;
        public event EventHandler<string>? MessageReceived;
        public event EventHandler<string>? ErrorOccurred;

        #endregion

        #region Connection Management

        public async Task ConnectAsync(string url)
        {
            try
            {
                await DisconnectAsync();

                var uri = new Uri(url);
                _client = new WebsocketClient(uri);

                // 設定事件處理
                _client.ReconnectTimeout = TimeSpan.FromSeconds(30);
                _client.MessageReceived.Subscribe(OnMessageReceived);
                _client.ReconnectionHappened.Subscribe(OnReconnectionHappened);
                _client.DisconnectionHappened.Subscribe(OnDisconnectionHappened);

                await _client.Start();

                _logger?.LogInformation($"Connected to WebSocket: {url}");
                ConnectionStateChanged?.Invoke(this, true);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to connect to WebSocket");
                ErrorOccurred?.Invoke(this, $"連線失敗: {ex.Message}");
                throw;
            }
        }

        public async Task DisconnectAsync()
        {
            if (_client != null)
            {
                await _client.Stop(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "Client disconnecting");
                _client?.Dispose();
                _client = null;

                _logger?.LogInformation("Disconnected from WebSocket");
                ConnectionStateChanged?.Invoke(this, false);
            }
        }

        #endregion

        #region Message Handling

        private void OnMessageReceived(ResponseMessage message)
        {
            var content = message.Text;
            _logger?.LogDebug($"Received message: {content}");
            MessageReceived?.Invoke(this, content);
        }

        private void OnReconnectionHappened(ReconnectionInfo info)
        {
            _logger?.LogInformation($"WebSocket reconnected: {info.Type}");
            ConnectionStateChanged?.Invoke(this, true);
        }

        private void OnDisconnectionHappened(DisconnectionInfo info)
        {
            _logger?.LogWarning($"WebSocket disconnected: {info.Type} - {info.CloseStatus}");
            ConnectionStateChanged?.Invoke(this, false);
        }

        #endregion

        #region Send Messages

        public async Task SendMessageAsync(string message)
        {
            if (!IsConnected)
                throw new InvalidOperationException("WebSocket is not connected");

            _client!.Send(message);
            await Task.CompletedTask;
        }

        public async Task SendCommandAsync(object command)
        {
            var json = JsonConvert.SerializeObject(command);
            await SendMessageAsync(json);
        }

        #endregion

        #region Flasher-Specific Methods

        public async Task StartFlashingAsync(string firmwarePath, string deviceType)
        {
            var command = new
            {
                action = "start_flash",
                firmware_path = firmwarePath,
                device_type = deviceType,
                timestamp = DateTime.UtcNow
            };

            await SendCommandAsync(command);
        }

        public async Task StopFlashingAsync()
        {
            var command = new
            {
                action = "stop_flash",
                timestamp = DateTime.UtcNow
            };

            await SendCommandAsync(command);
        }

        public async Task GetDeviceListAsync()
        {
            var command = new
            {
                action = "get_devices",
                timestamp = DateTime.UtcNow
            };

            await SendCommandAsync(command);
        }

        public async Task GetFlashProgressAsync()
        {
            var command = new
            {
                action = "get_progress",
                timestamp = DateTime.UtcNow
            };

            await SendCommandAsync(command);
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            DisconnectAsync().Wait(1000);
            _client?.Dispose();
        }

        #endregion
    }
}
