using RedisExplorer.Models;

namespace RedisExplorer.Messages
{
    public class AddConnectionMessage
    {
        public RedisConnection Connection { get; set; }
    }
}
