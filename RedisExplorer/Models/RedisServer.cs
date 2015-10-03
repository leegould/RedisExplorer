using System.Collections.Specialized;
using System.Linq;
using Caliburn.Micro;
using RedisExplorer.Messages;
using RedisExplorer.Properties;
using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisServer : TreeViewItem
    {
        private IEventAggregator eventAggregator { get; set; }

        private IConnectionMultiplexer connection { get; set; }

        public RedisServer(string displayName, string connectionString, IEventAggregator eventAggregator) : base(null, true, eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            Display = displayName;

            try
            {
                connection = ConnectionMultiplexer.Connect(connectionString);
            }
            catch (RedisConnectionException rce)
            {
                // todo: log
                throw;
            }
        }

        public IServer GetServer()
        {
            // TODO : this just gets first one
            return connection.GetEndPoints().Select(endpoint => connection.GetServer(endpoint)).FirstOrDefault();
        }

        public IDatabase GetDatabase(int dbnumber)
        {
            return connection.GetDatabase(dbnumber);
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

        public async void Delete(RedisServer server)
        {
            if (Settings.Default.Servers != null)
            {
                var servers = new StringCollection();
                servers.AddRange(Settings.Default.Servers.Cast<string>().Where(x => x.Contains(server.Display + ';')).ToArray());
                Settings.Default.Servers = servers;
                Settings.Default.Save();

                eventAggregator.PublishOnUIThread(new DeleteConnectionMessage());
            }
        }
    }
}
