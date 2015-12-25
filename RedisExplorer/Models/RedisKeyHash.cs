using System.Collections.Generic;
using System.Linq;

using Caliburn.Micro;
using RedisExplorer.Interface;
using RedisExplorer.Messages;

using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisKeyHash : RedisKey, IKeyValue<Dictionary<string, string>>
    {
        private Dictionary<string, string> keyValue;

        public RedisKeyHash(TreeViewItem parent, IEventAggregator eventAggregator) : base(parent, eventAggregator)
        {
        }

        public Dictionary<string, string> KeyValue
        {
            get
            {
                if (keyValue != null)
                {
                    return keyValue;
                }

                var key = KeyName;
                var db = Database;
                if (db.KeyExists(key) && db.KeyType(key) == RedisType.Hash)
                {
                    keyValue = db.HashGetAll(key).ToStringDictionary();
                }
                else
                {
                    keyValue = new Dictionary<string, string>();
                }

                return keyValue;
            }
            set
            {
                keyValue = value;
            }
        }

        public override bool Save()
        {
            var keyexists = Database.KeyExists(KeyName);
            var saved = false;

            if (KeyType == RedisType.Hash)
            {
                if (Database.KeyExists(KeyName) && Database.KeyType(KeyName) != KeyType)
                {
                    Database.KeyDelete(KeyName);
                }

                foreach (var keyvalue in KeyValue)
                {
                    Database.HashSet(KeyName, keyvalue.Key, keyvalue.Value);
                }
                
                saved = true;

                if (!keyexists)
                {
                    eventAggregator.PublishOnUIThread(new RedisKeyAddedMessage { Urn = KeyName });
                }
                else
                {
                    eventAggregator.PublishOnUIThread(new RedisKeyUpdatedMessage { Urn = KeyName });
                }

                var itemintree = (RedisKeyHash)Parent.Children.FirstOrDefault(x => x.IsSelected);
                if (itemintree != null)
                {
                    itemintree.KeyValue = KeyValue;
                }
            }

            return saved;
        }

        public override void Reload()
        {
            KeyValue = null;
            base.Reload();
        }
    }
}
