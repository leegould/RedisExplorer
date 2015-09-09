using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            
            window.Title = "RESTLess";
            //window.Icon = new BitmapImage(new Uri("pack://application:,,,/RedisExplorer;component/Assets/Images/arrows.ico"));

            return window;
        }
    }
}
