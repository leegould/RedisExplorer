using System;

using Caliburn.Micro;
using RedisExplorer.Messages;
using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisKey : TreeViewItem
    {
        public IDatabase Database { get; set; }

        private IEventAggregator eventAggregator { get; set; }

        private string KeyName { get; set; }

        public RedisKey(TreeViewItem parent, IDatabase database, IEventAggregator eventAggregator) : base(parent, false, eventAggregator)
        {
            this.Database = database;
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

        public string GetValue()
        {
            var key = GetKeyName();
            if (Database.KeyExists(key))
            {
                var ktype = Database.KeyType(key);

                if (ktype == RedisType.String)
                {
                    return Database.StringGet(key);
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

        public TimeSpan? GetTTL()
        {
            var key = GetKeyName();
            if (Database.KeyExists(key))
            {
                return Database.KeyTimeToLive(key);
            }
            return null;
        }

        public RedisType GetType()
        {
            var key = GetKeyName();
            if (Database.KeyExists(key))
            {
                return Database.KeyType(key);
            }

            return RedisType.None;
        }
    }
}
