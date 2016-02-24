using System.Collections.Generic;

namespace RedisExplorer.Messages
{
    public class KeysDeletedMessage
    {
        public int DatabaseName { get; set; }
        public List<string> Keys { get; set; }
    }
}
