using ChannelSettings.Module.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ChannelSettings.Module
{
    public class ChannelSettingsModule : IModule
    {
        public ChannelSettingsModule(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion("NavigationRegion", typeof(ChannelSettingsOverviewView));
        }
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ChannelSettingsDetailView>();
            containerRegistry.RegisterForNavigation<ChannelSettingsOverviewView>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {

        }
    }
}
