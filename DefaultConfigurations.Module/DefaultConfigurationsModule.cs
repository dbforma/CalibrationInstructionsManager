using DefaultConfigurations.Module.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace DefaultConfigurations.Module
{
    public class DefaultConfigurationsModule : IModule
    {
        public DefaultConfigurationsModule(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion("NavigationRegion", typeof(DefaultConfigurationsOverviewView));
        }
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<DefaultConfigurationsDetailView>();
            containerRegistry.RegisterForNavigation<DefaultConfigurationsOverviewView>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {

        }
    }
}
