using MyWpfMvvmApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyWpfMvvmApp.Features.Home.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        private string _message = "Hello MVVM World!";

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ICommand UpdateTimeCommand { get; }

        public HomePageViewModel()
        {
            UpdateTimeCommand = new RelayCommand(() =>
                Message = $"當前時間: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        }
    }
}
