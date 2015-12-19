using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    public class KeySortedSetViewModel : Screen, IHandle<TreeItemSelectedMessage>, IHandle<AddKeyMessage>, IValueItem
    {
        private BindableCollection<NumberedStringWrapper> keyValuesListBox;

        public BindableCollection<NumberedStringWrapper> KeyValuesListBox
        {
            get { return keyValuesListBox; }
            set
            {
                keyValuesListBox = value ?? new BindableCollection<NumberedStringWrapper>();
                NotifyOfPropertyChange(() => KeyValuesListBox);
            }
        }

        public KeySortedSetViewModel(IEventAggregator eventAggregator)
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

                KeyValuesListBox = new BindableCollection<NumberedStringWrapper>(value.Select((itemvalue, index) => new NumberedStringWrapper { RowNumber = index + 1, Item = itemvalue }));
            }
        }

        public void Handle(AddKeyMessage message)
        {
            KeyValuesListBox = new BindableCollection<NumberedStringWrapper>();
        }
    }
}
