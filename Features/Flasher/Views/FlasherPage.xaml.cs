using MyWpfMvvmApp.Features.Flasher.ViewModels;
using System.Windows.Controls;
using MyWpfMvvmApp.Services;

namespace MyWpfMvvmApp.Features.Flasher.Views
{
    public partial class FlasherPage : Page
    {
        public FlasherPage()
        {
            InitializeComponent();
            // 從 DI 容器取得 ViewModel
            DataContext = ServiceProvider.GetService<FlasherPageViewModel>();
        }
    }
}
