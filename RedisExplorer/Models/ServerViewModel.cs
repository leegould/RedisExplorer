using System.Collections.ObjectModel;
using System.Linq;

namespace RedisExplorer.Models
{
    public class ServerViewModel
    {
        private readonly ReadOnlyCollection<DatabaseViewModel> databases; 

        public ServerViewModel(RedisDatabase[] dbs)
        {
            databases = new ReadOnlyCollection<DatabaseViewModel>(
                (from db in dbs select new DatabaseViewModel(db)).ToList());
        }

        public ReadOnlyCollection<DatabaseViewModel> Databases
        {
            get
            {
                return databases;
            }
        }

        public string Name
        {
            get
            {
                return "Server Name";
            }
        }
    }
}
