using Caliburn.Micro;

using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisKey : TreeViewItem
    {
        public IDatabase Database { get; set; }

        public RedisKey(TreeViewItem parent, IDatabase database, IEventAggregator eventAggregator) : base(parent, false, eventAggregator)
        {
            this.Database = database;
        }

        public string GetKeyName()
        {
            var keyname = Display;

            if (this.Parent != null)
            {
                var parent = Parent;
                var parentType = parent.GetType();

                while (parentType == typeof(RedisKey))
                {
                    keyname = parent.Display + ":" + keyname;
                    parent = parent.Parent;
                    parentType = parent.GetType();
                }
            }
            return keyname;
        }

        public string GetValue()
        {
            var key = GetKeyName();
            if (Database.KeyExists(key))
            {
                var ktype = Database.KeyType(key);

                if (ktype == RedisType.String)
                {
                    return Database.StringGet(key);
                }
            }

            return string.Empty;
        }
    }
}
