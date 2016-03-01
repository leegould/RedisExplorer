using System.ComponentModel.Composition;

using Caliburn.Micro;

using RedisExplorer.Messages;
using RedisExplorer.Models;

namespace RedisExplorer.Controls
{
    [Export(typeof(AddConnectionViewModel))]
    public class AddConnectionViewModel : Screen
    {
        private string nameTextBox;

        private string addressTextBox;

        private string portTextBox;

        private readonly IEventAggregator eventAggregator;

        #region Properties

        public string NameTextBox
        {
            get
            {
                return nameTextBox;
            }
            set
            {
                nameTextBox = value;
                NotifyOfPropertyChange(() => NameTextBox);
            }
        }

        public string AddressTextBox
        {
            get
            {
                return addressTextBox;
            }
            set
            {
                addressTextBox = value;
                NotifyOfPropertyChange(() => AddressTextBox);
            }
        }

        public string PortTextBox
        {
            get
            {
                return portTextBox;
            }
            set
            {
                portTextBox = value;
                NotifyOfPropertyChange(() => PortTextBox);
            }
        }

        #endregion

        public AddConnectionViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            NameTextBox = "Redis Server";
            AddressTextBox = "localhost";
            PortTextBox = "6379";
        }

        #region Button Actions

        public void SaveButton()
        {
            var connection = new RedisConnection
                             {
                                 Address = AddressTextBox,
                                 Name = NameTextBox,
                                 Port = int.Parse(PortTextBox)
                             };

            eventAggregator.PublishOnUIThread(new AddConnectionMessage { Connection = connection });

            TryClose();
        }

        public void CancelButton()
        {
            TryClose();
        }

        #endregion
    }
}
