using MyWpfMvvmApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWpfMvvmApp.Services
{
    public class DataService : INotifyPropertyChanged
    {
        private static readonly DataService _instance = new();
        public static DataService Instance => _instance;

        private SampleData _sampleData = new();
        public SampleData SampleData
        {
            get => _sampleData;
            set
            {
                _sampleData = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SampleData)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
