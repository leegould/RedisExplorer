using System.Collections.Generic;
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

        private Stats statistics;

        private PersistenceStats persistenceStatistics;

        private ReplicationStats replicationStatistics;

        private ClusterStats clusterStatistics;

        private KeySpaceStats keySpaceStatistics;

        private string serverName;

        public bool Db0IsVisible { get; set; }
        public bool Db1IsVisible { get; set; }
        public bool Db2IsVisible { get; set; }
        public bool Db3IsVisible { get; set; }
        public bool Db4IsVisible { get; set; }
        public bool Db5IsVisible { get; set; }
        public bool Db6IsVisible { get; set; }
        public bool Db7IsVisible { get; set; }
        public bool Db8IsVisible { get; set; }
        public bool Db9IsVisible { get; set; }
        public bool Db10IsVisible { get; set; }
        public bool Db11IsVisible { get; set; }
        public bool Db12IsVisible { get; set; }
        public bool Db13IsVisible { get; set; }
        public bool Db14IsVisible { get; set; }
        public bool Db15IsVisible { get; set; }
        public bool Db16IsVisible { get; set; }

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

        public Stats Statistics
        {
            get
            {
                return statistics;
            }
            set
            {
                statistics = value;
                NotifyOfPropertyChange(() => Statistics);
            }
        }

        public PersistenceStats PersistenceStatistics
        {
            get
            {
                return persistenceStatistics;
            }
            set
            {
                persistenceStatistics = value;
                NotifyOfPropertyChange(() => PersistenceStatistics);
            }
        }

        public ReplicationStats ReplicationStatistics
        {
            get { return replicationStatistics; }
            set
            {
                replicationStatistics = value;
                NotifyOfPropertyChange(() => ReplicationStatistics);
            }
        }

        public ClusterStats ClusterStatistics
        {
            get { return clusterStatistics; }
            set
            {
                clusterStatistics = value;
                NotifyOfPropertyChange(() => ClusterStatistics);
            }
        }

        public KeySpaceStats KeySpaceStatistics
        {
            get { return keySpaceStatistics; }
            set
            {
                keySpaceStatistics = value;
                NotifyOfPropertyChange(() => KeySpaceStatistics);
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

            SetServerStats(serverInfo.FirstOrDefault(x => x.Key.ToLower() == "server"));

            SetClientStats(serverInfo.FirstOrDefault(x => x.Key.ToLower() == "clients"));

            SetMemoryStats(serverInfo.FirstOrDefault(x => x.Key.ToLower() == "memory"));

            SetCPUStats(serverInfo.FirstOrDefault(x => x.Key.ToLower() == "cpu"));

            SetStats(serverInfo.FirstOrDefault(x => x.Key.ToLower() == "stats"));

            SetPersistenceStats(serverInfo.FirstOrDefault(x => x.Key.ToLower() == "persistence"));

            SetReplicationStats(serverInfo.FirstOrDefault(x => x.Key.ToLower() == "replication"));

            SetClusterStats(serverInfo.FirstOrDefault(x => x.Key.ToLower() == "cluster"));

            SetKeyspaceStats(serverInfo.FirstOrDefault(x => x.Key.ToLower() == "keyspace"));
        }

        private void SetKeyspaceStats(IGrouping<string, KeyValuePair<string, string>> stats)
        {
            if (stats != null)
            {
                var keyspacestats = stats.ToDictionary(x => x.Key, x => x.Value);

                KeySpaceStatistics = new KeySpaceStats
                                     {
                                         Db0 = keyspacestats.ContainsKey("db0") ? keyspacestats["db0"] : string.Empty,
                                         Db1 = keyspacestats.ContainsKey("db1") ? keyspacestats["db1"] : string.Empty,
                                         Db2 = keyspacestats.ContainsKey("db2") ? keyspacestats["db2"] : string.Empty,
                                         Db3 = keyspacestats.ContainsKey("db3") ? keyspacestats["db3"] : string.Empty,
                                         Db4 = keyspacestats.ContainsKey("db4") ? keyspacestats["db4"] : string.Empty,
                                         Db5 = keyspacestats.ContainsKey("db5") ? keyspacestats["db5"] : string.Empty,
                                         Db6 = keyspacestats.ContainsKey("db6") ? keyspacestats["db6"] : string.Empty,
                                         Db7 = keyspacestats.ContainsKey("db7") ? keyspacestats["db7"] : string.Empty,
                                         Db8 = keyspacestats.ContainsKey("db8") ? keyspacestats["db8"] : string.Empty,
                                         Db9 = keyspacestats.ContainsKey("db9") ? keyspacestats["db9"] : string.Empty,
                                         Db10 = keyspacestats.ContainsKey("db10") ? keyspacestats["db10"] : string.Empty,
                                         Db11 = keyspacestats.ContainsKey("db11") ? keyspacestats["db11"] : string.Empty,
                                         Db12 = keyspacestats.ContainsKey("db12") ? keyspacestats["db12"] : string.Empty,
                                         Db13 = keyspacestats.ContainsKey("db13") ? keyspacestats["db13"] : string.Empty,
                                         Db14 = keyspacestats.ContainsKey("db14") ? keyspacestats["db14"] : string.Empty,
                                         Db15 = keyspacestats.ContainsKey("db15") ? keyspacestats["db15"] : string.Empty
                                     };

                Db0IsVisible = keyspacestats.ContainsKey("db0");
                Db1IsVisible = keyspacestats.ContainsKey("db1");
                Db2IsVisible = keyspacestats.ContainsKey("db2");
                Db3IsVisible = keyspacestats.ContainsKey("db3");
                Db4IsVisible = keyspacestats.ContainsKey("db4");
                Db5IsVisible = keyspacestats.ContainsKey("db5");
                Db6IsVisible = keyspacestats.ContainsKey("db6");
                Db7IsVisible = keyspacestats.ContainsKey("db7");
                Db8IsVisible = keyspacestats.ContainsKey("db8");
                Db9IsVisible = keyspacestats.ContainsKey("db9");
                Db10IsVisible = keyspacestats.ContainsKey("db10");
                Db11IsVisible = keyspacestats.ContainsKey("db11");
                Db12IsVisible = keyspacestats.ContainsKey("db12");
                Db13IsVisible = keyspacestats.ContainsKey("db13");
                Db14IsVisible = keyspacestats.ContainsKey("db14");
                Db15IsVisible = keyspacestats.ContainsKey("db15");
            }
        }

        private void SetClusterStats(IGrouping<string, KeyValuePair<string, string>> clusterstats)
        {
            ClusterStatistics = new ClusterStats { Enabled = false.ToString() };

            if (clusterstats != null)
            {
                var cluststatsdict = clusterstats.ToDictionary(x => x.Key, x => x.Value);

                ClusterStatistics.Enabled = cluststatsdict["cluster_enabled"];
            }
        }

        private void SetReplicationStats(IGrouping<string, KeyValuePair<string, string>> stats)
        {
            if (stats != null)
            {
                var replicationstats = stats.ToDictionary(x => x.Key, x => x.Value);

                ReplicationStatistics = new ReplicationStats
                                        {
                                            Role = replicationstats["role"],
                                            Slaves = replicationstats["connected_slaves"],
                                            ReplOffset = replicationstats["master_repl_offset"],
                                            BacklogActive = replicationstats["repl_backlog_active"],
                                            BacklogSize = replicationstats["repl_backlog_size"],
                                            BacklogFirstByteOffset = replicationstats["repl_backlog_first_byte_offset"],
                                            BacklogHistLen = replicationstats["repl_backlog_histlen"]
                                        };
            }
        }

        private void SetPersistenceStats(IGrouping<string, KeyValuePair<string, string>> stats)
        {
            if (stats != null)
            {
                var persistencestats = stats.ToDictionary(x => x.Key, x => x.Value);

                PersistenceStatistics = new PersistenceStats
                                        {
                                            Loading = persistencestats["loading"],
                                            ChangesSinceLastSave =persistencestats["rdb_changes_since_last_save"],
                                            LastBgSaveTimeSec = persistencestats["rdb_last_bgsave_time_sec"],
                                            LastBgSaveStatus = persistencestats["rdb_last_bgsave_status"],
                                            BgSaveInProgress = persistencestats["rdb_bgsave_in_progress"],
                                            LastSaveTime = persistencestats["rdb_last_save_time"],
                                            CurrentBgSaveTimeSec = persistencestats["rdb_current_bgsave_time_sec"],
                                            AOFCurrentRewriteTimeSec = persistencestats["aof_current_rewrite_time_sec"],
                                            AOFEnabled = persistencestats["aof_enabled"],
                                            AOFLastBgRewriteStatus = persistencestats["aof_last_bgrewrite_status"],
                                            AOFRewriteInProgress = persistencestats["aof_rewrite_in_progress"],
                                            AOFRewriteScheduled = persistencestats["aof_rewrite_scheduled"],
                                            AOFLastRewriteTimeSec = persistencestats["aof_current_rewrite_time_sec"]
                                        };
            }
        }

        private void SetStats(IGrouping<string, KeyValuePair<string, string>> stats)
        {
            if (stats != null)
            {
                var statsdict = stats.ToDictionary(x => x.Key, x => x.Value);

                Statistics = new Stats
                             {
                                 CommandsProcessed = statsdict["total_commands_processed"],
                                 ConnectionsReceived = statsdict["total_connections_received"],
                                 EvictedKeys = statsdict["evicted_keys"],
                                 ExpiredKeys = statsdict["expired_keys"],
                                 KeyspaceHits = statsdict["keyspace_hits"],
                                 KeyspaceMisses = statsdict["keyspace_misses"],
                                 LatestForkUsec = statsdict["latest_fork_usec"],
                                 OpsPerSec = statsdict["instantaneous_ops_per_sec"],
                                 PubsubChannels = statsdict["pubsub_channels"],
                                 PubsubPatterns = statsdict["pubsub_patterns"],
                                 RejectedConnections = statsdict["rejected_connections"],
                                 SyncFull = statsdict["sync_full"],
                                 SyncPartialErr = statsdict["sync_partial_err"],
                                 SyncPartialOk = statsdict["sync_partial_ok"]
                             };
            }
        }

        private void SetCPUStats(IGrouping<string, KeyValuePair<string, string>> cpustats)
        {
            if (cpustats != null)
            {
                var cpustatsdict = cpustats.ToDictionary(x => x.Key, x => x.Value);

                CPUStatistics = new CPUStats
                                {
                                    UsedCPUSys = cpustatsdict["used_cpu_sys"],
                                    UsedCPUSysChildren = cpustatsdict["used_cpu_sys_children"],
                                    UsedCPUUser = cpustatsdict["used_cpu_user"],
                                    UsedCPUUserChildren = cpustatsdict["used_cpu_user_children"]
                                };
            }
        }

        private void SetMemoryStats(IGrouping<string, KeyValuePair<string, string>> memorystats)
        {
            if (memorystats != null)
            {
                var memorystatsdict = memorystats.ToDictionary(x => x.Key, x => x.Value);

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
            }
        }

        private void SetClientStats(IGrouping<string, KeyValuePair<string, string>> clientstats)
        {
            if (clientstats != null)
            {
                var clientstatsdict = clientstats.ToDictionary(x => x.Key, x => x.Value);

                ClientStatistics = new ClientStats
                                   {
                                       BiggestInputBuf = clientstatsdict["client_biggest_input_buf"],
                                       LongestOutputList = clientstatsdict["client_longest_output_list"],
                                       BlockedClients = clientstatsdict["blocked_clients"],
                                       ConnectedClients = clientstatsdict["connected_clients"]
                                   };
            }
        }

        private void SetServerStats(IGrouping<string, KeyValuePair<string, string>> serverstats)
        {
            if (serverstats != null)
            {
                var serverstatsdict = serverstats.ToDictionary(x => x.Key, x => x.Value);

                ServerStatistics = new ServerStats
                                   {
                                       RedisVersion = serverstatsdict["redis_version"],
                                       GitSHA1 = serverstatsdict["redis_git_sha1"],
                                       GitDirty = serverstatsdict["redis_git_dirty"],
                                       BuildId = serverstatsdict["redis_build_id"],
                                       Mode = serverstatsdict["redis_mode"],
                                       ArchBits = serverstatsdict["arch_bits"],
                                       ConfigFile = serverstatsdict["config_file"],
                                       GCCVersion =
                                           serverstatsdict.ContainsKey("gcc_version")
                                               ? serverstatsdict["gcc_version"]
                                               : string.Empty,
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
            }
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

        public class PersistenceStats
        {
            public string Loading { get; set; }
            public string ChangesSinceLastSave { get; set; }
            public string BgSaveInProgress { get; set; }
            public string LastSaveTime { get; set; }
            public string LastBgSaveStatus { get; set; }
            public string LastBgSaveTimeSec { get; set; }
            public string CurrentBgSaveTimeSec { get; set; }
            public string AOFEnabled { get; set; }
            public string AOFRewriteInProgress { get; set; }
            public string AOFRewriteScheduled { get; set; }
            public string AOFLastRewriteTimeSec { get; set; }
            public string AOFCurrentRewriteTimeSec { get; set; }
            public string AOFLastBgRewriteStatus { get; set; }
        }

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

        public class ClusterStats
        {
            public string Enabled { get; set; }
        }

        public class KeySpaceStats
        {
            public string Db0 { get; set; }
            public string Db1 { get; set; }
            public string Db2 { get; set; }
            public string Db3 { get; set; }
            public string Db4 { get; set; }
            public string Db5 { get; set; }
            public string Db6 { get; set; }
            public string Db7 { get; set; }
            public string Db8 { get; set; }
            public string Db9 { get; set; }
            public string Db10 { get; set; }
            public string Db11 { get; set; }
            public string Db12 { get; set; }
            public string Db13 { get; set; }
            public string Db14 { get; set; }
            public string Db15 { get; set; }
        }

        #endregion
    }
}
