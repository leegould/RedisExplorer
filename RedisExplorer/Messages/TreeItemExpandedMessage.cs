using RedisExplorer.Models;

namespace RedisExplorer.Messages
{
    public class TreeItemExpandedMessage
    {
        public TreeViewItem SelectedItem { get; set; }
    }
}
