using Caliburn.Micro;

namespace RedisExplorer.Models
{
    public class RedisKey : TreeViewItem
    {
        public RedisKey(TreeViewItem parent, IEventAggregator eventAggregator) : base(parent, false, eventAggregator)
        {
        }
    }
}
