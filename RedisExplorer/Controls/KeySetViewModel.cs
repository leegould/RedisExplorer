using System.Collections.Generic;
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
        public class StringWrapper
        {
            public string Item { get; set; }
        }

        private ObservableCollection<StringWrapper> keyValuesListBox;
        private readonly IEventAggregator eventAggregator;

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

                KeyValuesListBox = new ObservableCollection<StringWrapper>(value.Select(x => new StringWrapper { Item = x}));
            }
        }
    }
}
