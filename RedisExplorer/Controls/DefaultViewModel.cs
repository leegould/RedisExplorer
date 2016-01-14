using System.Diagnostics;

using Caliburn.Micro;

using RedisExplorer.Interface;

namespace RedisExplorer.Controls
{
    public class DefaultViewModel : Screen, IDisplayPanel
    {
        public void NavigateTo(string url)
        {
            Process.Start(new ProcessStartInfo(url));
        }
    }
}
