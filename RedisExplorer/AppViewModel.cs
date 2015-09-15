using System;
using System.ComponentModel.Composition;
using System.Linq;
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
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("192.168.1.161,keepAlive = 180,allowAdmin=true");

            foreach(var endpoint in redis.GetEndPoints())
            {
                var server = redis.GetServer(endpoint);
                var svm = new RedisServer(server) { Display = "Redis Server" };

                var info = server.Info("keyspace");
                var databases = server.ConfigGet("databases");
                if (databases != null)
                {
                    int dbcounter = 0;
                    if (int.TryParse(databases.First().Value, out dbcounter))
                    {
                        foreach (var dbnumber in Enumerable.Range(0, dbcounter))
                        {
                            var display = dbnumber.ToString();
                            var dbinfo = info[0].FirstOrDefault(x => x.Key == "db" + display);

                            //if (dbinfo != null)
                            //{
                                display += " " + dbinfo.Value;
                            //}


                            svm.Children.Add(new RedisDatabase(svm, redis.GetDatabase(dbnumber)) { Display = display });
                        }
                    }
                }

                Servers.Add(svm);
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

        #endregion
    }
}
