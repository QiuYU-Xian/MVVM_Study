using System.Windows.Controls;
using MyWpfMvvmApp.Features.Settings.ViewModels;

namespace MyWpfMvvmApp.Features.Settings.Views
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            // 修正：設定 DataContext
            DataContext = new SettingsPageViewModel();
        }
    }
}
