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

        private string urnSeperatorTextBox { get; set; }

        public string MaxKeysTextBox
        {
            get { return maxKeysTextBox; }
            set
            {
                maxKeysTextBox = value;
                NotifyOfPropertyChange(() => MaxKeysTextBox);
            }
        }

        public string UrnSeperatorTextBox
        {
            get { return urnSeperatorTextBox; }
            set
            {
                urnSeperatorTextBox = value;
                NotifyOfPropertyChange(() => UrnSeperatorTextBox);
            }
        }

        public PreferencesViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            MaxKeysTextBox = string.IsNullOrEmpty(Settings.Default.MaxKeys) ? "1000" : Settings.Default.MaxKeys;
            UrnSeperatorTextBox = string.IsNullOrEmpty(Settings.Default.UrnSeperator) ? ";" : Settings.Default.UrnSeperator;
        }

        public void SaveButton()
        {
            Settings.Default.MaxKeys = MaxKeysTextBox;
            Settings.Default.UrnSeperator = UrnSeperatorTextBox;
            Settings.Default.Save();

            TryClose();
        }

        public void CancelButton()
        {
            TryClose();
        }
    }
}
