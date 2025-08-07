using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyWpfMvvmApp.Helpers;
using MyWpfMvvmApp.Services;
using MyWpfMvvmApp.Models;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Windows;

namespace MyWpfMvvmApp.Features.Flasher.ViewModels
{
    public partial class FlasherPageViewModel : BaseViewModel
    {
        private readonly IWebSocketService _webSocketService;

        public FlasherPageViewModel(IWebSocketService webSocketService)
        {
            _webSocketService = webSocketService;

            // 訂閱 WebSocket 事件
            _webSocketService.ConnectionStateChanged += OnConnectionStateChanged;
            _webSocketService.MessageReceived += OnMessageReceived;
            _webSocketService.ErrorOccurred += OnErrorOccurred;
        }

        #region Observable Properties

        [ObservableProperty]
        private bool isConnected = false;

        [ObservableProperty]
        private string connectionStatus = "未連接";

        [ObservableProperty]
        private string webSocketUrl = "ws://localhost:8080";

        [ObservableProperty]
        private string selectedFirmwarePath = string.Empty;

        [ObservableProperty]
        private DeviceInfo? selectedDevice;

        [ObservableProperty]
        private int flashProgress = 0;

        [ObservableProperty]
        private string flashStatus = "準備就緒";

        [ObservableProperty]
        private bool isFlashing = false;

        [ObservableProperty]
        private string currentLog = string.Empty;

        public ObservableCollection<DeviceInfo> AvailableDevices { get; } = new();
        public ObservableCollection<string> LogMessages { get; } = new();

        #endregion

        #region Commands

