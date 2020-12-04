using System;
using System.Collections.ObjectModel;
using CalibrationInstructionsManager.Core;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Models.Parameters.Contract;
using CalibrationInstructionsManager.Core.Models.Templates;
using ChannelSettings.Module.Service;
using Prism.Regions;

namespace ChannelSettings.Module.ViewModels
{
    public class ChannelSettingsDetailViewModel : ViewModelBase
    {
        #region Properties
        private IChannelSettingTemplate _selectedChannelSettingTemplate;
        public IChannelSettingTemplate SelectedChannelSettingTemplate { get { return _selectedChannelSettingTemplate; } set { SetProperty(ref _selectedChannelSettingTemplate, value); } }

        private ObservableCollection<IChannelSettingParameters> _observableSelectedParameters;
        public ObservableCollection<IChannelSettingParameters> ObservableSelectedParameters { get { return _observableSelectedParameters; } set { SetProperty(ref _observableSelectedParameters, value); } }

        private ObservableCollection<IChannelSettingParameters> _observableParameterCollection;
        public ObservableCollection<IChannelSettingParameters> ObservableParameterCollection { get { return _observableParameterCollection; } set { SetProperty(ref _observableParameterCollection, value); } }

        private readonly IPostgresql _database;

        #endregion // Properties

        public ChannelSettingsDetailViewModel(IPostgresql database, IRegionManager regionManager)
        {
            _database = database;
            _observableParameterCollection = new ObservableCollection<IChannelSettingParameters>(_database.GetChannelSettingParameters());
            ObservableSelectedParameters = new ObservableCollection<IChannelSettingParameters>();
            PopulateObservableCollection();
        }

        #region Methods

        /// <summary>
        /// Responsible for populating the Observable Collection based on database items and therefor what's displayed in the DataGrid
        /// </summary>

        private void PopulateObservableCollection()
        {
            //TODO: 
            IChannelSettingsDetailService detailService = new ChannelSettingsDetailService(_database);
            detailService.GetParameters(_observableParameterCollection);
        }

        /// <summary>
        /// Here is the logic defined what should happen if the regionManager navigates to/ from ViewModel/ View
        /// </summary>
        /// <param name="navigationContext"></param>
        
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


                /// <summary>
                /// Populate Parameter Collection depending on selected ChannelSettingsTemplate item
                /// </summary>
                for (int i = 0; i < _observableParameterCollection.Count; i++)
                {
                    if (SelectedChannelSettingTemplate.Id == _observableParameterCollection[i].TemplateId)
                    {
                        ObservableSelectedParameters.Add(_observableParameterCollection[i]);
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
