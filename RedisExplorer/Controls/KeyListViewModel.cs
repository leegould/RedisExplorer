using System.Linq;

using Caliburn.Micro;

using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;

using StackExchange.Redis;

namespace RedisExplorer.Controls
{
    public class KeyListViewModel : Screen, IHandle<TreeItemSelectedMessage>, IHandle<RedisKeyUpdatedMessage>, IValueItem
    {
        private BindableCollection<NumberedStringWrapper> keyValuesListBox;

        public BindableCollection<NumberedStringWrapper> KeyValuesListBox
        {
            get
            {
                return keyValuesListBox;
            }
            set
            {
                keyValuesListBox = value ?? new BindableCollection<NumberedStringWrapper>();
                NotifyOfPropertyChange(() => KeyValuesListBox);
            }
        }

        protected override void OnActivate()
        {
            if (KeyValuesListBox == null)
            {
                keyValuesListBox = new BindableCollection<NumberedStringWrapper>();
            }
            base.OnActivate();
        }

        public KeyListViewModel(IEventAggregator eventAggregator)
        {
            var eAggregator = eventAggregator;
            eAggregator.Subscribe(this);
        }

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message != null && message.SelectedItem is RedisKeyList && !message.SelectedItem.HasChildren)
            {
                DisplayValue((RedisKeyList)message.SelectedItem);
            }
        }

        private void DisplayValue(RedisKeyList item)
        {
            if (item != null)
            {
                var value = item.KeyValues;

                KeyValuesListBox = new BindableCollection<NumberedStringWrapper>(value.Select((itemvalue, index) => new NumberedStringWrapper { RowNumber = index + 1, Item = itemvalue }));
            }
        }

        public void Handle(RedisKeyUpdatedMessage message)
        {
            if (message.Type == RedisType.List)
            {
                
            }
        }
    }
}
