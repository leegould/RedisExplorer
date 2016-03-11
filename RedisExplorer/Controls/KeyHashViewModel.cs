using System.Linq;
using Caliburn.Micro;
using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    public class KeyHashViewModel : Screen, IHandle<TreeItemSelectedMessage>, IHandle<AddKeyMessage>, IKeyValue<BindableCollection<HashWrapper>>, IValueItem, IHandle<RedisKeyReloadMessage>
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

        #region Message Handlers

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message?.SelectedItem is RedisKeyHash && !message.SelectedItem.HasChildren)
            {
                DisplayValue((RedisKeyHash)message.SelectedItem);
            }
        }

        public void Handle(AddKeyMessage message)
        {
            KeyValue = new BindableCollection<HashWrapper>();
        }

        public void Handle(RedisKeyReloadMessage message)
        {
            var redisKeyHash = message.Item as RedisKeyHash;
            if (redisKeyHash != null)
            {
                DisplayValue(redisKeyHash);
            }
        }

        #endregion 

        #region Private

        private void DisplayValue(RedisKeyHash item)
        {
            if (item != null)
            {
                var value = item.KeyValue;

                KeyValue = new BindableCollection<HashWrapper>(value.Select(x => new HashWrapper { Key = x.Key, Value = x.Value }));
            }
        }

        #endregion
    }
}
