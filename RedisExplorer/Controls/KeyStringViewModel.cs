using Caliburn.Micro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    public class KeyStringViewModel : Screen, IHandle<TreeItemSelectedMessage> 
    {
        private RedisKeyString item;
        private string keyValueTextBox;
        private readonly IEventAggregator eventAggregator;

        public string KeyValueTextBox
        {
            get
            {
                return keyValueTextBox;
            }
            set
            {
                keyValueTextBox = value;
                NotifyOfPropertyChange(() => KeyValueTextBox);
            }
        }

        public KeyStringViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
        }

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message != null && message.SelectedItem != null && !message.SelectedItem.HasChildren)
            {
                item = message.SelectedItem as RedisKeyString;

                DisplayStringValue();
            }
        }

        private void DisplayStringValue()
        {
            //var stringItem = item as RedisKeyString;
            if (item != null)
            {
                var value = item.KeyValue;

                try
                {
                    KeyValueTextBox = JObject.Parse(value).ToString(Formatting.Indented);
                }
                catch (JsonReaderException)
                {
                    KeyValueTextBox = value;
                }
            }
        }
    }
}
