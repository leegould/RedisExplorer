using System.Collections.ObjectModel;
using System.ComponentModel;

using RedisExplorer.Models;

namespace RedisExplorer.Interface
{
    interface ITreeViewItemViewModel : INotifyPropertyChanged
    {
        ObservableCollection<TreeViewItemViewModel> Children { get; }
        bool HasDummyChild { get; }
        bool IsExpanded { get; set; }
        bool IsSelected { get; set; }
        TreeViewItemViewModel Parent { get; }
    }
}
