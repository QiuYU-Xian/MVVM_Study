using CommunityToolkit.Mvvm.ComponentModel;

namespace MyWpfMvvmApp.Helpers
{
    // 現在只需要繼承 ObservableObject 即可
    public abstract class BaseViewModel : ObservableObject
    {
        // CommunityToolkit.Mvvm 已經提供了所有必要的功能
        // 不需要手動實作 INotifyPropertyChanged
        // SetProperty 方法也已經包含在 ObservableObject 中
    }
}
