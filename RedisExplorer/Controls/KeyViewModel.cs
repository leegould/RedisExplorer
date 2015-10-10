using System.ComponentModel.Composition;

using Caliburn.Micro;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    [Export(typeof(KeyViewModel))]
    public class KeyViewModel : Screen, IHandle<TreeItemSelectedMessage>, IHandle<AddKeyMessage>
    {
        #region Members

        private RedisKey item;

        private readonly IEventAggregator eventAggregator;

        private bool hasSelected;
        private string keyNameTextBox;
        private string keyValueTextBox;
        private string ttlLabel;
        private string ttlSecondsLabel;
        private string typeLabel;

        #endregion

        #region Properties

        public bool HasSelected
        {
            get
            {
                return hasSelected;
            }
            set
            {
                hasSelected = value;
                NotifyOfPropertyChange(() => HasSelected);
            }
        }

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
                typeLabel = value;
                NotifyOfPropertyChange(() => TypeLabel);
            }
        }

        #endregion

        public KeyViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);

            SetDefault();
        }

        public void SetDefault()
        {
            TypeLabel = "Type";
            TTLLabel = "None";
            TTLSecondsLabel = "None";
        }

        #region Button Actions

        public void SaveButton()
        {
            item.KeyName = keyNameTextBox; 
            if (item.SaveValue(KeyValueTextBox))
            {
                
            }
        }

        public void DeleteButton()
        {
            if (item.Delete())
            {
                
            }
        }

        #endregion

        #region Message Handlers

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message != null && message.SelectedItem != null && message.SelectedItem.GetType() == typeof(RedisKey) && !message.SelectedItem.HasChildren)
            {
                item = message.SelectedItem as RedisKey;

                if (item != null)
                {
                    KeyNameTextBox = item.GetKeyName();
                    TypeLabel = item.GetKeyType().ToString();

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

        public void Handle(AddKeyMessage message)
        {
            item = new RedisKey(message.ParentDatabase, eventAggregator);
            SetDefault();
            KeyNameTextBox = string.Empty;
            KeyValueTextBox = string.Empty;
        }

        #endregion
    }
}
