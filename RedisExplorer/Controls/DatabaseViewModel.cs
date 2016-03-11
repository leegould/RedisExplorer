using System.ComponentModel.Composition;

using Caliburn.Micro;

using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    [Export(typeof(DatabaseViewModel))]
    public class DatabaseViewModel : Screen, IDisplayPanel, IHandle<TreeItemSelectedMessage>
    {
        private RedisDatabase redisDatabase;

        private string keyCount;

        private string dbName;

        public string DbName
        {
            get
            {
                return dbName;
            }
            set
            {
                dbName = value;
                NotifyOfPropertyChange(() => DbName);
            }
        }

        public string KeyCount
        {
            get
            {
                return keyCount;
            }
            set
            {
                keyCount = value;
                NotifyOfPropertyChange(() => KeyCount);
            }
        }

        public DatabaseViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe(this);
        }

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message?.SelectedItem is RedisDatabase)
            {
                redisDatabase = message.SelectedItem as RedisDatabase;

                DbName = redisDatabase.GetDatabaseNumber.ToString();
                KeyCount = redisDatabase.GetKeyCount.ToString();
            }
        }

        public void Flush()
        {
            redisDatabase?.Flush();
        }

        public void AddKey()
        {
            redisDatabase?.Add();
        }

        public void Reload()
        {
            redisDatabase?.Reload();
        }
    }
}
