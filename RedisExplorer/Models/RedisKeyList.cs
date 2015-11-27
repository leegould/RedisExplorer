using System.Collections.Generic;
using System.Linq;

using Caliburn.Micro;

using RedisExplorer.Messages;

using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisKeyList: RedisKey
    {
        private List<string> keyValues;

        public RedisKeyList(TreeViewItem parent, IEventAggregator eventAggregator)
            : base(parent, eventAggregator)
        {
        }

        public List<string> KeyValues
        {
            get
            {
                if (keyValues != null)
                {
                    return keyValues;
                }

                var key = KeyName;
                var db = Database;
                if (db.KeyExists(key))
                {
                    keyValues = db.ListRange(key).Select(x => x.ToString()).ToList();
                }
                else
                {
                    keyValues = new List<string>();
                }

                return keyValues;
            }
            set
            {
                keyValues = value;
            }
        }

        public override bool Save()
        {
            var existingkey = Database.KeyExists(KeyName);
            var saved = false;

            if (KeyType == RedisType.List)
            {
                Database.KeyDelete(KeyName);
                for (int i = 0; i <= KeyValues.Count; i++)
                {
                    if (!string.IsNullOrEmpty(KeyValues[i]))
                    {
                        Database.ListSetByIndex(KeyName, i, KeyValues[i]);
                    }
                }

                saved = true;

                if (!existingkey)
                {
                    eventAggregator.PublishOnUIThread(new RedisKeyAddedMessage { Urn = KeyName });
                }
                else
                {
                    eventAggregator.PublishOnUIThread(new RedisKeyUpdatedMessage { Urn = KeyName });
                }
            }

            return saved;
        }

        public override void Reload()
        {
            KeyValues = null;
            base.Reload();
        }
    }
}
