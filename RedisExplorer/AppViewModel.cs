using System.ComponentModel.Composition;
using System.Windows;

using Caliburn.Micro;

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
            this.DisplayName = "RedisExplorer";

            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            this.windowManager = windowManager;
            Servers = new BindableCollection<RedisServer>();

            LoadServers();
        }

        private void LoadServers()
        {
            //ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("192.168.1.1,keepAlive = 180,allowAdmin=true");

            //var svm = new RedisServer(redis.GetServer("192.168.1.1", 6379)) { Display = "Redis Server" };
            //var db1 = new RedisDatabase(svm, redis.GetDatabase(6)) { Display = "6" };
            //var db2 = new RedisDatabase(svm, redis.GetDatabase(7)) { Display = "7" };

            var svm = new RedisServer(null) { Display = "Server" };
            svm.Children.Add(new RedisDatabase(svm, null) { Display = "1" });
            svm.Children.Add(new RedisDatabase(svm, null) { Display = "2" });

            Servers.Add(svm);
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

        #endregion
    }
}
