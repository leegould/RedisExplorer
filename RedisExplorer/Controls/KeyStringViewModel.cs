using Caliburn.Micro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    public class KeyStringViewModel : Screen, IHandle<TreeItemSelectedMessage>, IValueItem
    {
        private string keyValue;
        private readonly IEventAggregator eventAggregator;

        public string KeyValue
        {
            get
            {
                return keyValue;
            }
            set
            {
                keyValue = value;
                NotifyOfPropertyChange(() => KeyValue);
            }
        }

        public KeyStringViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
        }

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message != null && message.SelectedItem is RedisKeyString && !message.SelectedItem.HasChildren)
            {
                DisplayStringValue(message.SelectedItem as RedisKeyString);
            }
        }

        private void DisplayStringValue(RedisKeyString item)
        {
            if (item != null)
            {
                var value = item.KeyValue;

                try
                {
                    KeyValue = JObject.Parse(value).ToString(Formatting.Indented);
                }
                catch (JsonReaderException)
                {
                    KeyValue = value;
                }
            }
        }
    }
}
