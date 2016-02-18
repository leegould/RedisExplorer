using System.Collections.Generic;

namespace RedisExplorer.Messages
{
    public class KeysDeletedMessage
    {
        public List<string> Keys { get; set; }
    }
}
