using System.Windows.Controls;
using MyWpfMvvmApp.Features.Settings.ViewModels;
using MyWpfMvvmApp.Services;

namespace MyWpfMvvmApp.Features.Settings.Views
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            // 從 DI 容器取得 ViewModel
            DataContext = ServiceProvider.GetService<SettingsPageViewModel>();
        }
    }
}