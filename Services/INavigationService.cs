
using System.Windows.Controls;

namespace MyWpfMvvmApp.Services
{
    public interface INavigationService
    {
        void NavigateToPage<T>() where T : Page, new();
        void NavigateToPage(Page page);
        void SetNavigationFrame(Frame frame);
    }
}
