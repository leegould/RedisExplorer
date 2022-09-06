namespace RedisExplorer.Models.Classes
{
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
}
