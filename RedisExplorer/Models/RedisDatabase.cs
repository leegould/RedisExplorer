using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisDatabase : TreeViewItem
    {
        private IEventAggregator eventAggregator { get; set; }

        private RedisServer parent { get; set; }

        private IDatabase Database { get; set; }

        public RedisDatabase(RedisServer parent, IDatabase database, IEventAggregator eventAggregator) : base(parent, true, eventAggregator)
        {
            this.parent = parent;
            this.Database = database;
            this.eventAggregator = eventAggregator;
        }

        protected override void LoadChildren()
        {
            if (parent != null && Database != null)
            {
                var keys = parent.server.Keys(Database.Database, "*", 1000);

                foreach (var key in keys)
                {
                    var parts = new Queue<string>(key.ToString().Split(':'));
                    if (parts.Count > 0)
                    {
                        AddChildren(this, parts, eventAggregator);
                    }
                }
            }
        }

        private static void AddChildren(TreeViewItem item, Queue<string> urn, IEventAggregator eventAggregator)
        {
            var keystr = urn.Dequeue();
            var key = item.Children.FirstOrDefault(x => x.Display == keystr);
            if (key == null)
            {
                key = new RedisKey(item, eventAggregator) { Display = keystr };
                item.Children.Add(key);
            }

            if (urn.Count > 0)
            {
                AddChildren(key, urn, eventAggregator);
            }
        }
    }
}
