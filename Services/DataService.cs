using CommunityToolkit.Mvvm.ComponentModel;
using MyWpfMvvmApp.Models;

namespace MyWpfMvvmApp.Services
{
    public partial class DataService : ObservableObject
    {
        // 移除單例模式，改用 DI 管理
        [ObservableProperty]
        private SampleData sampleData = new();
        
        // 如果需要，可以加入其他資料操作方法
        public void UpdateSampleData(string name, int value)
        {
            SampleData.Name = name;
            SampleData.Value = value;
            SampleData.CreatedAt = DateTime.Now;
        }
    }
}
