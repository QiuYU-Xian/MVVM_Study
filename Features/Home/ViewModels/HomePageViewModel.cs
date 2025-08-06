
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyWpfMvvmApp.Helpers;

namespace MyWpfMvvmApp.Features.Home.ViewModels
{
    public partial class HomePageViewModel : BaseViewModel
    {
        // 使用 [ObservableProperty] 自動生成屬性
        [ObservableProperty]
        private string message = "Hello MVVM World!";

        // 使用 [RelayCommand] 自動生成命令
        [RelayCommand]
        private void UpdateTime()
        {
            Message = $"當前時間: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        }
    }
}
