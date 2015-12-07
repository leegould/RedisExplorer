using System.Collections.Generic;
using System.Linq;

using Caliburn.Micro;

using RedisExplorer.Messages;

using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisKeySet : RedisKey
    {
        private List<string> keyValues;

        public RedisKeySet(TreeViewItem parent, IEventAggregator eventAggregator)
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
                    keyValues = db.SetMembers(key).Select(x => x.ToString()).ToList();
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
            var newkey = Database.KeyExists(KeyName);
            var saved = false;

            if (KeyType == RedisType.Set)
            {
                if (newkey)
                {
                    foreach (var keyvalue in KeyValues)
                    {
                        Database.SetAdd(KeyName, keyvalue);
                    }
                }
                else if (Database.KeyType(KeyName) != RedisType.Set)
                {
                    Database.KeyDelete(KeyName);
                    foreach (var keyvalue in KeyValues)
                    {
                        Database.SetAdd(KeyName, keyvalue);
                    }
                }
                else 
                {
                    var oldvalues = Database.SetMembers(KeyName).Select(x => x.ToString()).ToList();
                    var newvalues = KeyValues.Except(oldvalues).ToList();
                    var removedvalues = KeyValues.Except(newvalues).Except(oldvalues);

                    var count = Database.SetAdd(KeyName, newvalues.Select(x => (RedisValue)x).ToArray());
                    var removed = Database.SetRemove(KeyName, removedvalues.Select(x => (RedisValue) x).ToArray());
                }

                saved = true;

                if (!newkey)
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