        [RelayCommand]
        private async Task ConnectWebSocket()
        {
            try
            {
                ConnectionStatus = "連接中...";
                await _webSocketService.ConnectAsync(WebSocketUrl);

                // 連接成功後獲取設備列表
                await RefreshDeviceList();
            }
            catch (Exception ex)
            {
                ConnectionStatus = $"連接失敗: {ex.Message}";
                AddLog($"連接錯誤: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task DisconnectWebSocket()
        {
            try
            {
                await _webSocketService.DisconnectAsync();
                ConnectionStatus = "已斷線";
                AvailableDevices.Clear();
            }
            catch (Exception ex)
            {
                AddLog($"斷線錯誤: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task RefreshDeviceList()
        {
            if (!IsConnected) return;

            try
            {
                await _webSocketService.GetDeviceListAsync();
                AddLog("正在刷新設備列表...");
            }
            catch (Exception ex)
            {
                AddLog($"刷新設備列表錯誤: {ex.Message}");
            }
        }

        [RelayCommand]
        private void SelectFirmware()
        {
            var dialog = new OpenFileDialog
            {
                Title = "選擇韌體檔案",
                Filter = "韌體檔案|*.hex;*.bin;*.elf|所有檔案|*.*",
                CheckFileExists = true
            };

            if (dialog.ShowDialog() == true)
            {
                SelectedFirmwarePath = dialog.FileName;
                AddLog($"已選擇韌體: {Path.GetFileName(SelectedFirmwarePath)}");
            }
        }

        [RelayCommand(CanExecute = nameof(CanStartFlash))]
        private async Task StartFlash()
        {
            if (SelectedDevice == null || string.IsNullOrEmpty(SelectedFirmwarePath))
                return;

            try
            {
                IsFlashing = true;
                FlashProgress = 0;
                FlashStatus = "開始燒入...";

                await _webSocketService.StartFlashingAsync(SelectedFirmwarePath, SelectedDevice.Type);
                AddLog($"開始燒入 {Path.GetFileName(SelectedFirmwarePath)} 到 {SelectedDevice.Name}");
            }
            catch (Exception ex)
            {
                IsFlashing = false;
                FlashStatus = $"燒入失敗: {ex.Message}";
                AddLog($"燒入錯誤: {ex.Message}");
            }
        }

        [RelayCommand(CanExecute = nameof(CanStopFlash))]
        private async Task StopFlash()
        {
            try
            {
                await _webSocketService.StopFlashingAsync();
                IsFlashing = false;
                FlashStatus = "已停止燒入";
                AddLog("燒入作業已停止");
            }
            catch (Exception ex)
            {
                AddLog($"停止燒入錯誤: {ex.Message}");
            }
        }

        [RelayCommand]
        private void ClearLogs()
        {
            LogMessages.Clear();
            CurrentLog = string.Empty;
        }

        #endregion

        #region Command CanExecute Methods

        private bool CanStartFlash()
        {
            return IsConnected && !IsFlashing && SelectedDevice != null && !string.IsNullOrEmpty(SelectedFirmwarePath);
        }

        private bool CanStopFlash()
        {
            return IsConnected && IsFlashing;
        }

        #endregion

        #region Event Handlers

        private void OnConnectionStateChanged(object? sender, bool isConnected)
        {
            // 確保在 UI 執行緒上執行
            Application.Current.Dispatcher.Invoke(() =>
            {
                IsConnected = isConnected;
                ConnectionStatus = isConnected ? "已連接" : "未連接";

                if (!isConnected)
                {
                    AvailableDevices.Clear();
                    IsFlashing = false;
                }

                // 更新命令狀態
                StartFlashCommand.NotifyCanExecuteChanged();
                StopFlashCommand.NotifyCanExecuteChanged();
            });
        }

        private void OnMessageReceived(object? sender, string message)
        {
            // 確保在 UI 執行緒上執行
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    var data = JsonConvert.DeserializeObject<dynamic>(message);
                    HandleServerMessage(data);
                }
                catch (Exception ex)
                {
                    AddLog($"解析伺服器訊息錯誤: {ex.Message}");
                }
            });
        }

        private void OnErrorOccurred(object? sender, string error)
        {
            // 確保在 UI 執行緒上執行
            Application.Current.Dispatcher.Invoke(() =>
            {
                AddLog($"WebSocket 錯誤: {error}");
            });
        }

        private void HandleServerMessage(dynamic data)
        {
            string messageType = data.type?.ToString() ?? "";

            switch (messageType)
            {
                case "device_list":
                    UpdateDeviceList(data.devices);
                    break;

                case "flash_progress":
                    UpdateFlashProgress(data);
                    break;

                case "flash_complete":
                    OnFlashComplete(data);
                    break;

                case "flash_error":
                    OnFlashError(data);
                    break;

                case "log":
                    AddLog(data.message?.ToString() ?? "");
                    break;
            }
        }

        private void UpdateDeviceList(dynamic devices)
        {
            // 確保在 UI 執行緒上執行
            Application.Current.Dispatcher.Invoke(() =>
            {
                AvailableDevices.Clear();

                if (devices != null)
                {
                    foreach (var device in devices)
                    {
                        AvailableDevices.Add(new DeviceInfo
                        {
                            Port = device.port?.ToString() ?? "",
                            Name = device.name?.ToString() ?? "",
                            Type = device.type?.ToString() ?? "",
                            IsConnected = device.connected?.ToObject<bool>() ?? false,
                            Status = device.status?.ToString() ?? ""
                        });
                    }
                }

                AddLog($"找到 {AvailableDevices.Count} 個設備");
            });
        }

        private void UpdateFlashProgress(dynamic data)
        {
            // 確保在 UI 執行緒上執行
            Application.Current.Dispatcher.Invoke(() =>
            {
                FlashProgress = data.percentage?.ToObject<int>() ?? 0;
                FlashStatus = data.status?.ToString() ?? "";

                if (data.step != null)
                {
                    AddLog($"[{FlashProgress}%] {data.step}");
                }
            });
        }

        private void OnFlashComplete(dynamic data)
        {
            // 確保在 UI 執行緒上執行
            Application.Current.Dispatcher.Invoke(() =>
            {
                IsFlashing = false;
                FlashProgress = 100;
                FlashStatus = "燒入完成";
                AddLog("韌體燒入成功完成！");

                // 更新命令狀態
                StartFlashCommand.NotifyCanExecuteChanged();
                StopFlashCommand.NotifyCanExecuteChanged();
            });
        }

        private void OnFlashError(dynamic data)
        {
            // 確保在 UI 執行緒上執行
            Application.Current.Dispatcher.Invoke(() =>
            {
                IsFlashing = false;
                FlashStatus = $"燒入失敗: {data.error}";
                AddLog($"燒入錯誤: {data.error}");

                // 更新命令狀態
                StartFlashCommand.NotifyCanExecuteChanged();
                StopFlashCommand.NotifyCanExecuteChanged();
            });
        }

        #endregion

        #region Helper Methods

        private void AddLog(string message)
        {
            var logEntry = $"[{DateTime.Now:HH:mm:ss}] {message}";

            // 確保在 UI 執行緒上執行
            if (Application.Current.Dispatcher.CheckAccess())
            {
                // 已經在 UI 執行緒上
                LogMessages.Add(logEntry);
                CurrentLog = logEntry;

                // 限制日誌數量
                while (LogMessages.Count > 1000)
                {
                    LogMessages.RemoveAt(0);
                }
            }
            else
            {
                // 需要切換到 UI 執行緒
                Application.Current.Dispatcher.Invoke(() =>
                {
                    LogMessages.Add(logEntry);
                    CurrentLog = logEntry;

                    // 限制日誌數量
                    while (LogMessages.Count > 1000)
                    {
                        LogMessages.RemoveAt(0);
                    }
                });
            }
        }

        #endregion
    }
}
