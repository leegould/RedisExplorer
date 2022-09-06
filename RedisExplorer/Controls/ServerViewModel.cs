using Caliburn.Micro;
using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;
using RedisExplorer.Models.Classes;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

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
            redisServer?.Reload();
        }

        public ServerViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe(this);
        }

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message?.SelectedItem is RedisServer)
            {
                redisServer = (RedisServer) message.SelectedItem;

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

                replicationstats.TryGetValue("role", out string role);
                replicationstats.TryGetValue("connected_slaves", out string slaves);
                replicationstats.TryGetValue("master_repl_offset", out string replOffset);
                replicationstats.TryGetValue("repl_backlog_active", out string backlogActive);
                replicationstats.TryGetValue("repl_backlog_size", out string backlogSize);
                replicationstats.TryGetValue("repl_backlog_first_byte_offset", out string backlogFirstByteOffset);
                replicationstats.TryGetValue("repl_backlog_histlen", out string backlogHistLen);

                ReplicationStatistics = new ReplicationStats {
                    Role = role,
                    Slaves = slaves,
                    ReplOffset = replOffset,
                    BacklogActive = backlogActive,
                    BacklogSize = backlogSize,
                    BacklogFirstByteOffset = backlogFirstByteOffset,
                    BacklogHistLen = backlogHistLen
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

                cpustatsdict.TryGetValue("used_cpu_sys", out string usedCPUSys);
                cpustatsdict.TryGetValue("used_cpu_sys_children", out string usedCPUSysChildren);
                cpustatsdict.TryGetValue("used_cpu_user", out string usedCPUUser);
                cpustatsdict.TryGetValue("used_cpu_user_children", out string usedCPUUserChildren);

                CPUStatistics = new CPUStats {
                    UsedCPUSys = usedCPUSys,
                    UsedCPUSysChildren = usedCPUSysChildren,
                    UsedCPUUser = usedCPUUser,
                    UsedCPUUserChildren = usedCPUUserChildren
                };
            }
        }

        private void SetMemoryStats(IGrouping<string, KeyValuePair<string, string>> memorystats)
        {
            if (memorystats != null)
            {
                var memorystatsdict = memorystats.ToDictionary(x => x.Key, x => x.Value);

                memorystatsdict.TryGetValue("used_memory", out string usedMemory);
                memorystatsdict.TryGetValue("used_memory_human", out string usedMemoryHuman);
                memorystatsdict.TryGetValue("used_memory_lua", out string usedMemoryLua);
                memorystatsdict.TryGetValue("used_memory_peak", out string usedMemoryPeak);
                memorystatsdict.TryGetValue("used_memory_peak_human", out string usedMemoryPeakHuman);
                memorystatsdict.TryGetValue("used_memory_rss", out string usedMemoryRss);
                memorystatsdict.TryGetValue("mem_fragmentation_ratio", out string memFragmentationRatio);
                memorystatsdict.TryGetValue("mem_allocator", out string memAllocator);

                MemoryStatistics = new MemoryStats {
                    UsedMemory = usedMemory,
                    UsedMemoryHuman = usedMemoryHuman,
                    UsedMemoryLua = usedMemoryLua,
                    UsedMemoryPeak = usedMemoryPeak,
                    UsedMemoryPeakHuman = usedMemoryPeakHuman,
                    UsedMemoryRss = usedMemoryRss,
                    MemFragmentationRatio = memFragmentationRatio,
                    MemAllocator = memAllocator
                };
            }
        }

        private void SetClientStats(IGrouping<string, KeyValuePair<string, string>> clientstats)
        {
            if (clientstats != null)
            {
                var clientstatsdict = clientstats.ToDictionary(x => x.Key, x => x.Value);
                                
                clientstatsdict.TryGetValue("client_biggest_input_buf", out string biggestInputBuf);
                clientstatsdict.TryGetValue("client_longest_output_list", out string longestOutputList);
                clientstatsdict.TryGetValue("blocked_clients", out string blockedClients);
                clientstatsdict.TryGetValue("connected_clients", out string connectedClients);

                ClientStatistics = new ClientStats {
                                                        BiggestInputBuf = biggestInputBuf,
                                                        LongestOutputList = longestOutputList,
                                                        BlockedClients = blockedClients,
                                                        ConnectedClients = connectedClients
                                                    };
            }
        }

        private void SetServerStats(IGrouping<string, KeyValuePair<string, string>> serverstats)
        {
            if (serverstats != null)
            {
                var serverstatsdict = serverstats.ToDictionary(x => x.Key, x => x.Value);

                serverstatsdict.TryGetValue("redis_version", out string redisVersion);
                serverstatsdict.TryGetValue("redis_git_sha1", out string gitSHA1);
                serverstatsdict.TryGetValue("redis_git_dirty", out string gitDirty);
                serverstatsdict.TryGetValue("redis_build_id", out string buildId);
                serverstatsdict.TryGetValue("redis_mode", out string mode);
                serverstatsdict.TryGetValue("arch_bits", out string archBits);
                serverstatsdict.TryGetValue("config_file", out string configFile);
                serverstatsdict.TryGetValue("hz", out string hz);
                serverstatsdict.TryGetValue("lru_clock", out string lrUClock);
                serverstatsdict.TryGetValue("multiplexing_api", out string multiplexingApi);
                serverstatsdict.TryGetValue("os", out string os);
                serverstatsdict.TryGetValue("process_id", out string processId);
                serverstatsdict.TryGetValue("run_id", out string runId);
                serverstatsdict.TryGetValue("tcp_port", out string tcpPort);
                serverstatsdict.TryGetValue("uptime_in_days", out string uptimeInDays);
                serverstatsdict.TryGetValue("uptime_in_seconds", out string uptimeInSeconds);

                ServerStatistics = new ServerStats {
                    RedisVersion = redisVersion,
                    GitSHA1 = gitSHA1,
                    GitDirty = gitDirty,
                    BuildId = buildId,
                    Mode = mode,
                    ArchBits = archBits,
                    ConfigFile = configFile,
                    GCCVersion = serverstatsdict.ContainsKey("gcc_version")
                            ? serverstatsdict["gcc_version"]
                            : string.Empty,
                    HZ = hz,
                    LRUClock = lrUClock,
                    MultiplexingApi = multiplexingApi,
                    OS = os,
                    ProcessId = processId,
                    RunId = runId,
                    TCPPort = tcpPort,
                    UptimeInDays = uptimeInDays,
                    UptimeInSeconds = uptimeInSeconds
                };
            }
        }
    }
}
