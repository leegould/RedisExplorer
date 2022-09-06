namespace RedisExplorer.Models.Classes
{
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
}
