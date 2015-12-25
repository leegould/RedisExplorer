using System.Collections.Generic;
using System.Linq;

using Caliburn.Micro;
using RedisExplorer.Interface;
using RedisExplorer.Messages;

using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisKeySet : RedisKey, IKeyValue<List<string>>
    {
        private List<string> keyValue;

        public RedisKeySet(TreeViewItem parent, IEventAggregator eventAggregator)
            : base(parent, eventAggregator)
        {
        }

        public List<string> KeyValue
        {
            get
            {
                if (keyValue != null)
                {
                    return keyValue;
                }

                var key = KeyName;
                var db = Database;
                if (db.KeyExists(key))
                {
                    keyValue = db.SetMembers(key).Select(x => x.ToString()).ToList();
                }
                else
                {
                    keyValue = new List<string>();
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

            if (KeyType == RedisType.Set)
            {
                if (!keyexists)
                {
                    foreach (var keyvalue in KeyValue)
                    {
                        Database.SetAdd(KeyName, keyvalue);
                    }
                }
                else if (Database.KeyType(KeyName) != RedisType.Set)
                {
                    Database.KeyDelete(KeyName);
                    foreach (var keyvalue in KeyValue)
                    {
                        Database.SetAdd(KeyName, keyvalue);
                    }
                }
                else 
                {
                    var oldvalues = Database.SetMembers(KeyName).Select(x => x.ToString()).ToList();
                    var newvalues = KeyValue.Except(oldvalues).ToList();
                    var removedvalues = KeyValue.Except(newvalues).Except(oldvalues);

                    var count = Database.SetAdd(KeyName, newvalues.Select(x => (RedisValue)x).ToArray());
                    var removed = Database.SetRemove(KeyName, removedvalues.Select(x => (RedisValue) x).ToArray());
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
