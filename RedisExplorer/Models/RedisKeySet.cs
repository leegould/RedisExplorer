using System.Collections.Generic;
using System.Linq;

using Caliburn.Micro;
using RedisExplorer.Interface;

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
                keyValue = db.KeyExists(key) ? db.SetMembers(key).Select(x => x.ToString()).ToList() : new List<string>();

                return keyValue;
            }
            set
            {
                keyValue = value;
            }
        }

        public override bool Save()
        {
            var saved = false;

            if (KeyType == RedisType.Set)
            {
                if (!Database.KeyExists(KeyName))
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
                    var valuestoadd = newvalues.Select(x => (RedisValue)x).ToArray();
                    var removedvalues = KeyValue.Except(newvalues).Except(oldvalues).Select(x => (RedisValue)x).ToArray();

                    if (valuestoadd.Any())
                    {
                        Database.SetAdd(KeyName, valuestoadd);
                    }
                    if (removedvalues.Any())
                    {
                        Database.SetRemove(KeyName, removedvalues);
                    }
                }

                saved = true;

                //var itemintree = (RedisKeySet)Parent.Children.FirstOrDefault(x => x.IsSelected);
                //if (itemintree != null)
                //{
                //    itemintree.KeyValue = KeyValue;
                //}
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
