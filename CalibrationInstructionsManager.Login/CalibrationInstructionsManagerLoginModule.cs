using CalibrationInstructionsManager.Login.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace CalibrationInstructionsManager.Login
{
    public class CalibrationInstructionsManagerLoginModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<DatabaseLoginView>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {

        }
    }
}
