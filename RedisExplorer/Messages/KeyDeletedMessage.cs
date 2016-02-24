using RedisExplorer.Models;

namespace RedisExplorer.Messages
{
    public class KeyDeletedMessage
    {
        public RedisKey Key { get; set; }
    }
}
