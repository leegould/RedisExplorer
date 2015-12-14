using System.Linq;
using Caliburn.Micro;
using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    public class KeyHashViewModel: Screen, IHandle<TreeItemSelectedMessage>, IHandle<AddKeyMessage>, IValueItem
    {
        private BindableCollection<HashWrapper> keyValuesDict;

        public BindableCollection<HashWrapper> KeyValuesDict
        {
            get
            {
                return keyValuesDict;
            }
            set
            {
                keyValuesDict = value ?? new BindableCollection<HashWrapper>();
                NotifyOfPropertyChange(() => KeyValuesDict);
            }
        }

        protected override void OnActivate()
        {
            if (KeyValuesDict == null)
            {
                KeyValuesDict = new BindableCollection<HashWrapper>();
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
            if (message != null && message.SelectedItem is RedisKeyHash && !message.SelectedItem.HasChildren)
            {
                DisplayValue((RedisKeyHash)message.SelectedItem);
            }
        }

        private void DisplayValue(RedisKeyHash item)
        {
            if (item != null)
            {
                var value = item.KeyValues;

                KeyValuesDict = new BindableCollection<HashWrapper>(value.Select(x => new HashWrapper { Key = x.Key, Value = x.Value }));
            }
        }

        public void Handle(AddKeyMessage message)
        {
            KeyValuesDict = new BindableCollection<HashWrapper>();
        }
    }
}
