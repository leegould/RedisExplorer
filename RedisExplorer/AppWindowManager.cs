using System.Windows;

using Caliburn.Micro;

namespace RedisExplorer
{
    public class AppWindowManager : WindowManager
    {
        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            // This doesn't get used for first window I believe

            var window = base.EnsureWindow(model, view, isDialog);

            var m = model as AppViewModel;
            
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
