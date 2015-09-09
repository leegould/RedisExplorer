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

        private BindableCollection<ServerViewModel> servers; 

        #endregion

        #region Properties

        public BindableCollection<ServerViewModel> Servers
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
            Servers = new BindableCollection<ServerViewModel>();

            LoadServers();
        }

        private void LoadServers()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(",keepAlive = 180,allowAdmin=true");

            var db1 = new RedisDatabase { Database = redis.GetDatabase(6), Name = "6" };
            var db2 = new RedisDatabase { Database = redis.GetDatabase(7), Name = "7" };

            var dbcoll = new RedisDatabase[] { db1, db2 };
            var svm = new ServerViewModel(dbcoll);
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
