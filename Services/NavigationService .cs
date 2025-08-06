using System.Windows;
using System.Windows.Controls;

namespace MyWpfMvvmApp.Services
{
    public class NavigationService : INavigationService
    {
        private Frame? _mainFrame;

        public void SetNavigationFrame(Frame frame)
        {
            _mainFrame = frame;
        }

        public void NavigateToPage<T>() where T : Page, new()
        {
            NavigateToPage(new T());
        }

        public void NavigateToPage(Page page)
        {
            if (_mainFrame == null)
                throw new InvalidOperationException("Navigation frame is not set. Call SetNavigationFrame first.");

            if (Application.Current.Dispatcher.CheckAccess())
            {
                _mainFrame.Navigate(page);
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() => _mainFrame.Navigate(page));
            }
        }
    }
}
