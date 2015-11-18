using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedisExplorer.Messages;
using RedisExplorer.Models;

using StackExchange.Redis;

using Action = System.Action;
using RedisKey = RedisExplorer.Models.RedisKey;

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
        private DateTime? ttlDateTimePicker;
        private RedisType selectedType;

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

        public DateTime? TTLDateTimePicker
        {
            get
            {
                return ttlDateTimePicker;
            }
            set
            {
                ttlDateTimePicker = value;
                NotifyOfPropertyChange(() => TTLDateTimePicker);
            }
        }

        public RedisType SelectedType
        {
            get { return selectedType; }
            set
            {
                selectedType = value;
                NotifyOfPropertyChange(() => SelectedType);
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
            SelectedType = RedisType.String;
            TTLDateTimePicker = null;
        }

        public IEnumerable<RedisType> RedisTypeValues
        {
            get
            {
                return Enum.GetValues(typeof(RedisType)).Cast<RedisType>();
            }
        }

        #region Button Actions

        public void SaveButton()
        {
            if (item == null)
            {
                return;
            }
            item.KeyName = keyNameTextBox;

            var stringItem = item as RedisKeyString;
            if (stringItem != null)
            {
                stringItem.KeyValue = keyValueTextBox;
            }

            if (TTLDateTimePicker.HasValue)
            {
                item.TTL = new TimeSpan((TTLDateTimePicker.Value - DateTime.Now).Ticks);
            }

            item.Save();
        }

        public void DeleteButton()
        {
            if (item == null)
            {
                return;
            }
            item.Delete();
        }

        public void ReloadButton()
        {
            if (item == null)
            {
                return;
            }
            item.Reload();
            DisplayItem();
        }

        public void ClearButton()
        {
            TTLDateTimePicker = null;
        }

        public void OneHourButton()
        {
            TTLDateTimePicker = DateTime.Now.AddHours(1);
        }

        public void TwentyFourHoursButton()
        {
            TTLDateTimePicker = DateTime.Now.AddHours(24);
        }

        #endregion

        #region Message Handlers

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message != null && message.SelectedItem != null && !message.SelectedItem.HasChildren)
            {
                item = message.SelectedItem as RedisKeyString;

                DisplayItem();
            }
        }

        private void DisplayItem()
        {
            if (item != null)
            {
                KeyNameTextBox = item.KeyName;
                SelectedType = item.KeyType;

                var ttl = item.TTL;
                if (ttl.HasValue)
                {
                    TTLDateTimePicker = DateTime.Now + ttl.Value;
                }
                else
                {
                    TTLDateTimePicker = null;
                }

                if (item.KeyType == RedisType.String)
                {
                    DisplayStringValue();
                }
            }
        }

        private void DisplayStringValue()
        {
            var stringItem = item as RedisKeyString;
            if (stringItem != null)
            {
                var value = stringItem.KeyValue;
                
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

        public void Handle(AddKeyMessage message)
        {
            item = new RedisKeyString(message.ParentDatabase, eventAggregator);
            SetDefault();
            KeyNameTextBox = message.KeyBase;
            KeyValueTextBox = string.Empty;
        }

        #endregion
    }
}
