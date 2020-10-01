using System.Windows;
using System.Windows.Controls;
using CalibrationInstructionsManager.Core.Commands;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Dialogs.ViewModels;
using CalibrationInstructionsManager.Core.Dialogs.Views;
using CalibrationInstructionsManager.Core.Models.Templates;
using CalibrationInstructionsManager.Core.Regions;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using DefaultConfigurationTemplate = CalibrationInstructionsManager.Core.Models.Templates.DefaultConfigurationTemplate;

namespace CalibrationInstructionsManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();
            containerRegistry.RegisterDialog<MessageDialogView, MessageDialogViewModel>();
            containerRegistry.RegisterSingleton<IPostgreSQLDatabase, PostgreSQLDatabase>();
            containerRegistry.Register<IChannelSettingTemplate, ChannelSettingTemplate>();
            containerRegistry.Register<IMeasurementPointTemplate, MeasurementPointTemplate>();
            containerRegistry.Register<IDefaultConfigurationTemplate, DefaultConfigurationTemplate>();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<Views.CalibrationInstructionsManagerShellView>();
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.RegisterMapping(typeof(StackPanel), Container.Resolve<StackPanelRegionAdapter>());
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new DirectoryModuleCatalog() { ModulePath = @".\Modules" };
        }
    }
}
