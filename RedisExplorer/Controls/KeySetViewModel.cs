using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Caliburn.Micro;

using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    public class KeySetViewModel : Screen, IHandle<TreeItemSelectedMessage> 
    {
        public class StringWrapper
        {
            public string Item { get; set; }
        }

        private RedisKeySet item;
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
            if (message != null && message.SelectedItem != null && !message.SelectedItem.HasChildren)
            {
                item = message.SelectedItem as RedisKeySet;

                DisplayValue();
            }
        }

        private void DisplayValue()
        {
            if (item != null)
            {
                var value = item.KeyValues;

                KeyValuesListBox = new ObservableCollection<StringWrapper>(value.Select(x => new StringWrapper { Item = x}));
            }
        }
    }
}
