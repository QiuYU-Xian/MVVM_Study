using CommunityToolkit.Mvvm.ComponentModel;
using MyWpfMvvmApp.Models;

namespace MyWpfMvvmApp.Services
{
    public partial class DataService : ObservableObject
    {
        private static readonly DataService _instance = new();
        public static DataService Instance => _instance;

        [ObservableProperty]
        private SampleData sampleData = new();
    }
}
