using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace RedisExplorer.Controls
{
    [Export(typeof(PreferencesViewModel))]
    public class PreferencesViewModel : Screen
    {
        private readonly IEventAggregator eventAggregator;

        private string maxKeysTextBox { get; set; }

        public string MaxKeysTextBox
        {
            get { return maxKeysTextBox; }
            set
            {
                maxKeysTextBox = value;
                NotifyOfPropertyChange(() => MaxKeysTextBox);
            }
        }

        public PreferencesViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
        }

        public void SaveButton()
        {

        }

        public void CancelButton()
        {
            TryClose();
        }
    }
}
