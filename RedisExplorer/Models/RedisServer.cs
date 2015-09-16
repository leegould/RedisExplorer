using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisServer : TreeViewItem
    {
        public RedisServer(IServer server) : base(null, false)
        {

        }
    }
}
