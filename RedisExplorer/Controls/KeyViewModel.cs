using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace RedisExplorer.Controls
{
    [Export(typeof(KeyViewModel))]
    public class KeyViewModel : PropertyChangedBase
    {
        private readonly IEventAggregator eventAggregator;

        private string key;

        public KeyViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
        }
    }
}
