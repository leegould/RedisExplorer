using System;
using System.Linq;
using System.Threading.Tasks;

using Caliburn.Micro;
using RedisExplorer.Messages;
using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public abstract class RedisKey : TreeViewItem
    {
        protected IEventAggregator eventAggregator { get; set; }

        private IDatabase database { get; set; }

        private string keyName;

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

        public int DatabaseName
        {
            get { return GetParentDatabase.GetDatabaseNumber; }            
        }

        public void ChangeDatabase(int dbNumber)
        {
            Database = GetParentDatabase.GetDatabase(dbNumber);
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

                while (parentType.IsSubclassOf(typeof(RedisKey)))
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

                if (key != null && db != null && db.KeyExists(key))
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

                return key != null && db != null && db.KeyExists(key) ? db.KeyTimeToLive(key) : null;
            }
            set
            {
                ttl = value;
            }
        }

        public bool CanDelete
        {
            get
            {
                return true; 
            }
        }

        public bool ItemExists()
        {
            return Database.KeyExists(KeyName);
        }

        public virtual bool Save()
        {
            return false;
        }

        public void NotifyOfSave(bool existingKey)
        {
            if (existingKey)
            {
                eventAggregator.PublishOnUIThread(new RedisKeyUpdatedMessage { Key = this });
            }
            else
            {
                eventAggregator.PublishOnUIThread(new RedisKeyAddedMessage { Key = this });
            }
        }

        public bool Delete()
        {
            if (!HasChildren)
            {
                if (Database.KeyExists(KeyName) && Database.KeyDelete(KeyName))
                {
                    eventAggregator.PublishOnUIThread(new KeyDeletedMessage { Key = this });
                    return true;
                }
            }

            var parentdb = GetParentDatabase;
            var server = parentdb.Parent as RedisServer;
            if (server != null)
            {
                var keys = server.GetServer().Keys(parentdb.GetDatabaseNumber, Display + "*").ToList();
                var result = Database.KeyDelete(keys.ToArray());

                if (result > 0)
                {
                    eventAggregator.PublishOnUIThread(new KeysDeletedMessage { DatabaseName = parentdb.GetDatabaseNumber, Keys = keys.Select(x => x.ToString()).ToList() });
                    return true;
                }
            }
            return false;
        }

        public virtual void Reload()
        {
            var db = Database;

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
