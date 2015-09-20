using System.ComponentModel.Composition;
using Caliburn.Micro;
using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    [Export(typeof(KeyViewModel))]
    public class KeyViewModel : Screen, IHandle<TreeItemSelectedMessage>
    {
        private readonly IEventAggregator eventAggregator;

        private string keyNameTextBox;
        private string keyValueTextBox;

        public string KeyNameTextBox
        {
            get
            {
                return keyNameTextBox;
            }
            set
            {
                keyNameTextBox = value;
                NotifyOfPropertyChange(() => KeyNameTextBox);
            }
        }

        public string KeyValueTextBox
        {
            get
            {
                return keyValueTextBox;
            }
            set
            {
                keyValueTextBox = value;
                NotifyOfPropertyChange(() => KeyValueTextBox);
            }
        }

        public KeyViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
        }

        public void Handle(TreeItemSelectedMessage message)
        {
            if (message != null && message.SelectedItem.GetType() == typeof(RedisKey) && !message.SelectedItem.HasChildren)
            {
                KeyNameTextBox = message.SelectedItem.Display;

                
            }
        }
    }
}
