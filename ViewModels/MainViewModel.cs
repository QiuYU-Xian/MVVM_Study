using CommunityToolkit.Mvvm.Input;
using MyWpfMvvmApp.Features.Home.Views;
using MyWpfMvvmApp.Features.Settings.Views;
using MyWpfMvvmApp.Helpers;
using MyWpfMvvmApp.Views;
using System.Diagnostics;
using System.Windows;

namespace MyWpfMvvmApp.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            // 預設顯示首頁
            ShowHomePage();
        }


        [RelayCommand]
        private void ShowHomePage()
        {
            Debug.WriteLine("Navigating to Home Page");
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (MainWindow.Instance != null)
                    MainWindow.Instance.NavigateToPage(new HomePage());
            });
        }

        [RelayCommand]
        private void ShowSettingsPage()
        {
            Debug.WriteLine("Navigating to Settings Page");
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (MainWindow.Instance != null)
                    MainWindow.Instance.NavigateToPage(new SettingsPage());
            });
        }
    }
}
