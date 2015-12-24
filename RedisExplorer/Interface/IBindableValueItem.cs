using Caliburn.Micro;

namespace RedisExplorer.Interface
{
    public interface IBindableValueItem<T> : IScreen
    {
        BindableCollection<T> KeyValue { get; set; }
    }
}
