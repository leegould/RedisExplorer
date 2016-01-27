using System.ComponentModel.Composition;
using System.Linq;

using Caliburn.Micro;

using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    [Export(typeof(ServerViewModel))]
    public class ServerViewModel : Screen, IDisplayPanel, IHandle<TreeItemSelectedMessage>, IHandle<ServerReloadMessage>
    {
        private RedisServer redisServer;

        private ServerStats serverStatistics;

        private ClientStats clientStatistics;

        private MemoryStats memoryStatistics;

        private CPUStats cpuStats;

        private string serverName;

        #region Properties

        public string ServerName
        {
            get
            {
                return serverName;
            }
            set
            {
                serverName = value;
                NotifyOfPropertyChange(() => ServerName);
            }
        }

        public ServerStats ServerStatistics {
            get
            {
                return serverStatistics;
            }
            set
            {
                serverStatistics = value;
                NotifyOfPropertyChange(() => ServerStatistics);
            }
        }

        public ClientStats ClientStatistics
        {
            get
            {
                return clientStatistics;
            }
            set
            {
                clientStatistics = value;
                NotifyOfPropertyChange(() => ClientStatistics);
            }
        }

        public MemoryStats MemoryStatistics
        {
            get
            {
                return memoryStatistics;
            }
            set
            {
                memoryStatistics = value;
                NotifyOfPropertyChange(() => MemoryStatistics);
            }
        }

        public CPUStats CPUStatistics
        {
            get
            {
                return cpuStats;
            }
            set
            {
                cpuStats = value;
                NotifyOfPropertyChange(() => CPUStatistics);
            }
        }

        #endregion

        

        public void ReloadServer()
        {
            if (redisServer != null)
            {
                redisServer.Reload();
            }
        }

        public ServerViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe(this);
        }

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message != null && message.SelectedItem is RedisServer)
            {
                redisServer = message.SelectedItem as RedisServer;

                DisplayItem();
            }
        }

        public void Handle(ServerReloadMessage message)
        {
            DisplayItem();
        }

        private void DisplayItem()
        {
            ServerName = redisServer.Display;

            var serverInfo = redisServer.GetServerInfo().ToList();

            var serverstatsdict = serverInfo.FirstOrDefault(x => x.Key.ToLower() == "server").ToDictionary(x => x.Key, x => x.Value);

            ServerStatistics = new ServerStats
                               {
                                   RedisVersion = serverstatsdict["redis_version"],
                                   GitSHA1 = serverstatsdict["redis_git_sha1"],
                                   GitDirty = serverstatsdict["redis_git_dirty"],
                                   BuildId = serverstatsdict["redis_build_id"],
                                   Mode = serverstatsdict["redis_mode"],
                                   ArchBits = serverstatsdict["arch_bits"],
                                   ConfigFile = serverstatsdict["config_file"],
                                   GCCVersion = serverstatsdict.ContainsKey("gcc_version") ? serverstatsdict["gcc_version"] : string.Empty,
                                   HZ = serverstatsdict["hz"],
                                   LRUClock = serverstatsdict["lru_clock"],
                                   MultiplexingApi = serverstatsdict["multiplexing_api"],
                                   OS = serverstatsdict["os"],
                                   ProcessId = serverstatsdict["process_id"],
                                   RunId = serverstatsdict["run_id"],
                                   TCPPort = serverstatsdict["tcp_port"],
                                   UptimeInDays = serverstatsdict["uptime_in_days"],
                                   UptimeInSeconds = serverstatsdict["uptime_in_seconds"]
                               };

            var clientstatsdict = serverInfo.FirstOrDefault(x => x.Key.ToLower() == "clients").ToDictionary(x => x.Key, x => x.Value);

            ClientStatistics = new ClientStats
                               {
                                   BiggestInputBuf = clientstatsdict["client_biggest_input_buf"],
                                   LongestOutputList = clientstatsdict["client_longest_output_list"],
                                   BlockedClients = clientstatsdict["blocked_clients"],
                                   ConnectedClients = clientstatsdict["connected_clients"]
                               };

            var memorystatsdict = serverInfo.FirstOrDefault(x => x.Key.ToLower() == "memory").ToDictionary(x => x.Key, x => x.Value);


            MemoryStatistics = new MemoryStats
                               {
                                   UsedMemory = memorystatsdict["used_memory"],
                                   UsedMemoryHuman = memorystatsdict["used_memory_human"],
                                   UsedMemoryLua = memorystatsdict["used_memory_lua"],
                                   UsedMemoryPeak = memorystatsdict["used_memory_peak"],
                                   UsedMemoryPeakHuman = memorystatsdict["used_memory_peak_human"],
                                   UsedMemoryRss = memorystatsdict["used_memory_rss"],
                                   MemFragmentationRatio = memorystatsdict["mem_fragmentation_ratio"],
                                   MemAllocator = memorystatsdict["mem_allocator"]
                               };

            var cpustatsdict = serverInfo.FirstOrDefault(x => x.Key.ToLower() == "cpu").ToDictionary(x => x.Key, x => x.Value);

            CPUStatistics = new CPUStats
                            {
                                UsedCPUSys = cpustatsdict["used_cpu_sys"],
                                UsedCPUSysChildren = cpustatsdict["used_cpu_sys_children"],
                                UsedCPUUser = cpustatsdict["used_cpu_user"],
                                UsedCPUUserChildren = cpustatsdict["used_cpu_user_children"]
                            };
        }

        #region Classes

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

        public class ClientStats
        {
            public string ConnectedClients { get; set; }
            public string LongestOutputList { get; set; }
            public string BiggestInputBuf { get; set; }
            public string BlockedClients { get; set; }
        }

        public class MemoryStats
        {
            public string UsedMemory { get; set; }
            public string UsedMemoryHuman { get; set; }
            public string UsedMemoryRss { get; set; }
            public string UsedMemoryPeak { get; set; }
            public string UsedMemoryPeakHuman { get; set; }
            public string UsedMemoryLua { get; set; }
            public string MemFragmentationRatio { get; set; }
            public string MemAllocator { get; set; }
        }

        public class CPUStats
        {
            public string UsedCPUSys { get; set; }
            public string UsedCPUUser { get; set; }
            public string UsedCPUSysChildren { get; set; }
            public string UsedCPUUserChildren { get; set; }
        }

        #endregion
    }
}
