using System.Linq;

using Caliburn.Micro;

using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    public class KeyListViewModel : Screen, IHandle<TreeItemSelectedMessage>, IHandle<AddKeyMessage>, IKeyValue<BindableCollection<NumberedStringWrapper>>, IValueItem, IHandle<RedisKeyReloadMessage>
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

        protected override void OnActivate()
        {
            if (KeyValue == null)
            {
                keyValue = new BindableCollection<NumberedStringWrapper>();
            }
            base.OnActivate();
        }

        public KeyListViewModel(IEventAggregator eventAggregator)
        {
            var eAggregator = eventAggregator;
            eAggregator.Subscribe(this);
        }

        #region Message Handlers

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message != null && message.SelectedItem is RedisKeyList && !message.SelectedItem.HasChildren)
            {
                DisplayValue((RedisKeyList)message.SelectedItem);
            }
        }

        public void Handle(AddKeyMessage message)
        {
            KeyValue = new BindableCollection<NumberedStringWrapper>();
        }

        public void Handle(RedisKeyReloadMessage message)
        {
            DisplayValue((RedisKeyList)message.Item);
        }

        #endregion

        #region Private

        private void DisplayValue(RedisKeyList item)
        {
            if (item != null)
            {
                var value = item.KeyValue;

                KeyValue = new BindableCollection<NumberedStringWrapper>(value.Select((itemvalue, index) => new NumberedStringWrapper { RowNumber = index + 1, Item = itemvalue }));
            }
        }

        #endregion
    }
}
