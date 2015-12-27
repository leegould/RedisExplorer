using Caliburn.Micro;
using RedisExplorer.Interface;
using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisKeyString : RedisKey, IKeyValue<string>
    {
        private string keyValue;


        public RedisKeyString(TreeViewItem parent, IEventAggregator eventAggregator)
            : base(parent, eventAggregator)
        {
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
                    return db.StringGet(key);
                }

                return string.Empty;
            }
            set
            {
                keyValue = value;
            }
        }

        public override bool Save()
        {
            if (KeyType != RedisType.String && KeyType != RedisType.None) return false;

            var keyvalue = KeyValue;
            Database.StringSet(KeyName, keyvalue, TTL);

            return true;
        }

        public override void Reload()
        {
            KeyValue = string.Empty;
            base.Reload();
        }
    }
}
