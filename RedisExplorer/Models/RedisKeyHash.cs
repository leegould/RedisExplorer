using System.Collections.Generic;
using System.Linq;

using Caliburn.Micro;
using RedisExplorer.Interface;

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
            if (KeyType != RedisType.Hash) return false;

            if (Database.KeyExists(KeyName) && Database.KeyType(KeyName) != KeyType)
            {
                Database.KeyDelete(KeyName);
            }

            foreach (var keyvalue in KeyValue)
            {
                Database.HashSet(KeyName, keyvalue.Key, keyvalue.Value);
            }
                
            var itemintree = (RedisKeyHash)Parent.Children.FirstOrDefault(x => x.IsSelected);
            if (itemintree != null)
            {
                itemintree.KeyValue = KeyValue;
            }

            return true;
        }

        public override void Reload()
        {
            KeyValue = null;
            base.Reload();
        }
    }
}
