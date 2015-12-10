using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using RedisExplorer.Messages;
using RedisExplorer.Properties;
using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisDatabase : TreeViewItem
    {
        private IEventAggregator eventAggregator { get; set; }

        private RedisServer parent { get; set; }

        private int dbNumber { get; set; }

        private int maxKeys { get; set; }

        private string urnSeparator { get; set; }

        public RedisDatabase(RedisServer parent, int dbnumber, IEventAggregator eventAggregator) : base(parent, Settings.Default.LazyLoadDatabase, eventAggregator)
        {
            this.parent = parent;
            this.dbNumber = dbnumber;
            this.eventAggregator = eventAggregator;
            maxKeys = string.IsNullOrEmpty(Settings.Default.MaxKeys) ? 1000 : int.Parse(Settings.Default.MaxKeys);
            urnSeparator = string.IsNullOrEmpty(Settings.Default.UrnSeparator) ? ":" : Settings.Default.UrnSeparator;
        }

        public IDatabase GetDatabase()
        {
            return parent.GetDatabase(dbNumber);
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
                if (ktype == RedisType.String)
                {
                    key = new RedisKeyString(item, eventAggregator) { Display = keystr };
                }
                else if (ktype == RedisType.Set)
                {
                    key = new RedisKeySet(item, eventAggregator) { Display = keystr };
                }
                else if (ktype == RedisType.List)
                {
                    key = new RedisKeyList(item, eventAggregator) { Display = keystr };
                }
                else if (ktype == RedisType.Hash)
                {
                    key = new RedisKeyHash(item, eventAggregator) { Display = keystr };
                }
                else
                {
                    key = new RedisKeyString(item, eventAggregator) { Display = keystr };
                }
                item.Children.Add(key);
            }

            if (urn.Count > 0)
            {
                AddChildren(key, urn, ktype, eventAggregator);
            }
        }

        public void Reload(RedisServer server)
        {
            var s = parent.GetServer();

            eventAggregator.PublishOnUIThread(new DatabaseReloadMessage { DbNumber = dbNumber });

            Children.Clear();

            LoadChildren();
        }

        public void Flush(RedisDatabase database)
        {
            var s = parent.GetServer();
            s.FlushDatabase(dbNumber);

            eventAggregator.PublishOnUIThread(new FlushDbMessage { dbNumber = dbNumber });

            Children.Clear();
            LoadChildren();
        }

        public void Add()
        {
            eventAggregator.PublishOnUIThread(new AddKeyMessage { ParentDatabase = this });
        }
    }
}
