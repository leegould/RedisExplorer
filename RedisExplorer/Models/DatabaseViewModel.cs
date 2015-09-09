namespace RedisExplorer.Models
{
    public class DatabaseViewModel : TreeViewItemViewModel
    {
        private readonly RedisDatabase rdatabase;

        public DatabaseViewModel(RedisDatabase db)
            : base(null, false)
        {
            rdatabase = db;
        }

        public string Name 
        { 
            get
            {
                return rdatabase.Name;
            } 
        }
    }
}
