using System.Collections.ObjectModel;

using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisDatabase : TreeViewItem
    {
        private IDatabase database { get; set; }

        public RedisDatabase(TreeViewItem parent, IDatabase database) : base(parent, true)
        {
            this.database = database;
        }

        protected override void LoadChildren()
        {
            if (database != null)
            {
                var key = new RedisKey(this) { Display = "Blah1" };
                this.Children.Add(key);
            }
            
            //base.LoadChildren();
        }
    }
}
