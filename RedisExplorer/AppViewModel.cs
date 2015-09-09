using System.ComponentModel.Composition;
using System.Windows;

using Caliburn.Micro;

using RedisExplorer.Interface;

namespace RedisExplorer
{
    [Export(typeof(AppViewModel))]
    public sealed class AppViewModel : Conductor<ITabItem>.Collection.OneActive, IApp
    {
        #region Private members

        private readonly IEventAggregator eventAggregator;

        private readonly IWindowManager windowManager;

        private string statusBarTextBlock;

        #endregion

        public AppViewModel(IEventAggregator eventAggregator, IWindowManager windowManager)
        {
            this.DisplayName = "RedisExplorer";

            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            this.windowManager = windowManager;
        }
        
        #region Properties

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

        #endregion
    }
}
