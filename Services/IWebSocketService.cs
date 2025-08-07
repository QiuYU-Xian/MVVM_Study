
namespace MyWpfMvvmApp.Services
{
    public interface IWebSocketService
    {
        // 連線狀態
        bool IsConnected { get; }

        // 連線事件
        event EventHandler<bool> ConnectionStateChanged;
        event EventHandler<string> MessageReceived;
        event EventHandler<string> ErrorOccurred;

        // 連線控制
        Task ConnectAsync(string url);
        Task DisconnectAsync();

        // 發送訊息
        Task SendMessageAsync(string message);
        Task SendCommandAsync(object command);

        // 燒入器專用方法
        Task StartFlashingAsync(string firmwarePath, string deviceType);
        Task StopFlashingAsync();
        Task GetDeviceListAsync();
        Task GetFlashProgressAsync();
    }
}
