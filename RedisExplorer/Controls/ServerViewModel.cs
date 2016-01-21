using System.ComponentModel.Composition;

using Caliburn.Micro;

using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    [Export(typeof(ServerViewModel))]
    public class ServerViewModel : Screen, IDisplayPanel, IHandle<TreeItemSelectedMessage>, IHandle<ServerReloadMessage>
    {
        private RedisServer redisServer;

        private string serverInfo;

        public string ServerInfo
        {
            get { return serverInfo; }
            set
            {
                serverInfo = value;
                NotifyOfPropertyChange(() => ServerInfo);
            }
        }

        public void ReloadServer()
        {
            if (redisServer != null)
            {
                redisServer.Reload();
            }
        }

        public ServerViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe(this);
        }

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message != null && message.SelectedItem is RedisServer)
            {
                redisServer = message.SelectedItem as RedisServer;

                DisplayItem();
            }
        }

        public void Handle(ServerReloadMessage message)
        {
            DisplayItem();
        }

        private void DisplayItem()
        {
            ServerInfo = redisServer.GetServerInfo();
        }
    }
}
