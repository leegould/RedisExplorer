using System;
using System.Collections.Generic;
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

        public int GetDatabaseNumber => dbNumber;

        public int GetKeyCount => keyCount;

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
                
                ExpandChildNode(Children, message.Key.KeyName.Split(new[] {urnSeparator}, StringSplitOptions.RemoveEmptyEntries).ToList());
            }
        }

        public void Handle(RedisKeyAddedMessage message)
        {
            if (dbNumber == message.Key.DatabaseName)
            {
                ChangeKeyCount(1);

                ExpandChildNode(Children, message.Key.KeyName.Split(new[] { urnSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList());
            }
        }
        
        public void Handle(KeyDeletedMessage message)
        {
            if (dbNumber == message.Key.DatabaseName)
            {
                ChangeKeyCount(-1);

                var patharr = message.Key.KeyName.Split(new[] { urnSeparator }, StringSplitOptions.RemoveEmptyEntries);
                ExpandChildNode(Children, patharr.Take(patharr.Length - 1).ToList());

                eventAggregator.PublishOnUIThread(new AddKeyMessage { ParentDatabase = this });
            }
        }

        public void Handle(KeysDeletedMessage message)
        {
            if (dbNumber == message.DatabaseName)
            {
                ChangeKeyCount(-message.Keys.Count);

                var akey = message.Keys.FirstOrDefault();
                if (akey != null)
                {
                    var patharr = akey.Split(new[] { urnSeparator }, StringSplitOptions.RemoveEmptyEntries);
                    ExpandChildNode(Children, patharr.Take(patharr.Length - 1).ToList());

                    eventAggregator.PublishOnUIThread(new AddKeyMessage { ParentDatabase = this });
                }
            }
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

        private static void ExpandChildNode(IEnumerable<TreeViewItem> items, IList<string> path)
        {
            var item = items.FirstOrDefault(x => x.Display == path.FirstOrDefault());
            if (item == null)
            {
                return;
            }
            if (!item.HasChildren || path.Count == 1)
            {
                item.IsExpanded = true;
                item.IsSelected = true;
            }
            path.RemoveAt(0);

            ExpandChildNode(item.Children, path);
        }

        private void ChangeKeyCount(int keymodifier)
        {
            keyCount = keyCount + keymodifier;
            SetDisplay();
            ReloadDatabase();
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
    }
}
