namespace RedisExplorer.Interface
{
    public interface IKeyValue<T>
    {
        T KeyValue { get; set; }
    }
}
