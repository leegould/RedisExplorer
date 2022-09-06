﻿using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Dynamic;
using System.Linq;
using System.Windows;

using Caliburn.Micro;
using RedisExplorer.Controls;
using RedisExplorer.Interface;
using RedisExplorer.Messages;
using RedisExplorer.Models;
using RedisExplorer.Properties;

namespace RedisExplorer
{
    [Export(typeof(AppViewModel))]
    public sealed class AppViewModel : Conductor<IDisplayPanel>.Collection.OneActive, IApp, IHandle<TreeItemSelectedMessage>, IHandle<TreeItemExpandedMessage>, IHandle<AddConnectionMessage>, IHandle<DeleteConnectionMessage>, IHandle<RedisKeyAddedMessage>, IHandle<RedisKeyUpdatedMessage>, IHandle<ConnectionFailedMessage>, IHandle<InfoNotValidMessage>, IHandle<ReloadKeyMessage>, IHandle<ServerReloadMessage>, IHandle<KeyDeletedMessage>, IHandle<DatabaseReloadMessage>, IHandle<AddKeyMessage>, IHandle<KeysDeletedMessage>
    {
        #region Private members

        private readonly IEventAggregator eventAggregator;

        private readonly IWindowManager windowManager;

        private string statusBarTextBlock;

        private BindableCollection<RedisServer> servers; 

        #endregion

        #region Properties

        public BindableCollection<RedisServer> Servers
        {
            get { return servers; }
            set
            {
                servers = value;
                NotifyOfPropertyChange(() => Servers);
            }
        }

        #endregion

        public AppViewModel(IEventAggregator eventAggregator, IWindowManager windowManager)
        {
            DisplayName = "Redis Explorer";

            //SetTheme();

            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            this.windowManager = windowManager;
            Servers = new BindableCollection<RedisServer>();

            KeyViewModel = new KeyViewModel(eventAggregator);
            KeyViewModel.ConductWith(this);

            DefaultViewModel = new DefaultViewModel();
            DefaultViewModel.ConductWith(this);

            ServerViewModel = new ServerViewModel(eventAggregator);
            ServerViewModel.ConductWith(this);

            DatabaseViewModel = new DatabaseViewModel(eventAggregator);
            DatabaseViewModel.ConductWith(this);

            ActivateItem(DefaultViewModel);


            LoadServers();
        }

        //private void SetTheme()
        //{
        //    ThemeManager.ChangeAppStyle(Application.Current,
        //                                ThemeManager.GetAccent(Settings.Default.Accent),
        //                                ThemeManager.GetAppTheme(Settings.Default.Theme));
        //}

        private void LoadServers()
        {
            Servers.Clear();
            if (Settings.Default.Servers != null)
            {
                foreach (var conn in from string connection in Settings.Default.Servers select new RedisConnection(connection) into server select new RedisServer(server.Name, server.Address + ",keepAlive = 180,allowAdmin=true", eventAggregator))
                {
                    Servers.Add(conn);
                }
            }
        }

        #region Properties

        public KeyViewModel KeyViewModel { get; set; }

        public DefaultViewModel DefaultViewModel { get; set; }

        public ServerViewModel ServerViewModel { get; set; }

        public DatabaseViewModel DatabaseViewModel { get; set; }

        public string StatusBarTextBlock
        {
            get
            {
                return statusBarTextBlock;
            }
            set
            {
                statusBarTextBlock = value;
                NotifyOfPropertyChange(() => StatusBarTextBlock);
            }
        }

        #endregion

        #region Menu

        public void Exit()
        {
            Application.Current.Shutdown();
        }


        public void AddServer()
        {
            dynamic settings = new ExpandoObject();
            settings.Width = 400;
            settings.Height = 275;
            settings.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            settings.Title = "Add Server";

            windowManager.ShowDialog(new AddConnectionViewModel(eventAggregator), null, settings);
        }

        public void Preferences()
        {
            dynamic settings = new ExpandoObject();
            settings.Width = 400;
            settings.Height = 550;
            settings.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            settings.Title = "Preferences";

            windowManager.ShowDialog(new PreferencesViewModel(eventAggregator), null, settings);    
        }

        public void About()
        {
            ActivateItem(DefaultViewModel);
            StatusBarTextBlock = string.Empty;
        }

        #endregion

        public void Handle(AddConnectionMessage message)
        {
            StringCollection connections = Settings.Default.Servers ?? new StringCollection();

            var newconn = new RedisConnection
            {
                Name = message.Connection.Name,
                Address = message.Connection.Address,
                Port = message.Connection.Port
            };

            connections.Add(newconn.ToString());

            Settings.Default.Servers = connections;
            Settings.Default.Save();

            StatusBarTextBlock = "Connection Added : " + newconn.Name;

            LoadServers();
        }

        public void Handle(DeleteConnectionMessage message)
        {
            StatusBarTextBlock = "Connection Deleted";
            LoadServers();
        }

        public void Handle(RedisKeyAddedMessage message)
        {
            StatusBarTextBlock = "Added Key : " + message.Key.KeyName;
        }

        public void Handle(ConnectionFailedMessage message)
        {
            StatusBarTextBlock = "Could not connect : " + message.ErrorMessage;
        }

        public void Handle(InfoNotValidMessage message)
        {
            StatusBarTextBlock = "Could not query database counts.";
        }

        public void Handle(RedisKeyUpdatedMessage message)
        {
            StatusBarTextBlock = "Updated Key : " + message.Key.KeyName;
        }

        public void Handle(ReloadKeyMessage message)
        {
            StatusBarTextBlock = "Reloaded Key : " + message.Urn;
        }

        public void Handle(ServerReloadMessage message)
        {
            StatusBarTextBlock = "Reload Server : " + message.Name;
        }

        public void Handle(KeyDeletedMessage message)
        {
            StatusBarTextBlock = "Deleted Key : " + message.Key.KeyName;
        }

        public void Handle(KeysDeletedMessage message)
        {
            StatusBarTextBlock = "Deleted " + message.Keys.Count + " Keys";
        }

        public void Handle(DatabaseReloadMessage message)
        {
            StatusBarTextBlock = "Reloaded Database : " + message.DbNumber;
        }

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message.SelectedItem is RedisServer)
            {
                ActivateItem(ServerViewModel);
                StatusBarTextBlock = "Connecting to server : " + message.SelectedItem.Display;
            }
            else if (message.SelectedItem is RedisDatabase)
            {
                ActivateItem(DatabaseViewModel);
                StatusBarTextBlock = "Selected Database : " + message.SelectedItem.Display;
            }
            else
            {
                ActivateItem(KeyViewModel);
                StatusBarTextBlock = "Selected : " + message.SelectedItem.Display;
            }
        }

        public void Handle(TreeItemExpandedMessage message)
        {
            StatusBarTextBlock = "Expanded : " + message.SelectedItem.Display;
        }

        public void Handle(AddKeyMessage message)
        {
            if (!ActiveItem.Equals(KeyViewModel))
            {
                ActivateItem(KeyViewModel);
            }
        }
    }
}
