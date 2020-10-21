using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CalibrationInstructionsManager.Core;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Events;
using CalibrationInstructionsManager.Core.Models.Parameters;
using CalibrationInstructionsManager.Core.Models.Templates;
using Prism.Events;
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
        private ObservableCollection<ChannelSettingParameters> ObservableParameterCollection { get; }

        private IPostgreSQLDatabase _database;

        // private ChannelSettingParameters _channelSettingParameters;
        // public ChannelSettingParameters ChannelSettingParameters { get { return _channelSettingParameters; } set { SetProperty(ref _channelSettingParameters, value); } }

        #endregion // Properties

        public ChannelSettingsDetailViewModel(IPostgreSQLDatabase database, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _database = database;
            ObservableParameterCollection = new ObservableCollection<ChannelSettingParameters>();
            ObservableSelectedParameters = new ObservableCollection<ChannelSettingParameters>();
            GetParametersFromDatabase();
            //eventAggregator.GetEvent<PassSelectedItemEvent>().Subscribe(OnItemReceived);
            eventAggregator.GetEvent<TemplateCopiedEvent>().Subscribe(OnItemReceived);
        }

        // In Service verlagern und alles was dazu gehört
        private void OnItemReceived(Dictionary<string, int> dict)
        {
            ChannelSettingParameters _channelSettingParameters;

            int newId;
            dict.TryGetValue("newId", out newId);

            int oldId;
            dict.TryGetValue("oldId", out oldId);

            var selectedChannelSettingParameters = _database.GetSelectedChannelSettingParameters(oldId);

            

            // TODO: Fix dataset entries being populated to database X times 

            foreach (var item in selectedChannelSettingParameters)
            {
                _channelSettingParameters = new ChannelSettingParameters();

                _channelSettingParameters.TemplateId = newId;
                _channelSettingParameters.DefaultValue = item.DefaultValue;
                _channelSettingParameters.UncertaintyValue = item.UncertaintyValue;
                _channelSettingParameters.ParameterIndex = item.ParameterIndex;
                _channelSettingParameters.ParameterQuantity = item.ParameterQuantity;
                _channelSettingParameters.TypeId = item.TypeId;

                _database.CopyExistingChannelSettingParameters(_channelSettingParameters);

            }

        }

        #region Methods

        public ObservableCollection<ChannelSettingParameters> GetParametersFromDatabase()
        {
            ObservableParameterCollection.Clear();

            foreach (var item in _database.GetChannelSettingParameters())
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
