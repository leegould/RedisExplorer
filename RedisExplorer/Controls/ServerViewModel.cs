using System.ComponentModel.Composition;

using Caliburn.Micro;

using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    [Export(typeof(ServerViewModel))]
    public class ServerViewModel : Screen, IDisplayPanel, IHandle<TreeItemSelectedMessage>
    {
        private readonly IEventAggregator eventAggregator;

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

        public ServerViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
        }

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message != null && message.SelectedItem is RedisServer)
            {
                var server = message.SelectedItem as RedisServer;

                ServerInfo = server.GetServerInfo();
            }
        }
    }
}
