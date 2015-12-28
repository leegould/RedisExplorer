using System.Linq;
using Caliburn.Micro;
using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    public class KeySortedSetViewModel : Screen, IHandle<TreeItemSelectedMessage>, IHandle<AddKeyMessage>, IKeyValue<BindableCollection<ScoreWrapper>>, IValueItem
    {
        private BindableCollection<ScoreWrapper> keyValue;

        public BindableCollection<ScoreWrapper> KeyValue
        {
            get { return keyValue; }
            set
            {
                keyValue = value ?? new BindableCollection<ScoreWrapper>();
                NotifyOfPropertyChange(() => KeyValue);
            }
        }

        public KeySortedSetViewModel(IEventAggregator eventAggregator)
        {
            var eAggregator = eventAggregator;
            eAggregator.Subscribe(this);
        }

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message != null && message.SelectedItem is RedisKeySortedSet && !message.SelectedItem.HasChildren)
            {
                DisplayValue((RedisKeySortedSet)message.SelectedItem);
            }
        }

        private void DisplayValue(RedisKeySortedSet item)
        {
            if (item != null)
            {
                var value = item.KeyValue;

                KeyValue = new BindableCollection<ScoreWrapper>(value.Select((itemvalue, index) => new ScoreWrapper { RowNumber = index + 1, Item = itemvalue.Element, Score = itemvalue.Score }));
            }
        }

        public void Handle(AddKeyMessage message)
        {
            KeyValue = new BindableCollection<ScoreWrapper>();
        }
    }
}
