using RedisExplorer.Models;

namespace RedisExplorer.Messages
{
    public class AddKeyMessage
    {
        public RedisDatabase ParentDatabase { get; set; }
    }
}
