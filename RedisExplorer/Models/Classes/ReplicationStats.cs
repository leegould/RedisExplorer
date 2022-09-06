namespace RedisExplorer.Models.Classes
{
    public class ReplicationStats
    {
        public string Role { get; set; }
        public string Slaves { get; set; }
        public string ReplOffset { get; set; }
        public string BacklogActive { get; set; }
        public string BacklogSize { get; set; }
        public string BacklogFirstByteOffset { get; set; }
        public string BacklogHistLen { get; set; }
    }
}
