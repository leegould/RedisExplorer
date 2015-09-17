using System.Linq;

using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public class RedisServer : TreeViewItem
    {
        private IConnectionMultiplexer connection { get; set; }

        public IServer server { get; set; }

        public RedisServer(IConnectionMultiplexer connectionMultiplexer, IServer server) : base(null, true)
        {
            this.connection = connectionMultiplexer;
            this.server = server;
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

                        var db = new RedisDatabase(this, connection.GetDatabase(dbnumber)) { Display = display };

                        Children.Add(db);
                    }
                }
            }
        }
    }
}
