using System.Windows;
using System.Windows.Controls;
using CalibrationInstructionsManager.Core.Commands;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Dialogs.ViewModels;
using CalibrationInstructionsManager.Core.Dialogs.Views;
using CalibrationInstructionsManager.Core.Models.Parameters;
using CalibrationInstructionsManager.Core.Models.Parameters.Contract;
using CalibrationInstructionsManager.Core.Models.Templates;
using CalibrationInstructionsManager.Core.Regions;
using ChannelSettings.Module.Service;
using MeasurementPointsCopyService;
//using ChannelSettings.Module.Service;
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
            containerRegistry.RegisterSingleton<IPostgresql, Postgresql>();
            containerRegistry.RegisterSingleton<IChannelSettingTemplate, ChannelSettingTemplate>();
            containerRegistry.RegisterSingleton<IMeasurementPointTemplate, MeasurementPointTemplate>();
            containerRegistry.RegisterSingleton<IDefaultConfigurationTemplate, DefaultConfigurationTemplate>();
            containerRegistry.RegisterSingleton<IChannelSettingParameters, IChannelSettingParameters>();
            containerRegistry.RegisterSingleton<IMeasurementPointParameters, MeasurementPointParameters>();
            containerRegistry.RegisterSingleton<IChannelSettingsOverviewService, ChannelSettingsOverviewService>();
            containerRegistry.RegisterSingleton<IChannelSettingsDetailService, ChannelSettingsDetailService>();
            containerRegistry.RegisterSingleton<IMeasurementPointsOverviewService, MeasurementPointsOverviewService>();
            containerRegistry.RegisterSingleton<IMeasurementPointsDetailService, MeasurementPointsDetailService>();
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
