using System.Linq;
using Caliburn.Micro;
using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    public class KeySortedSetViewModel : Screen, IHandle<TreeItemSelectedMessage>, IHandle<AddKeyMessage>, IValueItem
    {
        private BindableCollection<ScoreWrapper> keyValuesListBox;

        public BindableCollection<ScoreWrapper> KeyValuesListBox
        {
            get { return keyValuesListBox; }
            set
            {
                keyValuesListBox = value ?? new BindableCollection<ScoreWrapper>();
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
            if (message != null && message.SelectedItem is RedisKeySortedSet && !message.SelectedItem.HasChildren)
            {
                DisplayValue((RedisKeySortedSet)message.SelectedItem);
            }
        }

        private void DisplayValue(RedisKeySortedSet item)
        {
            if (item != null)
            {
                var value = item.KeyValues;

                KeyValuesListBox = new BindableCollection<ScoreWrapper>(value.Select((itemvalue, index) => new ScoreWrapper { RowNumber = index + 1, Item = itemvalue.Element, Score = itemvalue.Score }));
            }
        }

        public void Handle(AddKeyMessage message)
        {
            KeyValuesListBox = new BindableCollection<ScoreWrapper>();
        }
    }
}
