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
        private DefaultConfigurationTemplate _selectedDefaultConfigurationTemplate;
        public DefaultConfigurationTemplate SelectedDefaultConfigurationTemplate { get { return _selectedDefaultConfigurationTemplate; } set { SetProperty(ref _selectedDefaultConfigurationTemplate, value); } }

        private ObservableCollection<DefaultConfigurationValueType> _selectedValuesAndTypes;
        public ObservableCollection<DefaultConfigurationValueType> SelectedValuesAndTypes { get { return _selectedValuesAndTypes; } set { SetProperty(ref _selectedValuesAndTypes, value); } }

        private ObservableCollection<DefaultConfigurationValueType> _observableValuesAndTypes;
        public ObservableCollection<DefaultConfigurationValueType> ObservableValuesAndTypes { get { return _observableValuesAndTypes; } set { SetProperty(ref _observableValuesAndTypes, value); } }

        private IPostgreSQLDatabase _database;

        #endregion // Properties

        public DefaultConfigurationsDetailViewModel(IPostgreSQLDatabase database)
         {
            _database = database;
            ObservableValuesAndTypes = new ObservableCollection<DefaultConfigurationValueType>();
            SelectedValuesAndTypes = new ObservableCollection<DefaultConfigurationValueType>();
            GetValuesAndTypesFromDatabase();
        }

        #region Methods

        public ObservableCollection<DefaultConfigurationValueType> GetValuesAndTypesFromDatabase()
        {
            ObservableValuesAndTypes.Clear();

            foreach (var item in _database.GetDefaultConfigurationValueTypeParameters().ToList())
            {
                ObservableValuesAndTypes.Add(item);
            }

            return ObservableValuesAndTypes;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("defaultConfigurationTemplate"))
            {
                SelectedDefaultConfigurationTemplate = navigationContext.Parameters.GetValue<DefaultConfigurationTemplate>("defaultConfigurationTemplate");

                SelectedValuesAndTypes.Clear();

                for (int i = 0; i < ObservableValuesAndTypes.Count; i++)
                {
                    if (SelectedDefaultConfigurationTemplate.Id == ObservableValuesAndTypes[i].ParameterId)
                    {
                        SelectedValuesAndTypes.Add(ObservableValuesAndTypes[i]);
                    }
                }

            }

            var defaultConfiguration = navigationContext.Parameters["defaultConfigurationTemplate"] as DefaultConfigurationTemplate;

            if (defaultConfiguration != null)
            {
                SelectedDefaultConfigurationTemplate = defaultConfiguration;
            }
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var defaultConfiguration = navigationContext.Parameters["defaultConfigurationTemplate"] as DefaultConfigurationTemplate;
        
            if (defaultConfiguration != null)
            {
                // Create new instance if FullName does not match
                return SelectedDefaultConfigurationTemplate != null && SelectedDefaultConfigurationTemplate.FullName == defaultConfiguration.FullName;
            }
            else
            {
                return true;
            }
        }
        
        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
        
        }

        #endregion // Methods
    }
}

