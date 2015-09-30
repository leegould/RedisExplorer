namespace RedisExplorer.Models
{
    public class RedisConnection
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public int Port { get; set; }

        public override string ToString()
        {
            return Name + ";" + Address + ";" + Port;
        }

        public RedisConnection()
        {
            
        }

        public RedisConnection(string str)
        {
            var parts = str.Split(';');
            if (parts.Length == 3)
            {
                Name = parts[0];
                Address = parts[1];
                Port = int.Parse(parts[2]);
            }
        }
    }
}
