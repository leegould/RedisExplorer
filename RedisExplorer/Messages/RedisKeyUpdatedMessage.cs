using RedisExplorer.Models;

namespace RedisExplorer.Messages
{
    public class RedisKeyUpdatedMessage
    {
        public RedisKey Item { get; set; }
    }
}
