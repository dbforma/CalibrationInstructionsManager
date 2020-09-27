using MeasurementPoints.Module.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace MeasurementPoints.Module
{
    public class MeasurementPointsModule : IModule
    {
        public MeasurementPointsModule(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion("NavigationRegion", typeof(MeasurementPointsOverviewView));
        }
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MeasurementPointsDetailView>();
            containerRegistry.RegisterForNavigation<MeasurementPointsOverviewView>();
        }
    
        public void OnInitialized(IContainerProvider containerProvider)
        {
    
        }
    }
}
