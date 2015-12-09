using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Caliburn.Micro;

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
                if (db.KeyExists(key))
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
            var existingkey = Database.KeyExists(KeyName);
            var saved = false;

            if (KeyType == RedisType.Hash)
            {
                // TODO
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
