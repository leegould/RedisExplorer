using System.ComponentModel.Composition;
using System.Dynamic;
using System.Windows;

using Caliburn.Micro;

using RedisExplorer.Controls;
using RedisExplorer.Interface;
using RedisExplorer.Models;

using StackExchange.Redis;

namespace RedisExplorer
{
    [Export(typeof(AppViewModel))]
    public sealed class AppViewModel : Conductor<ITabItem>.Collection.OneActive, IApp
    {
        #region Private members

        private readonly IEventAggregator eventAggregator;

        private readonly IWindowManager windowManager;

        private string statusBarTextBlock;

        private BindableCollection<RedisServer> servers; 

        #endregion

        #region Properties

        public BindableCollection<RedisServer> Servers
        {
            get { return servers; }
            set
            {
                servers = value;
                NotifyOfPropertyChange(() => Servers);
            }
        }

        #endregion

        public AppViewModel(IEventAggregator eventAggregator, IWindowManager windowManager)
        {
            this.DisplayName = "Redis Explorer";

            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            this.windowManager = windowManager;
            Servers = new BindableCollection<RedisServer>();

            LoadServers();
        }

        private void LoadServers()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("192.168.1.161,keepAlive = 180,allowAdmin=true");

            foreach(var endpoint in redis.GetEndPoints())
            {
                Servers.Add(new RedisServer(redis, redis.GetServer(endpoint)) { Display = "Redis Server" });
            }
        }

        #region Properties

        public string StatusBarTextBlock
        {
            get
            {
                return statusBarTextBlock;
            }
            set
            {
                statusBarTextBlock = value;
                NotifyOfPropertyChange(() => StatusBarTextBlock);
            }
        }

        #endregion

        #region Menu

        public void Exit()
        {
            Application.Current.Shutdown();
        }


        public void AddServer()
        {
            dynamic settings = new ExpandoObject();
            settings.Width = 300;
            settings.Height = 250;
            settings.WindowStartupLocation = WindowStartupLocation.Manual;
            settings.Title = "Add Server";

            windowManager.ShowWindow(new AddConnectionViewModel(eventAggregator), null, settings);    
        }

        #endregion
    }
}
