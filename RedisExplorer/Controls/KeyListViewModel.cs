using System;

using Caliburn.Micro;

using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    public class KeyListViewModel : Screen, IHandle<TreeItemSelectedMessage>, IValueItem
    {
        public void Handle(TreeItemSelectedMessage message)
        {
            if (message != null && message.SelectedItem is RedisKeyList && !message.SelectedItem.HasChildren)
            {
                DisplayValue(message.SelectedItem as RedisKeyList);
            }
        }

        private void DisplayValue(RedisKeyList redisKeyList)
        {
            throw new NotImplementedException();
        }
    }
}
