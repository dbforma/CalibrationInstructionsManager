using System;
using System.Collections.ObjectModel;
using System.Linq;
using CalibrationInstructionsManager.Core;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Models.Templates;
using CalibrationInstructionsManager.Core.Models.ValueTypes;
using Prism.Regions;

namespace DefaultConfigurations.Module.ViewModels
{
    public class DefaultConfigurationsDetailViewModel : ViewModelBase
    {
        #region Properties
        private IDefaultConfigurationTemplate _selectedDefaultConfigurationTemplate;
        public IDefaultConfigurationTemplate SelectedDefaultConfigurationTemplate { get { return _selectedDefaultConfigurationTemplate; } set { SetProperty(ref _selectedDefaultConfigurationTemplate, value); } }

        private ObservableCollection<DefaultConfigurationValueType> _observableSelectedParameters;
        public ObservableCollection<DefaultConfigurationValueType> ObservableSelectedParameters { get { return _observableSelectedParameters; } set { SetProperty(ref _observableSelectedParameters, value); } }

        private ObservableCollection<DefaultConfigurationValueType> _observableParameterCollection;
        public ObservableCollection<DefaultConfigurationValueType> ObservableParameterCollection { get { return _observableParameterCollection; } set { SetProperty(ref _observableParameterCollection, value); } }

        private IPostgreSQLDatabase _database;

        #endregion // Properties

        public DefaultConfigurationsDetailViewModel(IPostgreSQLDatabase database, IRegionManager regionManager)
         {
            _database = database;
            ObservableParameterCollection = new ObservableCollection<DefaultConfigurationValueType>();
            ObservableSelectedParameters = new ObservableCollection<DefaultConfigurationValueType>();
            GetValuesAndTypesFromDatabase();
         }

        #region Methods

        public ObservableCollection<DefaultConfigurationValueType> GetValuesAndTypesFromDatabase()
        {
            ObservableParameterCollection.Clear();

            foreach (var item in _database.GetDefaultConfigurationValueTypeParameters().ToList())
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
                    SelectedDefaultConfigurationTemplate = (IDefaultConfigurationTemplate)navigationContext.Parameters.GetValue<Object>("selectedTemplate");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }
                
            
                ObservableSelectedParameters.Clear();
            
                for (int i = 0; i < ObservableParameterCollection.Count; i++)
                {
                    if (SelectedDefaultConfigurationTemplate.Id == ObservableParameterCollection[i].ParameterId)
                    {
                        ObservableSelectedParameters.Add(ObservableParameterCollection[i]);
                    }
                }
            
            }
            
            var defaultConfiguration = navigationContext.Parameters["selectedTemplate"] as IDefaultConfigurationTemplate;
            
            if (defaultConfiguration != null)
            {
                SelectedDefaultConfigurationTemplate = defaultConfiguration;
            }
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var defaultConfiguration = navigationContext.Parameters["selectedTemplate"] as IDefaultConfigurationTemplate;
        
            if (defaultConfiguration != null)
            {
                // Create new instance if Id does not match and parameter is not null
                return SelectedDefaultConfigurationTemplate != null && SelectedDefaultConfigurationTemplate.Id == defaultConfiguration.Id;
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

