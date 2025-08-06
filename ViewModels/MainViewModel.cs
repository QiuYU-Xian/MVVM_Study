using CommunityToolkit.Mvvm.Input;
using MyWpfMvvmApp.Features.Home.Views;
using MyWpfMvvmApp.Features.Settings.Views;
using MyWpfMvvmApp.Helpers;
using MyWpfMvvmApp.Services;
using MyWpfMvvmApp.Views;
using System.Diagnostics;
using System.Windows;

namespace MyWpfMvvmApp.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // 預設顯示首頁
            ShowHomePage();
        }

        [RelayCommand]
        private void ShowHomePage()
        {
            // 使用依賴注入取得頁面實例
            var homePage = ServiceProvider.GetService<HomePage>();
            _navigationService.NavigateToPage(homePage);
        }

        [RelayCommand]
        private void ShowSettingsPage()
        {
            // 使用依賴注入取得頁面實例
            var settingsPage = ServiceProvider.GetService<SettingsPage>();
            _navigationService.NavigateToPage(settingsPage);
        }
    }
}
