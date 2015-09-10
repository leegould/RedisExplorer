using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
