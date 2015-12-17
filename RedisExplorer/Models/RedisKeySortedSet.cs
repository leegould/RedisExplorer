using System.Collections.Generic;
using System.Linq;

using Caliburn.Micro;

using RedisExplorer.Messages;

using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisKeySortedSet : RedisKey
    {
        private List<SortedSetEntry> keyValues;

        public RedisKeySortedSet(TreeViewItem parent, IEventAggregator eventAggregator) : base(parent, eventAggregator)
        {
        }

        public List<SortedSetEntry> KeyValues
        {
            get
            {
                if (keyValues != null)
                {
                    return keyValues;
                }

                keyValues = Database.KeyExists(KeyName) ? Database.SortedSetRangeByScoreWithScores(KeyName).Select(x => x).ToList() : new List<SortedSetEntry>();

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

            if (KeyType == RedisType.SortedSet)
            {
                if (keyexists && Database.KeyType(KeyName) != RedisType.SortedSet)
                {
                    Database.KeyDelete(KeyName);
                }

                Database.SortedSetAdd(KeyName, KeyValues.ToArray()); 
                
                if (!keyexists)
                {
                    eventAggregator.PublishOnUIThread(new RedisKeyAddedMessage { Urn = KeyName });
                }
                else
                {
                    eventAggregator.PublishOnUIThread(new RedisKeyUpdatedMessage { Urn = KeyName });
                }

                return true;
            }

            return false;
        }

        public override void Reload()
        {
            KeyValues = null;
            base.Reload();
        }
    }
}
