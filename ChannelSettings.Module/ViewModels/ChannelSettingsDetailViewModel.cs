using System;
using System.Collections.ObjectModel;
using System.Linq;
using CalibrationInstructionsManager.Core;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Models.Templates;
using CalibrationInstructionsManager.Core.Models.ValueTypes;
using Prism.Regions;

namespace ChannelSettings.Module.ViewModels
{
    public class ChannelSettingsDetailViewModel : ViewModelBase
    {
        #region Properties
        private IChannelSettingTemplate _selectedChannelSettingTemplate;
        public IChannelSettingTemplate SelectedChannelSettingTemplate { get { return _selectedChannelSettingTemplate; } set { SetProperty(ref _selectedChannelSettingTemplate, value); } }

        private ObservableCollection<ChannelSettingParameters> _observableSelectedParameters;
        public ObservableCollection<ChannelSettingParameters> ObservableSelectedParameters { get { return _observableSelectedParameters; } set { SetProperty(ref _observableSelectedParameters, value); } }

        private ObservableCollection<ChannelSettingParameters> _observableParameterCollection;
        public ObservableCollection<ChannelSettingParameters> ObservableParameterCollection { get { return _observableParameterCollection; } set { SetProperty(ref _observableParameterCollection, value); } }

        private IPostgreSQLDatabase _database;

        #endregion // Properties

        public ChannelSettingsDetailViewModel(IPostgreSQLDatabase database, IRegionManager regionManager)
        {
            _database = database;
            ObservableParameterCollection = new ObservableCollection<ChannelSettingParameters>();
            ObservableSelectedParameters = new ObservableCollection<ChannelSettingParameters>();
            GetParametersFromDatabase();
        }

        #region Methods

        public ObservableCollection<ChannelSettingParameters> GetParametersFromDatabase()
        {
            ObservableParameterCollection.Clear();

            foreach (var item in _database.GetChannelSettingParameters().ToList())
            {
                ObservableParameterCollection.Add(item);
            }
            return ObservableParameterCollection;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("selectedTemplate"))
            {
                //TODO: Handle InvalidCastException in a more proper manner
                try
                {
                    SelectedChannelSettingTemplate = (IChannelSettingTemplate)navigationContext.Parameters.GetValue<Object>("selectedTemplate");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }


                ObservableSelectedParameters.Clear();

                for (int i = 0; i < ObservableParameterCollection.Count; i++)
                {
                    if (SelectedChannelSettingTemplate.Id == ObservableParameterCollection[i].TemplateId)
                    {
                        ObservableSelectedParameters.Add(ObservableParameterCollection[i]);
                    }
                }

            }

            var channelSetting = navigationContext.Parameters["selectedTemplate"] as IChannelSettingTemplate;

            if (channelSetting != null)
            {
                SelectedChannelSettingTemplate = channelSetting;
            }
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var defaultConfiguration = navigationContext.Parameters["selectedTemplate"] as IChannelSettingTemplate;

            if (defaultConfiguration != null)
            {
                // Create new instance if Id does not match and parameter is not null
                return SelectedChannelSettingTemplate != null && SelectedChannelSettingTemplate.Id == defaultConfiguration.Id;
            }
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
        }

        #endregion // Methods
    }
}
