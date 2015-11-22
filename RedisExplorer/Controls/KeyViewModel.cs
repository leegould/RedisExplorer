using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using RedisExplorer.Messages;
using RedisExplorer.Models;
using StackExchange.Redis;
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

        public KeyStringViewModel KeyStringViewModel { get; set; }

        //public KeySetViewModel KeySetViewModel { get; set; }

        private Screen currentDataView { get; set; }

        public Screen CurrentDataView
        {
            get
            {
                return currentDataView;
            }
            set
            {
                currentDataView = value;
                NotifyOfPropertyChange(() => CurrentDataView);
            }
        }
        
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

            KeyStringViewModel = new KeyStringViewModel(eventAggregator);
            KeyStringViewModel.ConductWith(this);

            //KeySetViewModel = new KeySetViewModel(eventAggregator);
            //KeySetViewModel.ConductWith(this);

            CurrentDataView = KeyStringViewModel;

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
            DisplayItem(item);
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
            if (message != null && message.SelectedItem is RedisKey && !message.SelectedItem.HasChildren)
            {
                if (message.SelectedItem is RedisKeyString)
                {
                    CurrentDataView = KeyStringViewModel;
                }
                //else if (message.SelectedItem is RedisKeySet)
                //{
                //    CurrentDataView = KeySetViewModel;
                //}

                item = message.SelectedItem as RedisKey;
                DisplayItem(item);
            }
        }

        private void DisplayItem(RedisKey item)
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
            }
        }

        public void Handle(AddKeyMessage message)
        {
            item = new RedisKeyString(message.ParentDatabase, eventAggregator);
            SetDefault();
            KeyNameTextBox = message.KeyBase;
            //KeyStringViewModel.Item = item;
        }

        #endregion
    }
}
