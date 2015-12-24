using System.Linq;

using Caliburn.Micro;

using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    public class KeySetViewModel : Screen, IHandle<TreeItemSelectedMessage>, IHandle<AddKeyMessage>, IBindableValueItem<NumberedStringWrapper>, IValueItem
    {
        private BindableCollection<NumberedStringWrapper> keyValue;

        public BindableCollection<NumberedStringWrapper> KeyValue
        {
            get
            {
                return keyValue;
            }
            set
            {
                keyValue = value ?? new BindableCollection<NumberedStringWrapper>();
                NotifyOfPropertyChange(() => KeyValue);
            }
        }

        public KeySetViewModel(IEventAggregator eventAggregator)
        {
            var eAggregator = eventAggregator;
            eAggregator.Subscribe(this);
        }

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message != null && message.SelectedItem is RedisKeySet && !message.SelectedItem.HasChildren)
            {
                DisplayValue((RedisKeySet)message.SelectedItem);
            }
        }

        private void DisplayValue(RedisKeySet item)
        {
            if (item != null)
            {
                var value = item.KeyValues;

                KeyValue = new BindableCollection<NumberedStringWrapper>(value.Select((itemvalue, index) => new NumberedStringWrapper { RowNumber = index + 1, Item = itemvalue }));
            }
        }

        public void Handle(AddKeyMessage message)
        {
            KeyValue = new BindableCollection<NumberedStringWrapper>();
        }
    }
}
