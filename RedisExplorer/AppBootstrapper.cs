using System;
using System.Collections.Generic;
using System.Windows;

using Caliburn.Micro;

using RedisExplorer.Interface;

namespace RedisExplorer
{
    public class AppBootstrapper : BootstrapperBase
    {
        private SimpleContainer container;

        public AppBootstrapper() 
        {
            Initialize();
        }

        protected override void Configure()
        {
            container = new SimpleContainer();
            
            container.Singleton<IWindowManager, AppWindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.PerRequest<IApp, AppViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = container.GetInstance(service, key);
            if (instance != null)
                return instance;
            throw new InvalidOperationException("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return container.GetAllInstances(serviceType);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<IApp>(); 
        }
    }
}
