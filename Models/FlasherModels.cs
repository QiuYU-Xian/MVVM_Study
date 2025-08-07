
using System.Collections.ObjectModel;

namespace MyWpfMvvmApp.Models
{
    public class DeviceInfo
    {
        public string Port { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsConnected { get; set; }
        public string Status { get; set; } = "未連接";
    }

    public class FlashProgress
    {
        public int Percentage { get; set; }
        public string CurrentStep { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public TimeSpan ElapsedTime { get; set; }
    }

    public class FirmwareInfo
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string Version { get; set; } = string.Empty;
        public string DeviceType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class FlashSession
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DeviceInfo Device { get; set; } = new();
        public FirmwareInfo Firmware { get; set; } = new();
        public FlashProgress Progress { get; set; } = new();
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ObservableCollection<string> Logs { get; set; } = new();
    }

    public class ConnectionSettings
    {
        public string WebSocketUrl { get; set; } = "ws://localhost:8080";
        public int TimeoutSeconds { get; set; } = 30;
        public bool AutoReconnect { get; set; } = true;
        public int ReconnectInterval { get; set; } = 5;
    }
}
