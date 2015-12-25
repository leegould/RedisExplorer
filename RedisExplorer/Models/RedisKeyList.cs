using System.Collections.Generic;
using System.Linq;

using Caliburn.Micro;
using RedisExplorer.Interface;
using RedisExplorer.Messages;

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
                if (db.KeyExists(key))
                {
                    keyValue = db.ListRange(key).Select(x => x.ToString()).ToList();
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
            var existingkey = Database.KeyExists(KeyName);
            var saved = false;

            if (KeyType == RedisType.List)
            {
                Database.KeyDelete(KeyName);
                foreach (var keyvalue in KeyValue.Where(keyvalue => !string.IsNullOrEmpty(keyvalue)))
                {
                    Database.ListRightPush(KeyName, keyvalue);
                }

                saved = true;


                if (!existingkey)
                {
                    eventAggregator.PublishOnUIThread(new RedisKeyAddedMessage { Urn = KeyName, Type = RedisType.List });
                }
                else
                {
                    eventAggregator.PublishOnUIThread(new RedisKeyUpdatedMessage { Urn = KeyName, Type = RedisType.List });
                }

                var itemintree = (RedisKeyList)Parent.Children.FirstOrDefault(x => x.IsSelected);
                if (itemintree != null)
                {
                    itemintree.KeyValue = KeyValue;
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
