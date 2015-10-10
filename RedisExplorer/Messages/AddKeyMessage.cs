using StackExchange.Redis;

namespace RedisExplorer.Messages
{
    public class AddKeyMessage
    {
        public IDatabase ParentDatabase { get; set; }
    }
}
