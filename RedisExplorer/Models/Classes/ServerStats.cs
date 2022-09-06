namespace RedisExplorer.Models.Classes
{
    public class ServerStats
    {
        public string RedisVersion { get; set; }
        public string GitSHA1 { get; set; }
        public string GitDirty { get; set; }
        public string BuildId { get; set; }
        public string Mode { get; set; }
        public string OS { get; set; }
        public string ArchBits { get; set; }
        public string MultiplexingApi { get; set; }
        public string GCCVersion { get; set; }
        public string ProcessId { get; set; }
        public string RunId { get; set; }
        public string TCPPort { get; set; }
        public string UptimeInSeconds { get; set; }
        public string UptimeInDays { get; set; }
        public string HZ { get; set; }
        public string LRUClock { get; set; }
        public string ConfigFile { get; set; }
    }
}
