namespace RedisExplorer.Models.Classes
{
    public class Stats
    {
        public string ConnectionsReceived { get; set; }
        public string CommandsProcessed { get; set; }
        public string OpsPerSec { get; set; }
        public string RejectedConnections { get; set; }
        public string SyncFull { get; set; }
        public string SyncPartialOk { get; set; }
        public string SyncPartialErr { get; set; }
        public string ExpiredKeys { get; set; }
        public string EvictedKeys { get; set; }
        public string KeyspaceHits { get; set; }
        public string KeyspaceMisses { get; set; }
        public string PubsubChannels { get; set; }
        public string PubsubPatterns { get; set; }
        public string LatestForkUsec { get; set; }
    }
}
