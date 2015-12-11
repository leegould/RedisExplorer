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
    public class KeyHashViewModel: Screen, IHandle<TreeItemSelectedMessage>, IHandle<AddKeyMessage>, IValueItem
    {
        private BindableCollection<KeyValuePair<string, string>> keyValuesDict;

        public BindableCollection<KeyValuePair<string, string>> KeyValuesDict
        {
            get
            {
                return keyValuesDict;
            }
            set
            {
                keyValuesDict = value ?? new BindableCollection<KeyValuePair<string, string>>();
                NotifyOfPropertyChange(() => KeyValuesDict);
            }
        }

        protected override void OnActivate()
        {
            if (KeyValuesDict == null)
            {
                KeyValuesDict = new BindableCollection<KeyValuePair<string, string>>();
            }
            base.OnActivate();
        }

        public KeyHashViewModel(IEventAggregator eventAggregator)
        {
            var eAggregator = eventAggregator;
            eAggregator.Subscribe(this);
        }

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message != null && message.SelectedItem is RedisKeyList && !message.SelectedItem.HasChildren)
            {
                DisplayValue((RedisKeyHash)message.SelectedItem);
            }
        }

        private void DisplayValue(RedisKeyHash item)
        {
            if (item != null)
            {
                var value = item.KeyValues;

                KeyValuesDict = new BindableCollection<KeyValuePair<string, string>>(value.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)));
            }
        }

        public void Handle(AddKeyMessage message)
        {
            KeyValuesDict = new BindableCollection<KeyValuePair<string, string>>();
        }
    }
}
