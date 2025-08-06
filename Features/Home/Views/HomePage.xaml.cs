using MyWpfMvvmApp.Features.Home.ViewModels;
using System.Windows.Controls;
using MyWpfMvvmApp.Services;

namespace MyWpfMvvmApp.Features.Home.Views
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            // 從 DI 容器取得 ViewModel
            DataContext = ServiceProvider.GetService<HomePageViewModel>();
        }
    }
}
