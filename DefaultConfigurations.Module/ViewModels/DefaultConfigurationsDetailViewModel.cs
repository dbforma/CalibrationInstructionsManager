using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using CalibrationInstructionsManager.Core;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Models.Templates;
using CalibrationInstructionsManager.Core.Models.ValueTypes;
using DefaultConfigurations.Module.Views;
using Prism.Regions;

namespace DefaultConfigurations.Module.ViewModels
{
    public class DefaultConfigurationsDetailViewModel : ViewModelBase, INavigationAware
    {
        #region Properties
        private DefaultConfigurationTemplate _selectedDefaultConfigurationTemplate;
        public DefaultConfigurationTemplate SelectedDefaultConfigurationTemplate { get { return _selectedDefaultConfigurationTemplate; } set { SetProperty(ref _selectedDefaultConfigurationTemplate, value); } }

        private ObservableCollection<DefaultConfigurationValueType> _selectedValuesAndTypes;
        public ObservableCollection<DefaultConfigurationValueType> SelectedValuesAndTypes { get { return _selectedValuesAndTypes; } set { SetProperty(ref _selectedValuesAndTypes, value); } }

        private ObservableCollection<DefaultConfigurationValueType> _observableValuesAndTypes;
        public ObservableCollection<DefaultConfigurationValueType> ObservableValuesAndTypes { get { return _observableValuesAndTypes; } set { SetProperty(ref _observableValuesAndTypes, value); } }

        //TODO: Try ICollectionView to solve InvalidCastException or Eventaggregator and pass selected object from ViewModel to Detail-ViewModel
        private ICollectionView _valuesTypesCollection;

        public ICollectionView ValuesTypesCollection { get { return _valuesTypesCollection; } set { SetProperty(ref _valuesTypesCollection, value); } }

        private IPostgreSQLDatabase _database;

        #endregion // Properties

        public DefaultConfigurationsDetailViewModel(IPostgreSQLDatabase database, IRegionManager regionManager)
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

        public new void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("selectedTemplate"))
            {
                //TODO: Handle InvalidCastException
                SelectedDefaultConfigurationTemplate = navigationContext.Parameters.GetValue<DefaultConfigurationTemplate>("selectedTemplate");
     
                SelectedValuesAndTypes.Clear();

                for (int i = 0; i < ObservableValuesAndTypes.Count; i++)
                {
                    if (SelectedDefaultConfigurationTemplate.Id == ObservableValuesAndTypes[i].ParameterId)
                    {
                        SelectedValuesAndTypes.Add(ObservableValuesAndTypes[i]);
                    }
                }

            }

            var defaultConfiguration = navigationContext.Parameters["selectedTemplate"] as DefaultConfigurationTemplate;

            if (defaultConfiguration != null)
            {
                SelectedDefaultConfigurationTemplate = defaultConfiguration;
            }
        }

        public new bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var defaultConfiguration = navigationContext.Parameters["selectedTemplate"] as DefaultConfigurationTemplate;
        
            if (defaultConfiguration != null)
            {
                // Create new instance if FullName does not match
                return SelectedDefaultConfigurationTemplate != null && SelectedDefaultConfigurationTemplate.FullName.ToLower() == defaultConfiguration.FullName.ToLower();
            }
            else
            {
                return true;
            }
        }
        
        public new void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
            }

        #endregion // Methods
    }
}

