using System.Collections.ObjectModel;
using System.Linq;

using Caliburn.Micro;

using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    public class KeyListViewModel : Screen, IHandle<TreeItemSelectedMessage>, IValueItem
    {
        private readonly IEventAggregator eventAggregator;

        private ObservableCollection<StringWrapper> keyValuesListBox;

        public ObservableCollection<StringWrapper> KeyValuesListBox
        {
            get
            {
                return keyValuesListBox;
            }
            set
            {
                keyValuesListBox = value;
                NotifyOfPropertyChange(() => KeyValuesListBox);
            }
        }

        public KeyListViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
        }

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message != null && message.SelectedItem is RedisKeyList && !message.SelectedItem.HasChildren)
            {
                DisplayValue((RedisKeyList)message.SelectedItem);
            }
        }

        private void DisplayValue(RedisKeyList item)
        {
            if (item != null)
            {
                var value = item.KeyValues;

                KeyValuesListBox = new ObservableCollection<StringWrapper>(value.Select(x => new StringWrapper { Item = x }));
            }
        }
    }
}
