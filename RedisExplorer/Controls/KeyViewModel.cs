using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;

using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;
using StackExchange.Redis;
using RedisKey = RedisExplorer.Models.RedisKey;

namespace RedisExplorer.Controls
{
    [Export(typeof(KeyViewModel))]
    public class KeyViewModel : Conductor<IValueItem>.Collection.OneActive, IHandle<TreeItemSelectedMessage>, IHandle<AddKeyMessage>
    {
        #region Members

        private RedisKey item;

        private readonly IEventAggregator eventAggregator;

        private bool hasSelected;
        private string keyNameTextBox;
        private DateTime? ttlDateTimePicker;
        private RedisType selectedType;

        private KeyStringViewModel keyStringViewModel { get; set; }
        public KeySetViewModel keySetViewModel { get; set; }
        public KeyListViewModel keyListViewModel { get; set; }
        public KeyHashViewModel keyHashViewModel { get; set; }
        public KeySortedSetViewModel keySortedSetViewModel { get; set; }
        
        #endregion

        #region Properties

        public KeyStringViewModel KeyStringViewModel
        {
            get
            {
                return keyStringViewModel;
            }
            set
            {
                keyStringViewModel = value;
                NotifyOfPropertyChange(() => KeyStringViewModel);
            }
        }

        public KeySetViewModel KeySetViewModel 
        {
            get
            {
                return keySetViewModel;
            }
            set
            {
                keySetViewModel = value;
                NotifyOfPropertyChange(() => KeySetViewModel);
            }
        }

        public KeyListViewModel KeyListViewModel
        {
            get
            {
                return keyListViewModel;
            }
            set
            {
                keyListViewModel = value;
                NotifyOfPropertyChange(() => KeyListViewModel);
            }
        }

        public KeyHashViewModel KeyHashViewModel
        {
            get
            {
                return keyHashViewModel;
            }
            set
            {
                keyHashViewModel = value;
                NotifyOfPropertyChange(() => KeyHashViewModel);
            }
        }

        public KeySortedSetViewModel KeySortedSetViewModel
        {
            get
            {
                return keySortedSetViewModel;
            }
            set
            {
                keySortedSetViewModel = value;
                NotifyOfPropertyChange(() => KeySortedSetViewModel);
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

                if (item != null)
                { 
                    var redisTypeViewModelMap = new Dictionary<RedisType, IValueItem>
                                                    {
                                                        { RedisType.String, KeyStringViewModel },
                                                        { RedisType.Set, KeySetViewModel },
                                                        { RedisType.List, KeyListViewModel },
                                                        { RedisType.Hash, KeyHashViewModel },
                                                        { RedisType.SortedSet, KeySortedSetViewModel }
                                                    };

                    if (!Maps.RedisTypeKeyMap.ContainsKey(selectedType))
                    {
                        selectedType = RedisType.String;
                    }

                    var olditem = item;
                    item = (RedisKey)Activator.CreateInstance(Maps.RedisTypeKeyMap[selectedType], item.Parent, eventAggregator);
                    if (item != null)
                    {
                        item.KeyName = olditem.KeyName;
                        item.KeyType = selectedType;
                        item.TTL = olditem.TTL;
                        item.Display = olditem.KeyName;
                        ActivateItem(redisTypeViewModelMap[selectedType]);
                    }
                }

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

            KeySetViewModel = new KeySetViewModel(eventAggregator);
            KeySetViewModel.ConductWith(this);
                
            KeyListViewModel = new KeyListViewModel(eventAggregator);
            KeyListViewModel.ConductWith(this);

            KeyHashViewModel = new KeyHashViewModel(eventAggregator);
            KeyHashViewModel.ConductWith(this);

            KeySortedSetViewModel = new KeySortedSetViewModel(eventAggregator);
            KeySortedSetViewModel.ConductWith(this);

            Items.Add(KeyStringViewModel);
            Items.Add(KeySetViewModel);
            Items.Add(KeyListViewModel);
            Items.Add(KeyHashViewModel);
            Items.Add(KeySortedSetViewModel);

            ActivateItem(KeyStringViewModel);

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
            item.KeyType = SelectedType;

            if (TTLDateTimePicker.HasValue)
            {
                item.TTL = new TimeSpan((TTLDateTimePicker.Value - DateTime.Now).Ticks);
            }

            if (SelectedType == RedisType.String)
            {
                ((RedisKeyString)item).KeyValue = ((KeyStringViewModel)ActiveItem).KeyValue;
            }
            else if (SelectedType == RedisType.Set)
            {
                var value = ((KeySetViewModel)ActiveItem).KeyValue;
                if (value != null)
                {
                    ((RedisKeySet) item).KeyValue = value.Select(x => x.Item).ToList();
                }
            }
            else if (SelectedType == RedisType.List)
            {
                var value = ((KeyListViewModel) ActiveItem).KeyValue;
                if (value != null)
                {
                    ((RedisKeyList)item).KeyValue = value.Select(x => x.Item).ToList();
                }
            }
            else if (SelectedType == RedisType.Hash)
            {
                var value = ((KeyHashViewModel)ActiveItem).KeyValue;
                if (value != null)
                {
                    ((RedisKeyHash)item).KeyValue = value.ToDictionary(x => x.Key, x => x.Value);
                }
            }
            else if (SelectedType == RedisType.SortedSet)
            {
                var value = ((KeySortedSetViewModel) ActiveItem).KeyValue;
                if (value != null)
                {
                    ((RedisKeySortedSet) item).KeyValue = value.Select(x => new SortedSetEntry(x.Item, x.Score)).ToList(); 
                }
            }

            if (item.Save())
            {
                item.NotifyOfSave();
            }
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
                if (message.SelectedItem is RedisKeySet)
                {
                    ActivateItem(KeySetViewModel);
                }
                else if (message.SelectedItem is RedisKeyList)
                {
                    ActivateItem(KeyListViewModel);
                } 
                else if (message.SelectedItem is RedisKeyHash)
                {
                    ActivateItem(KeyHashViewModel);
                }
                else if (message.SelectedItem is RedisKeySortedSet)
                {
                    ActivateItem(KeySortedSetViewModel);
                }
                else
                {
                    ActivateItem(KeyStringViewModel);                    
                }

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
        }

        #endregion
    }
}
