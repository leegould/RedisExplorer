using System.ComponentModel.Composition;
using Caliburn.Micro;
using RedisExplorer.Properties;

namespace RedisExplorer.Controls
{
    [Export(typeof(PreferencesViewModel))]
    public class PreferencesViewModel : Screen
    {
        private readonly IEventAggregator eventAggregator;

        private string maxKeysTextBox { get; set; }

        private string urnSeparatorTextBox { get; set; }

        private bool oneClickCheckBox { get; set; }

        private bool lazyLoadServerCheckBox { get; set; }

        private bool lazyLoadDatabaseCheckBox { get; set; }

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

        public PreferencesViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            MaxKeysTextBox = string.IsNullOrEmpty(Settings.Default.MaxKeys) ? "1000" : Settings.Default.MaxKeys;
            UrnSeparatorTextBox = string.IsNullOrEmpty(Settings.Default.UrnSeparator) ? ":" : Settings.Default.UrnSeparator;
            OneClickCheckBox = Settings.Default.OneClick;
        }

        public void SaveButton()
        {
            Settings.Default.MaxKeys = MaxKeysTextBox;
            Settings.Default.UrnSeparator = UrnSeparatorTextBox;
            Settings.Default.OneClick = OneClickCheckBox;
            Settings.Default.Save();

            TryClose();
        }

        public void CancelButton()
        {
            TryClose();
        }
    }
}
