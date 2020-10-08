using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Documents;
using CalibrationInstructionsManager.Core;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Events;
using CalibrationInstructionsManager.Core.Models.Parameters;
using CalibrationInstructionsManager.Core.Models.Templates;
using CalibrationInstructionsManager.Core.Models.Types;
using Prism.Commands;
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

        // private ObservableCollection<IChannelSettingType> _observableTypeCollection;
        // public ObservableCollection<IChannelSettingType> ObservableTypeCollection { get { return _observableTypeCollection; } set { SetProperty(ref _observableTypeCollection, value); } }


        #endregion // Properties

        public ChannelSettingsDetailViewModel(IPostgreSQLDatabase database, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _database = database;
            ObservableParameterCollection = new ObservableCollection<ChannelSettingParameters>();
            ObservableSelectedParameters = new ObservableCollection<ChannelSettingParameters>();
            GetParametersFromDatabase();
            // ObservableTypeCollection = new ObservableCollection<IChannelSettingType>();
            // GetTypesFromDatabase();
            eventAggregator.GetEvent<PassSelectedItemEvent>().Subscribe(OnItemReceived);
            eventAggregator.GetEvent<PassCreatedTemplateIdEvent>().Subscribe(OnIdReceived);
        }

        public int _id;

        private void OnIdReceived(int obj)
        {
            _id = obj;
        }

        // TODO: Fix dataset entries being populated to database X times
        private void OnItemReceived(ChannelSettingTemplate obj)
        {
            var selectedChannelSettingParameters = _database.GetSelectedChannelSettingParameters(obj.Id);
            var lastParameterId = _database.GetChannelSettingParameters().Last().ParameterId;
            var lastTemplateId = _database.GetChannelSettingTemplates().Last().Id;

            foreach (var item in selectedChannelSettingParameters)
            {
                var channelSettingParameters = new ChannelSettingParameters();

                channelSettingParameters.ParameterId = ++lastParameterId;
                channelSettingParameters.TemplateId = lastTemplateId;
                channelSettingParameters.DefaultValue = item.DefaultValue;
                channelSettingParameters.UncertaintyValue = item.UncertaintyValue;
                channelSettingParameters.ParameterIndex = item.ParameterIndex;
                channelSettingParameters.ParameterQuantity = item.ParameterQuantity;
                channelSettingParameters.TypeId = item.TypeId;

                _database.CopyExistingChannelSettingParameters(channelSettingParameters);
            }
            


            // var channelSettingParameters = new ChannelSettingParameters();
            //
            // _database.GetSelectedChannelSettingParameters(SelectedChannelSettingTemplate.Id);
            //
            // var selectedId = SelectedChannelSettingTemplate.Id;
            // Console.WriteLine(selectedId);
            //
            // _database.CopyExistingChannelSettingParameters(parameters);
            // get selected id
            // get parameters of selected id 
            // copy parameters of selected id to new created
            // if (_database.GetChannelSettingTemplates().Any(a => a.FullName == SelectedChannelSettingTemplate.FullName ))
            // {
            //     Console.WriteLine("juhu");
            // }


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

        // public ObservableCollection<IChannelSettingType> GetTypesFromDatabase()
        // {
        //     ObservableTypeCollection.Clear();
        //
        //     foreach (var item in _database.GetChannelSettingTypes())
        //     {
        //         ObservableTypeCollection.Add(item);
        //     }
        //
        //     return ObservableTypeCollection;
        // }

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
