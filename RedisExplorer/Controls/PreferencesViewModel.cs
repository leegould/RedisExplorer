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

        public PreferencesViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            MaxKeysTextBox = string.IsNullOrEmpty(Settings.Default.MaxKeys) ? "1000" : Settings.Default.MaxKeys;
            UrnSeparatorTextBox = string.IsNullOrEmpty(Settings.Default.UrnSeparator) ? ":" : Settings.Default.UrnSeparator;
        }

        public void SaveButton()
        {
            Settings.Default.MaxKeys = MaxKeysTextBox;
            Settings.Default.UrnSeparator = UrnSeparatorTextBox;
            Settings.Default.Save();

            TryClose();
        }

        public void CancelButton()
        {
            TryClose();
        }
    }
}
