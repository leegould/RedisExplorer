using System;

using Caliburn.Micro;
using RedisExplorer.Messages;
using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisKey : TreeViewItem
    {
        private IEventAggregator eventAggregator { get; set; }

        private IDatabase database { get; set; }

        private string keyName;

        private string keyValue;

        private RedisType keyType;

        private TimeSpan? ttl;

        public RedisKey(TreeViewItem parent, IEventAggregator eventAggregator) : base(parent, false, eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        protected IDatabase Database
        {
            get
            {
                if (database != null || Parent == null)
                {
                    return database;
                }

                var parent = Parent;
                var parentType = parent.GetType();

                while (parentType != typeof(RedisDatabase))
                {
                    parent = parent.Parent;
                    parentType = parent.GetType();
                }

                database = ((RedisDatabase)parent).GetDatabase();
                return database;
            }
            set
            {
                database = value;
            }
        }

        public string KeyName
        {
            get
            {
                if (!string.IsNullOrEmpty(keyName))
                {
                    return keyName;
                }

                keyName = Display;

                if (Parent == null)
                {
                    return KeyName;
                }

                var parent = Parent;
                var parentType = parent.GetType();

                while (parentType == typeof(RedisKey))
                {
                    KeyName = parent.Display + ":" + KeyName;
                    parent = parent.Parent;
                    parentType = parent.GetType();
                }

                return keyName;
            }
            set
            {
                keyName = value;
            }
        }

        public string KeyValue
        {
            get
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    return keyValue;
                }

                var key = KeyName;
                var db = Database;
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
            set
            {
                keyValue = value;
            }
        }

        public RedisType KeyType
        {
            get
            {
                if (keyType != RedisType.None)
                {
                    return keyType;
                }

                var key = KeyName;
                var db = Database;

                if (db != null && db.KeyExists(key))
                {
                    return db.KeyType(key);
                }

                return RedisType.None;
            }
            set
            {
                keyType = value;
            }
        }

        public TimeSpan? TTL
        {
            get
            {
                if (ttl != null)
                {
                    return ttl;
                }

                var key = KeyName;
                var db = Database;

                return db.KeyExists(key) ? db.KeyTimeToLive(key) : null;
            }
            set
            {
                ttl = value;
            }
        }

        public bool CanDelete
        {
            get { return !HasChildren; }
        }
        
        public bool SaveValue()
        {
            if (Database.KeyExists(KeyName))
            {
                if (KeyType == RedisType.String)
                {
                    eventAggregator.PublishOnUIThread(new RedisKeyUpdatedMessage { Item = this });
                    return Database.StringSet(KeyName, KeyValue, TTL);
                }
            }

            return false;
        }

        public bool Delete()
        {
            var key = KeyName;
            var db = Database;

            return db.KeyExists(key) && db.KeyDelete(key);
        }
    }
}
