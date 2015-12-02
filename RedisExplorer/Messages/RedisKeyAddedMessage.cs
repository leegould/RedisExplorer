using StackExchange.Redis;

namespace RedisExplorer.Messages
{
    public class RedisKeyAddedMessage
    {
        public string Urn { get; set; }
        public RedisType Type { get; set; }
    }
}
