using System.Windows;

using Caliburn.Micro;

namespace RedisExplorer
{
    public class AppWindowManager : WindowManager
    {
        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            var window = base.EnsureWindow(model, view, isDialog);

            var m = model as AppViewModel;

            window.SizeToContent = SizeToContent.Manual;

            window.Title = "Redis Explorer";
            //window.Icon = new BitmapImage(new Uri("pack://application:,,,/RedisExplorer;component/Assets/Images/arrows.ico"));

            //window.Top = appsettings.Top;
            //window.Left = appsettings.Left;
            //window.Width = 600d;
            //window.Height = 400d;

            return window;
        }
    }
}
