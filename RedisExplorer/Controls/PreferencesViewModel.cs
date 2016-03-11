using System.Collections.Generic;
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
        #region Members

        private string maxKeysTextBox { get; set; }

        private string urnSeparatorTextBox { get; set; }

        private bool oneClickCheckBox { get; set; }

        private bool lazyLoadServerCheckBox { get; set; }

        private bool lazyLoadDatabaseCheckBox { get; set; }

        private string selectedTheme { get; set; }

        private string selectedAccent { get; set; }

        #endregion

        #region Properties

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

        public List<string> Themes => new List<string> { "BaseLight", "BaseDark" };

        public string SelectedTheme
        {
            get { return selectedTheme; }
            set
            {
                selectedTheme = value;
                NotifyOfPropertyChange(() => SelectedTheme);
            }
        }

        public List<string> Accents => new List<string> { "Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald", "Teal", "Cyan", "Cobalt", "Indigo", "Violet", "Pink", "Magenta", "Crimson", "Amber", "Yellow", "Brown", "Olive", "Steel", "Mauve", "Taupe", "Sienna" };

        public string SelectedAccent
        {
            get { return selectedAccent; }
            set
            {
                selectedAccent = value;
                NotifyOfPropertyChange(() => SelectedAccent);
            }
        }

        #endregion

        public PreferencesViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe(this);
            MaxKeysTextBox = string.IsNullOrEmpty(Settings.Default.MaxKeys) ? "1000" : Settings.Default.MaxKeys;
            UrnSeparatorTextBox = string.IsNullOrEmpty(Settings.Default.UrnSeparator) ? ":" : Settings.Default.UrnSeparator;
            OneClickCheckBox = Settings.Default.OneClick;
            LazyLoadServerCheckBox = Settings.Default.LazyLoadServer;
            LazyLoadDatabaseCheckBox = Settings.Default.LazyLoadDatabase;
            SelectedTheme = Settings.Default.Theme;
            SelectedAccent = Settings.Default.Accent;
        }

        public void SaveButton()
        {
            SaveSettings();

            ThemeManager.ChangeAppStyle(Application.Current, 
                                        ThemeManager.GetAccent(SelectedAccent), 
                                        ThemeManager.GetAppTheme(SelectedTheme));

            TryClose();
        }

        public void ApplyButton()
        {
            SaveSettings();

            ChangeTheme();
        }

        public void CancelButton()
        {
            TryClose();
        }

        private void ChangeTheme()
        {
            ThemeManager.ChangeAppStyle(Application.Current,
                ThemeManager.GetAccent(SelectedAccent),
                ThemeManager.GetAppTheme(SelectedTheme));
        }

        private void SaveSettings()
        {
            Settings.Default.MaxKeys = MaxKeysTextBox;
            Settings.Default.UrnSeparator = UrnSeparatorTextBox;
            Settings.Default.OneClick = OneClickCheckBox;
            Settings.Default.LazyLoadServer = LazyLoadServerCheckBox;
            Settings.Default.LazyLoadDatabase = LazyLoadDatabaseCheckBox;
            Settings.Default.Theme = SelectedTheme;
            Settings.Default.Accent = SelectedAccent;
            Settings.Default.Save();
        }
    }
}
