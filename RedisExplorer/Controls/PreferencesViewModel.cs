using System.ComponentModel.Composition;
using System.Windows;
using Caliburn.Micro;
using MahApps.Metro;
using RedisExplorer.Properties;

namespace RedisExplorer.Controls
{
    [Export(typeof(PreferencesViewModel))]
    public class PreferencesViewModel : Screen
    {
        private string maxKeysTextBox { get; set; }

        private string urnSeparatorTextBox { get; set; }

        private bool oneClickCheckBox { get; set; }

        private bool lazyLoadServerCheckBox { get; set; }

        private bool lazyLoadDatabaseCheckBox { get; set; }

        private string themeTextBox { get; set; }

        public string MaxKeysTextBox
        {
            get { return maxKeysTextBox; }
            set
            {
                maxKeysTextBox = value;
                NotifyOfPropertyChange(() => MaxKeysTextBox);
            }
        }

        public string UrnSeparatorTextBox
        {
            get { return urnSeparatorTextBox; }
            set
            {
                urnSeparatorTextBox = value;
                NotifyOfPropertyChange(() => UrnSeparatorTextBox);
            }
        }

        public bool OneClickCheckBox
        {
            get {  return oneClickCheckBox; }
            set
            {
                oneClickCheckBox = value;
                NotifyOfPropertyChange(() => OneClickCheckBox);
            }
        }

        public bool LazyLoadServerCheckBox
        {
            get { return lazyLoadServerCheckBox; }
            set
            {
                lazyLoadServerCheckBox = value;
                NotifyOfPropertyChange(() => LazyLoadServerCheckBox);
            }
        }

        public bool LazyLoadDatabaseCheckBox
        {
            get { return lazyLoadDatabaseCheckBox; }
            set
            {
                lazyLoadDatabaseCheckBox = value;
                NotifyOfPropertyChange(() => LazyLoadDatabaseCheckBox);
            }
        }

        public string ThemeTextBox
        {
            get { return themeTextBox; }
            set
            {
                themeTextBox = value;
                NotifyOfPropertyChange(() => ThemeTextBox);
            }
        }

        public PreferencesViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe(this);
            MaxKeysTextBox = string.IsNullOrEmpty(Settings.Default.MaxKeys) ? "1000" : Settings.Default.MaxKeys;
            UrnSeparatorTextBox = string.IsNullOrEmpty(Settings.Default.UrnSeparator) ? ":" : Settings.Default.UrnSeparator;
            OneClickCheckBox = Settings.Default.OneClick;
            LazyLoadServerCheckBox = Settings.Default.LazyLoadServer;
            LazyLoadDatabaseCheckBox = Settings.Default.LazyLoadDatabase;
            ThemeTextBox = Settings.Default.Theme;
        }

        public void SaveButton()
        {
            Settings.Default.MaxKeys = MaxKeysTextBox;
            Settings.Default.UrnSeparator = UrnSeparatorTextBox;
            Settings.Default.OneClick = OneClickCheckBox;
            Settings.Default.LazyLoadServer = LazyLoadServerCheckBox;
            Settings.Default.LazyLoadDatabase = LazyLoadDatabaseCheckBox;
            Settings.Default.Theme = ThemeTextBox;
            Settings.Default.Save();

            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Green"), ThemeManager.GetAppTheme(Settings.Default.Theme));

            TryClose();
        }

        public void CancelButton()
        {
            TryClose();
        }
    }
}
