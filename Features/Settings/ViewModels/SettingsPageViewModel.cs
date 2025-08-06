using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using MyWpfMvvmApp.Helpers;
using System.Collections.ObjectModel;

namespace MyWpfMvvmApp.Features.Settings.ViewModels
{
    public partial class SettingsPageViewModel : BaseViewModel
    {
        #region Observable Properties

        [ObservableProperty]
        private string appName = "My WPF MVVM App";

        public string Version => "1.0.0";

        [ObservableProperty]
        private string selectedLanguage = "繁體中文";

        public ObservableCollection<string> Languages { get; } = new()
        {
            "繁體中文", "簡體中文", "English", "日本語"
        };

        [ObservableProperty]
        private bool isLightTheme = true;

        [ObservableProperty]
        private bool isDarkTheme = false;

        [ObservableProperty]
        private double fontSize = 14;

        [ObservableProperty]
        private bool enableAnimations = true;

        [ObservableProperty]
        private bool autoSave = true;

        [ObservableProperty]
        private int saveInterval = 5;

        public ObservableCollection<int> SaveIntervals { get; } = new()
        {
            1, 2, 5, 10, 15, 30
        };

        [ObservableProperty]
        private string dataPath = @"C:\MyApp\Data";

        [ObservableProperty]
        private string testMessage = "這是一個測試訊息";

        #endregion

        #region Property Change Handlers

        // 當 IsLightTheme 改變時自動調用
        partial void OnIsLightThemeChanged(bool value)
        {
            if (value)
            {
                IsDarkTheme = false;
            }
        }

        // 當 IsDarkTheme 改變時自動調用
        partial void OnIsDarkThemeChanged(bool value)
        {
            if (value)
            {
                IsLightTheme = false;
            }
        }

        #endregion

        #region Relay Commands

        [RelayCommand]
        private void BrowseDataPath()
        {
            var dialog = new OpenFileDialog
            {
                Title = "選擇資料路徑",
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "選擇資料夾"
            };

            if (dialog.ShowDialog() == true)
            {
                DataPath = System.IO.Path.GetDirectoryName(dialog.FileName) ?? DataPath;
            }
        }

        [RelayCommand]
        private void ResetDefaults()
        {
            AppName = "My WPF MVVM App";
            SelectedLanguage = "繁體中文";
            IsLightTheme = true;
            FontSize = 14;
            EnableAnimations = true;
            AutoSave = true;
            SaveInterval = 5;
            DataPath = @"C:\MyApp\Data";
            TestMessage = "這是一個測試訊息";
        }

        [RelayCommand]
        private void SaveSettings()
        {
            // 這裡可以實作儲存設定到檔案或登錄檔的邏輯
            System.Windows.MessageBox.Show(
                $"設定已儲存！\n\n" +
                $"應用程式名稱: {AppName}\n" +
                $"語言: {SelectedLanguage}\n" +
                $"主題: {(IsLightTheme ? "淺色" : "深色")}\n" +
                $"字體大小: {FontSize}\n" +
                $"啟用動畫: {(EnableAnimations ? "是" : "否")}\n" +
                $"自動儲存: {(AutoSave ? "是" : "否")}\n" +
                $"儲存間隔: {SaveInterval} 分鐘\n" +
                $"資料路徑: {DataPath}",
                "設定儲存成功",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }

        #endregion
    }
}
