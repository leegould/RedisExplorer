using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Caliburn.Micro;

using RedisExplorer.Messages;

using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisKeyHash : RedisKey
    {
        private Dictionary<string, string> keyValues;

        public RedisKeyHash(TreeViewItem parent, IEventAggregator eventAggregator) : base(parent, eventAggregator)
        {
        }

        public Dictionary<string, string> KeyValues
        {
            get
            {
                if (keyValues != null)
                {
                    return keyValues;
                }

                var key = KeyName;
                var db = Database;
                if (db.KeyExists(key) && db.KeyType(key) == RedisType.Hash)
                {
                    keyValues = db.HashGetAll(key).ToStringDictionary();
                }
                else
                {
                    keyValues = new Dictionary<string, string>();
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
            var keyexists = Database.KeyExists(KeyName);
            var saved = false;

            if (KeyType == RedisType.Hash)
            {
                //if (!keyexists)
                //{
                    foreach (var keyvalue in KeyValues)
                    {
                        Database.HashSet(KeyName, keyvalue.Key, keyvalue.Value);
                    }
                //}
                
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
            KeyValues = null;
            base.Reload();
        }
    }
}
