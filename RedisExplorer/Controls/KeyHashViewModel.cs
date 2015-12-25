using System.Linq;
using Caliburn.Micro;
using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    public class KeyHashViewModel : Screen, IHandle<TreeItemSelectedMessage>, IHandle<AddKeyMessage>, IBindableValueItem<HashWrapper>, IValueItem
    {
        private BindableCollection<HashWrapper> keyValue;

        public BindableCollection<HashWrapper> KeyValue
        {
            get
            {
                return keyValue;
            }
            set
            {
                keyValue = value ?? new BindableCollection<HashWrapper>();
                NotifyOfPropertyChange(() => KeyValue);
            }
        }

        protected override void OnActivate()
        {
            if (KeyValue == null)
            {
                KeyValue = new BindableCollection<HashWrapper>();
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
                var value = item.KeyValue;

                KeyValue = new BindableCollection<HashWrapper>(value.Select(x => new HashWrapper { Key = x.Key, Value = x.Value }));
            }
        }

        public void Handle(AddKeyMessage message)
        {
            KeyValue = new BindableCollection<HashWrapper>();
        }
    }
}
