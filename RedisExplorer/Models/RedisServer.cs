using System.Collections.Specialized;
using System.Linq;
using Caliburn.Micro;
using RedisExplorer.Messages;
using RedisExplorer.Properties;
using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisServer : TreeViewItem, IHandle<FlushDbMessage>
    {
        private IEventAggregator eventAggregator { get; set; }

        private string connectionStr { get; set; }

        private IConnectionMultiplexer connection { get; set; }

        protected IConnectionMultiplexer Connection
        {
            get
            {
                if (connection != null) { return connection; }
                try
                {
                    connection = ConnectionMultiplexer.Connect(connectionStr);
                    return connection;
                }
                catch (RedisConnectionException rcException)
                {
                    eventAggregator.PublishOnUIThread(new ConnectionFailedMessage { ErrorMessage = rcException.Message });
                    return null;
                }
            }
        }

        public RedisServer(string displayName, string connectionString, IEventAggregator eventAggregator) : base(null, Settings.Default.LazyLoadServer, eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            Display = displayName;
            connectionStr = connectionString;
        }
        
        public IServer GetServer()
        {
            // TODO : this just gets first one
            return Connection != null ? Connection.GetEndPoints().Select(endpoint => Connection.GetServer(endpoint)).FirstOrDefault() : null;
        }

        public IDatabase GetDatabase(int dbnumber)
        {
            return Connection.GetDatabase(dbnumber);
        }

        protected override void LoadChildren()
        {
            var server = GetServer();

            if (server != null)
            {
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
                            if (info != null)
                            {
                                var dbinfo = info[0].FirstOrDefault(x => x.Key == "db" + display);
                                if (!string.IsNullOrEmpty(dbinfo.Value))
                                {
                                    display += " (" + dbinfo.Value.Split(',')[0].Split('=')[1] + ")";
                                }
                            }

                            var db = new RedisDatabase(this, dbnumber, eventAggregator)
                                     {
                                         Display = display
                                     };

                            Children.Add(db);
                        }
                    }
                }
            }
        }

        public void Reload(RedisServer server)
        {
            Children.Clear();
            LoadChildren();
        }

        public void Delete(RedisServer server)
        {
            if (Settings.Default.Servers != null)
            {
                var servers = new StringCollection();
                servers.AddRange(Settings.Default.Servers.Cast<string>().ToArray().Where(x => !x.StartsWith(server.Display + ";")).ToArray());
                Settings.Default.Servers = servers;
                Settings.Default.Save();

                eventAggregator.PublishOnUIThread(new DeleteConnectionMessage());
            }
        }

        public void Handle(FlushDbMessage message)
        {
            Children.Clear();
            LoadChildren();
        }
    }
}
