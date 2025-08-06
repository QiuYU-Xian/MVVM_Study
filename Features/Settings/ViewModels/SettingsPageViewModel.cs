using Microsoft.Win32;
using MyWpfMvvmApp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyWpfMvvmApp.Features.Settings.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        #region 屬性

        private string _appName = "My WPF MVVM App";
        public string AppName
        {
            get => _appName;
            set => SetProperty(ref _appName, value);
        }

        public string Version => "1.0.0";

        private string _selectedLanguage = "繁體中文";
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set => SetProperty(ref _selectedLanguage, value);
        }

        public ObservableCollection<string> Languages { get; } = new()
        {
            "繁體中文", "簡體中文", "English", "日本語"
        };

        private bool _isLightTheme = true;
        public bool IsLightTheme
        {
            get => _isLightTheme;
            set
            {
                if (SetProperty(ref _isLightTheme, value) && value)
                {
                    IsDarkTheme = false;
                }
            }
        }

        private bool _isDarkTheme = false;
        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (SetProperty(ref _isDarkTheme, value) && value)
                {
                    IsLightTheme = false;
                }
            }
        }

        private double _fontSize = 14;
        public double FontSize
        {
            get => _fontSize;
            set => SetProperty(ref _fontSize, value);
        }

        private bool _enableAnimations = true;
        public bool EnableAnimations
        {
            get => _enableAnimations;
            set => SetProperty(ref _enableAnimations, value);
        }

        private bool _autoSave = true;
        public bool AutoSave
        {
            get => _autoSave;
            set => SetProperty(ref _autoSave, value);
        }

        private int _saveInterval = 5;
        public int SaveInterval
        {
            get => _saveInterval;
            set => SetProperty(ref _saveInterval, value);
        }

        public ObservableCollection<int> SaveIntervals { get; } = new()
        {
            1, 2, 5, 10, 15, 30
        };

        private string _dataPath = @"C:\MyApp\Data";
        public string DataPath
        {
            get => _dataPath;
            set => SetProperty(ref _dataPath, value);
        }

        private string _testMessage = "這是一個測試訊息";
        public string TestMessage
        {
            get => _testMessage;
            set => SetProperty(ref _testMessage, value);
        }

        #endregion

        #region 命令

        public ICommand BrowseDataPathCommand { get; }
        public ICommand ResetDefaultsCommand { get; }
        public ICommand SaveSettingsCommand { get; }

        #endregion

        public SettingsPageViewModel()
        {
            BrowseDataPathCommand = new RelayCommand(BrowseDataPath);
            ResetDefaultsCommand = new RelayCommand(ResetDefaults);
            SaveSettingsCommand = new RelayCommand(SaveSettings);
        }

        #region 命令實作

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

        private void SaveSettings()
        {
            // 這裡可以實作儲存設定到檔案或登錄檔的邏輯
            // 為了示範，我們只是顯示一個簡單的訊息
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
