using Microsoft.Extensions.DependencyInjection;
using MyWpfMvvmApp.Features.Home.ViewModels;
using MyWpfMvvmApp.Features.Home.Views;
using MyWpfMvvmApp.Features.Settings.ViewModels;
using MyWpfMvvmApp.Features.Settings.Views;
using MyWpfMvvmApp.ViewModels;


namespace MyWpfMvvmApp.Services
{
    public static class ServiceProvider
    {
        private static IServiceProvider? _serviceProvider;

        public static IServiceProvider Current => _serviceProvider
            ?? throw new InvalidOperationException("Service provider is not initialized. Call ConfigureServices first.");

        public static void ConfigureServices()
        {
            var services = new ServiceCollection();

            // 註冊服務
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<DataService>();

            // 註冊 ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<HomePageViewModel>();
            services.AddTransient<SettingsPageViewModel>();

            // 註冊 Views (Pages)
            services.AddTransient<HomePage>();
            services.AddTransient<SettingsPage>();

            _serviceProvider = services.BuildServiceProvider();
        }

        public static T GetService<T>() where T : notnull
        {
            return Current.GetRequiredService<T>();
        }

        public static void Dispose()
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
