using System.Collections.ObjectModel;
using System.ComponentModel;

using RedisExplorer.Models;

namespace RedisExplorer.Interface
{
    interface ITreeViewItemViewModel : INotifyPropertyChanged
    {
        ObservableCollection<TreeViewItem> Children { get; }
        bool HasDummyChild { get; }
        bool IsExpanded { get; set; }
        bool IsSelected { get; set; }
        TreeViewItem Parent { get; }
        string Display { get; set; }
    }
}
