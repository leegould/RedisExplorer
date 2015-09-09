using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisDatabase
    {
        public IDatabase Database { get; set; }

        public string Name { get; set; }
    }
}
