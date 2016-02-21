using StackExchange.Redis;

namespace RedisExplorer.Messages
{
    public class RedisKeyUpdatedMessage
    {
        public Models.RedisKey Key { get; set; }
        //public string Urn { get; set; }
        //public RedisType Type { get; set; }
    }
}
