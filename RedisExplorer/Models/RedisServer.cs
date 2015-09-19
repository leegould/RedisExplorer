using System.Linq;
using Caliburn.Micro;
using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisServer : TreeViewItem
    {
        private IEventAggregator eventAggregator { get; set; }

        private IConnectionMultiplexer connection { get; set; }

        public IServer server { get; set; }

        public RedisServer(IConnectionMultiplexer connectionMultiplexer, IServer server, IEventAggregator eventAggregator) : base(null, true, eventAggregator)
        {
            this.connection = connectionMultiplexer;
            this.server = server;
            this.eventAggregator = eventAggregator;
        }

        protected override void LoadChildren()
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

                        var db = new RedisDatabase(this, connection.GetDatabase(dbnumber), eventAggregator) { Display = display };

                        Children.Add(db);
                    }
                }
            }
        }
    }
}
