using System.Windows;

using Caliburn.Micro;

namespace RedisExplorer
{
    public class AppWindowManager : WindowManager
    {
        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            var window = base.EnsureWindow(model, view, isDialog);

            window.SizeToContent = SizeToContent.Manual;

            window.Title = "Redis Explorer";

            return window;
        }
    }
}
