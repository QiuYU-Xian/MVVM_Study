using MyWpfMvvmApp.Services;
using System.Windows;

namespace MyWpfMvvmApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // 設定依賴注入容器
            ServiceProvider.ConfigureServices();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // 清理服務容器
            ServiceProvider.Dispose();
            base.OnExit(e);
        }
    }

}
