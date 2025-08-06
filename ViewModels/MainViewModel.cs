using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using MyWpfMvvmApp.Helpers;
using MyWpfMvvmApp.Features.Home.Views;
using MyWpfMvvmApp.Features.Settings.Views;
using MyWpfMvvmApp.Views;
namespace MyWpfMvvmApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            ShowHomePageCommand = new RelayCommand(() => ShowHomePage());
            ShowSettingsPageCommand = new RelayCommand(() => ShowSettingsPage());

            // 預設顯示首頁
            ShowHomePage();
        }

        public ICommand ShowHomePageCommand { get; }
        public ICommand ShowSettingsPageCommand { get; }

        private void ShowHomePage()
        {
            Debug.WriteLine("Navigating to Home Page");
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (MainWindow.Instance != null)
                    MainWindow.Instance.NavigateToPage(new HomePage());
            });
        }

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
