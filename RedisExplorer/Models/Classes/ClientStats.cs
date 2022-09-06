namespace RedisExplorer.Models.Classes
{
    public class ClientStats
    {
        public string ConnectedClients { get; set; }
        public string LongestOutputList { get; set; }
        public string BiggestInputBuf { get; set; }
        public string BlockedClients { get; set; }
    }
}
