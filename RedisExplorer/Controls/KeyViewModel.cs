using System.ComponentModel.Composition;
using Caliburn.Micro;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    [Export(typeof(KeyViewModel))]
    public class KeyViewModel : Screen, IHandle<TreeItemSelectedMessage>
    {
        #region Members

        private readonly IEventAggregator eventAggregator;

        private string keyNameTextBox;
        private string keyValueTextBox;
        private string ttlLabel;
        private string ttlSecondsLabel;
        private string typeLabel;

        #endregion

        #region Properties

        public string KeyNameTextBox
        {
            get
            {
                return keyNameTextBox;
            }
            set
            {
                keyNameTextBox = value;
                NotifyOfPropertyChange(() => KeyNameTextBox);
            }
        }

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

        public string TTLLabel
        {
            get
            {
                return ttlLabel;
            }
            set
            {
                ttlLabel = value;
                NotifyOfPropertyChange(() => TTLLabel);
            }
        }

        public string TTLSecondsLabel
        {
            get
            {
                return ttlSecondsLabel;
            }
            set
            {
                ttlSecondsLabel = value;
                NotifyOfPropertyChange(() => TTLSecondsLabel);
            }
        }

        public string TypeLabel
        {
            get
            {
                return typeLabel;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    typeLabel = "Type";
                }
                else
                {
                    typeLabel = value;
                    NotifyOfPropertyChange(() => TypeLabel);
                }
            }
        }

        #endregion

        public KeyViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);

            TypeLabel = string.Empty;
        }

        #region Message Handlers

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message != null && message.SelectedItem != null && message.SelectedItem.GetType() == typeof(RedisKey) && !message.SelectedItem.HasChildren)
            {
                var item = message.SelectedItem as RedisKey;

                if (item != null)
                {
                    KeyNameTextBox = item.GetKeyName();
                    TypeLabel = item.GetType().ToString();

                    var ttl = item.GetTTL();
                    if (ttl.HasValue)
                    {
                        TTLLabel = ttl.Value.ToString();
                        TTLSecondsLabel = ttl.Value.TotalSeconds.ToString();
                    } 

                    var value = item.GetValue();

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

        #endregion
    }
}
