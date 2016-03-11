using System.Collections.Generic;
using System.Linq;

using Caliburn.Micro;
using RedisExplorer.Interface;

using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisKeyList : RedisKey, IKeyValue<List<string>>
    {
        private List<string> keyValue;

        public RedisKeyList(TreeViewItem parent, IEventAggregator eventAggregator) : base(parent, eventAggregator)
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
                keyValue = db.KeyExists(key) ? db.ListRange(key).Select(x => x.ToString()).ToList() : new List<string>();

                return keyValue;
            }
            set
            {
                keyValue = value;
            }
        }

        public override bool Save()
        {
            if (KeyType != RedisType.List) return false;

            Database.KeyDelete(KeyName);
            foreach (var keyvalue in KeyValue.Where(keyvalue => !string.IsNullOrEmpty(keyvalue)))
            {
                Database.ListRightPush(KeyName, keyvalue);
            }

            //var itemintree = (RedisKeyList)Parent.Children.FirstOrDefault(x => x.IsSelected);
            //if (itemintree != null)
            //{
            //    itemintree.KeyValue = KeyValue;
            //}

            return true;
        }

        public override void Reload()
        {
            KeyValue = null;
            base.Reload();
        }
    }
}
