using RedisExplorer.Models;

namespace RedisExplorer.Messages
{
    public class RedisKeyReloadMessage
    {
        public RedisKey Item { get; set; }
    }
}
