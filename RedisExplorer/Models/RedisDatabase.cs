using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using RedisExplorer.Messages;
using RedisExplorer.Properties;
using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisDatabase : TreeViewItem, IHandle<RedisKeyUpdatedMessage>, IHandle<RedisKeyAddedMessage>, IHandle<KeyDeletedMessage>, IHandle<KeysDeletedMessage>
    {
        private IEventAggregator eventAggregator { get; set; }

        private RedisServer parent { get; set; }

        private int dbNumber { get; set; }

        private int maxKeys { get; set; }

        private string urnSeparator { get; set; }

        private int keyCount { get; set; }

        private string display { get; set; }

        public RedisDatabase(RedisServer parent, int dbnumber, IEventAggregator eventAggregator, int keycount = 0) : base(parent, Settings.Default.LazyLoadDatabase, eventAggregator)
        {
            this.parent = parent;
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            dbNumber = dbnumber;
            maxKeys = string.IsNullOrEmpty(Settings.Default.MaxKeys) ? 1000 : int.Parse(Settings.Default.MaxKeys);
            urnSeparator = string.IsNullOrEmpty(Settings.Default.UrnSeparator) ? ":" : Settings.Default.UrnSeparator;
            keyCount = keycount;
            SetDisplay();
        }

        public IDatabase GetDatabase(int? dbnum = null)
        {
            return parent.GetDatabase(dbnum ?? dbNumber);
        }

        public override string Display
        {
            get
            {
                return display;
            }
            set
            {
                display = value;
                NotifyOfPropertyChange(() => Display);
            }
        }

        public int GetDatabaseNumber
        {
            get { return dbNumber; }
        }

        public int GetKeyCount
        {
            get { return keyCount; }
        }

        protected override void LoadChildren()
        {
            var db = GetDatabase();
            if (db != null)
            {
                var keys = parent.GetServer().Keys(db.Database, "*", maxKeys);

                foreach (var key in keys)
                {
                    var ktype = db.KeyType(key);
                    var parts = new Queue<string>(key.ToString().Split(new [] { urnSeparator }, StringSplitOptions.RemoveEmptyEntries));
                    if (parts.Count > 0)
                    {
                        AddChildren(this, parts, ktype, eventAggregator);
                    }
                }
            }
        }
        
        private static void AddChildren(TreeViewItem item, Queue<string> urn, RedisType ktype, IEventAggregator eventAggregator)
        {
            var keystr = urn.Dequeue();
            var key = item.Children.FirstOrDefault(x => x.Display == keystr);
            if (key == null)
            {
                key = RedisKeyFactory.Get(ktype, item, eventAggregator);
                key.Display = keystr;
                item.Children.Add(key);
            }

            if (urn.Count > 0)
            {
                AddChildren(key, urn, ktype, eventAggregator);
            }
        }
        
        public void Reload()
        {
            eventAggregator.PublishOnUIThread(new DatabaseReloadMessage { DbNumber = dbNumber });

            ReloadDatabase();
        }

        public void Flush()
        {
            var s = parent.GetServer();
            s.FlushDatabase(dbNumber);

            eventAggregator.PublishOnUIThread(new FlushDbMessage { dbNumber = dbNumber });
            
            ReloadDatabase();
        }

        public void Add()
        {
            eventAggregator.PublishOnUIThread(new AddKeyMessage { ParentDatabase = this });
        }

        public void Handle(RedisKeyUpdatedMessage message)
        {
            if (dbNumber == message.Key.DatabaseName)
            {
                ReloadDatabase();

                //RedisKey key = null;
                //var step = Children;
                //foreach (var node in message.Key.KeyName.Split(new[] { urnSeparator }, StringSplitOptions.RemoveEmptyEntries).Select(keypart => step.FirstOrDefault(x => x.Display == keypart)))
                //{
                //    if (node != null && node.HasChildren)
                //    {
                //        step = node.Children;
                //    }
                //    else
                //    {
                //        key = node as RedisKey;
                //    }
                //}

                var key = FindChildNode(Children, message.Key.KeyName.Split(new[] {urnSeparator}, StringSplitOptions.RemoveEmptyEntries).ToList());

                if (key != null)
                {
                    key.IsExpanded = true;
                    key.IsSelected = true;
                }
            }
        }

        private TreeViewItem FindChildNode(ObservableCollection<TreeViewItem> items, List<string> path)
        {
            var item = items.FirstOrDefault(x => x.Display == path.FirstOrDefault());
            if (item == null)
            {
                return null;
            }
            if (!item.HasChildren)
            {
                return item;
            }
            path.RemoveAt(0);
            return FindChildNode(item.Children, path);
        }

        public void Handle(RedisKeyAddedMessage message)
        {
            ChangeKeyCount(message.Key.DatabaseName, 1);
        }
        
        public void Handle(KeyDeletedMessage message)
        {
            ChangeKeyCount(message.Key.DatabaseName, -1);
        }

        public void Handle(KeysDeletedMessage message)
        {
            ChangeKeyCount(message.DatabaseName, -message.Keys.Count);
        }


        private static class RedisKeyFactory
        {
            public static RedisKey Get(RedisType redisType, TreeViewItem item, IEventAggregator eventAggregator)
            {
                RedisKey key;
                switch (redisType)
                {
                    case RedisType.String:
                        key = new RedisKeyString(item, eventAggregator);
                        break;
                    case RedisType.Set:
                        key = new RedisKeySet(item, eventAggregator);
                        break;
                    case RedisType.List:
                        key = new RedisKeyList(item, eventAggregator);
                        break;
                    case RedisType.Hash:
                        key = new RedisKeyHash(item, eventAggregator);
                        break;
                    case RedisType.SortedSet:
                        key = new RedisKeySortedSet(item, eventAggregator);
                        break;
                    default:
                        key = new RedisKeyString(item, eventAggregator);
                        break;
                }
                return key;
            }
        }

        private void SetDisplay()
        {
            Display = dbNumber + " (" + keyCount + ")";
        }

        private void ReloadDatabase()
        {
            Children.Clear();

            LoadChildren();
        }

        private void ChangeKeyCount(int dbnumber, int keymodifier)
        {
            if (dbNumber == dbnumber)
            {
                keyCount = keyCount + keymodifier;
                SetDisplay();
                ReloadDatabase();
            }
        }
    }
}
