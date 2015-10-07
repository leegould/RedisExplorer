using System;

using Caliburn.Micro;
using RedisExplorer.Messages;
using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisKey : TreeViewItem
    {
        private IEventAggregator eventAggregator { get; set; }

        private IDatabase Database { get; set; }

        private string KeyName { get; set; }

        public RedisKey(TreeViewItem parent, IEventAggregator eventAggregator) : base(parent, false, eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public string GetKeyName()
        {
            if (string.IsNullOrEmpty(KeyName))
            {
                KeyName = Display;

                if (Parent != null)
                {
                    var parent = Parent;
                    var parentType = parent.GetType();

                    while (parentType == typeof(RedisKey))
                    {
                        KeyName = parent.Display + ":" + KeyName;
                        parent = parent.Parent;
                        parentType = parent.GetType();
                    }
                }
            }

            return KeyName;
        }

        public IDatabase GetDatabase()
        {
            if (Database == null)
            {
                if (Parent != null)
                {
                    var parent = Parent;
                    var parentType = parent.GetType();

                    while (parentType != typeof(RedisDatabase))
                    {
                        parent = parent.Parent;
                        parentType = parent.GetType();
                    }

                    Database = ((RedisDatabase)parent).GetDatabase();
                }
            }
            return Database;
        }

        public string GetValue()
        {
            var key = GetKeyName();
            var db = GetDatabase();
            if (db.KeyExists(key))
            {
                var ktype = db.KeyType(key);

                if (ktype == RedisType.String)
                {
                    return db.StringGet(key);
                }
            }

            return string.Empty;
        }

        public bool SaveValue(string value, TimeSpan? expiry = null)
        {
            var key = GetKeyName();
            if (Database.KeyExists(key))
            {
                var ktype = Database.KeyType(key);
                if (ktype == RedisType.String)
                {
                    eventAggregator.PublishOnUIThread(new RedisKeyUpdatedMessage { Item = this });
                    return Database.StringSet(key, value, expiry);
                }
            }

            return false;
        }

        public bool Delete()
        {
            var key = GetKeyName();
            var db = GetDatabase();

            if (db.KeyExists(key))
            {
                return db.KeyDelete(key);
            }
            return false;
        }

        public TimeSpan? GetTTL()
        {
            var key = GetKeyName();
            var db = GetDatabase();

            if (db.KeyExists(key))
            {
                return db.KeyTimeToLive(key);
            }
            return null;
        }

        public RedisType GetKeyType()
        {
            var key = GetKeyName();
            var db = GetDatabase();

            if (db != null && db.KeyExists(key))
            {
                return db.KeyType(key);
            }

            return RedisType.None;
        }
    }
}
