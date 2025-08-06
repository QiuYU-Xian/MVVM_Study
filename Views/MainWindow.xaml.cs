using System.Windows;
using System.Windows.Controls;
using MyWpfMvvmApp.ViewModels;
namespace MyWpfMvvmApp.Views
{

    public partial class MainWindow : Window
    {
        public static MainWindow? Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            DataContext = new MainViewModel();
        }
        public void NavigateToPage(Page page)
        {
            MainFrame.Navigate(page);
        }
    }
}
