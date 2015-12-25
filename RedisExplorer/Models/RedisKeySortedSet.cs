using System.Collections.Generic;
using System.Linq;

using Caliburn.Micro;
using RedisExplorer.Interface;
using RedisExplorer.Messages;

using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisKeySortedSet : RedisKey, IKeyValue<List<SortedSetEntry>>
    {
        private List<SortedSetEntry> keyValue;

        public RedisKeySortedSet(TreeViewItem parent, IEventAggregator eventAggregator) : base(parent, eventAggregator)
        {
        }

        public List<SortedSetEntry> KeyValue
        {
            get
            {
                if (keyValue != null)
                {
                    return keyValue;
                }

                keyValue = Database.KeyExists(KeyName) ? Database.SortedSetRangeByScoreWithScores(KeyName).Select(x => x).ToList() : new List<SortedSetEntry>();

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

            if (KeyType == RedisType.SortedSet)
            {
                if (keyexists && Database.KeyType(KeyName) != RedisType.SortedSet)
                {
                    Database.KeyDelete(KeyName);
                }

                Database.SortedSetAdd(KeyName, KeyValue.ToArray()); 
                
                if (!keyexists)
                {
                    eventAggregator.PublishOnUIThread(new RedisKeyAddedMessage { Urn = KeyName });
                }
                else
                {
                    eventAggregator.PublishOnUIThread(new RedisKeyUpdatedMessage { Urn = KeyName });
                }

                var itemintree = (RedisKeySortedSet)Parent.Children.FirstOrDefault(x => x.IsSelected);
                if (itemintree != null)
                {
                    itemintree.KeyValue = KeyValue;
                }

                return true;
            }

            return false;
        }

        public override void Reload()
        {
            KeyValue = null;
            base.Reload();
        }
    }
}
