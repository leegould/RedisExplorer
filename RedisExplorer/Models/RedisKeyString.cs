using Caliburn.Micro;

using RedisExplorer.Messages;

using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisKeyString : RedisKey
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
            var newkey = Database.KeyExists(KeyName);
            var saved = false;

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

        public override void Reload()
        {
            KeyValue = string.Empty;
            base.Reload();
        }
    }
}
