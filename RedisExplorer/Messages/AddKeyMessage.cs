using RedisExplorer.Models;

namespace RedisExplorer.Messages
{
    public class AddKeyMessage
    {
        public string KeyBase { get; set; }

        public RedisDatabase ParentDatabase { get; set; }
    }
}
