using System.Collections.ObjectModel;
using System.Linq;

using Caliburn.Micro;

using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    public class KeySetViewModel : Screen, IHandle<TreeItemSelectedMessage>, IValueItem
    {
        private BindableCollection<NumberedStringWrapper> keyValuesListBox;
        private readonly IEventAggregator eventAggregator;

        public BindableCollection<NumberedStringWrapper> KeyValuesListBox
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

        public KeySetViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
        }

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message != null && message.SelectedItem is RedisKeySet && !message.SelectedItem.HasChildren)
            {
                DisplayValue(message.SelectedItem as RedisKeySet);
            }
        }

        private void DisplayValue(RedisKeySet item)
        {
            if (item != null)
            {
                var value = item.KeyValues;

                KeyValuesListBox = new BindableCollection<NumberedStringWrapper>(value.Select((itemvalue, index) => new NumberedStringWrapper { RowNumber = index + 1, Item = itemvalue }));
            }
        }
    }
}
