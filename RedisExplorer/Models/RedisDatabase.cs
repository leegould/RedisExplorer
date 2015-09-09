using Caliburn.Micro;

using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisDatabase : TreeViewItem
    {
        public RedisDatabase(TreeViewItem parent, IDatabase database) : base(parent, false)
        {

        }
    }
}
