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

        protected RedisDatabase GetParentDatabase
        {
            get
            {
                var parent = Parent;
                var parentType = parent.GetType();

                while (parentType != typeof(RedisDatabase))
                {
                    parent = parent.Parent;
                    parentType = parent.GetType();
                }

                return ((RedisDatabase)parent);
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
        
        public bool Save()
        {
            bool newkey = Database.KeyExists(KeyName);
            bool saved = false;

            if (KeyType == RedisType.String || KeyType == RedisType.None)
            {
                saved = Database.StringSet(KeyName, KeyValue, TTL);
            }

            if (!newkey)
            {
                eventAggregator.PublishOnUIThread(new RedisKeyAddedMessage { Urn = KeyName });
            }
            else 
            {
                eventAggregator.PublishOnUIThread(new RedisKeyUpdatedMessage { Urn = KeyName });
            }

            return saved;
        }

        public bool Delete()
        {
            var key = KeyName;
            var db = Database;

            return db.KeyExists(key) && db.KeyDelete(key);
        }

        public void Reload()
        {
            var db = Database;

            KeyValue = string.Empty;
            KeyType = RedisType.None;
            TTL = null;

            eventAggregator.PublishOnUIThread(new ReloadKeyMessage { Urn = KeyName });
        }

        public void Add()
        {
            eventAggregator.PublishOnUIThread(new AddKeyMessage { ParentDatabase = GetParentDatabase, KeyBase = KeyName });
        }
    }
}
