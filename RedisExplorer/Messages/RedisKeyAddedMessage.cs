using RedisExplorer.Models;

namespace RedisExplorer.Messages
{
    public class RedisKeyAddedMessage
    {
        public RedisKey Item { get; set; }
    }
}
