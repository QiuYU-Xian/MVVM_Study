using MyWpfMvvmApp.Services;
using MyWpfMvvmApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MyWpfMvvmApp.Views
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // 先設定導航服務的 Frame 參考
            var navigationService = ServiceProvider.GetService<INavigationService>();
            navigationService.SetNavigationFrame(MainFrame);

            // 再設定 DataContext (這時 MainViewModel 建構函數會執行)
            DataContext = ServiceProvider.GetService<MainViewModel>();
        }
    }
}
