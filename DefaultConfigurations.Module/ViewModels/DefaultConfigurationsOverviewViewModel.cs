using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using CalibrationInstructionsManager.Core;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Models.Templates;
using Prism.Commands;
using Prism.Regions;

namespace DefaultConfigurations.Module.ViewModels
{
    public class DefaultConfigurationsOverviewViewModel : ViewModelBase, INavigationAware
    {
        #region Properties & Commands

        private ObservableCollection<DefaultConfigurationTemplate> _defaultConfigurationTemplates;

        public ObservableCollection<DefaultConfigurationTemplate> DefaultConfigurationTemplates { get { return _defaultConfigurationTemplates; } set { SetProperty(ref _defaultConfigurationTemplates, value); } }

        public DelegateCommand<DefaultConfigurationTemplate> SelectedTemplateCommand { get; set; }
        public DelegateCommand<object> test { get; set; }

        private IRegionManager _regionManager;
        private IPostgreSQLDatabase _database;

        private ListCollectionView _defaultConfigurationGroups;
        public ListCollectionView DefaultConfigurationGroups { get { return _defaultConfigurationGroups; } set { SetProperty(ref _defaultConfigurationGroups, value); } }

        #endregion // Properties & Commands

        /// <summary>
        /// DelegateCommand<T> is a DefaultConfigurationTemplate, this is the parameter of the command
        /// </summary>
        /// <param name="database"></param>
        /// <param name="regionManager"></param>

        public DefaultConfigurationsOverviewViewModel(IPostgreSQLDatabase database, IRegionManager regionManager)
        {
            
            //SelectedTemplateCommand = new DelegateCommand<DefaultConfigurationTemplate>(TemplateSelected);
            test = new DelegateCommand<object>(TemplateSelected);

            _database = database;
            _regionManager = regionManager;

            DefaultConfigurationTemplates = new ObservableCollection<DefaultConfigurationTemplate>(database.GetDefaultConfigurationTemplates().ToList());

            
        }



        #region Methods
        private void TemplateSelected(object defaultConfigurationTemplate)
        {
            if (defaultConfigurationTemplate.ToString() == "{NewItemPlaceholder}")
            {
                _defaultConfigurationTemplates.Add(new DefaultConfigurationTemplate());
                return;
            }


            var parameters = new NavigationParameters();
            parameters.Add("defaultConfigurationTemplate", defaultConfigurationTemplate);

            if (defaultConfigurationTemplate != null)
            {
                _regionManager.RequestNavigate("DefaultConfigurationDetailsRegion", "DefaultConfigurationsDetailView",
                    parameters);
            }
        }

        /// <summary>
        /// Sending parameter "defaultConfigurationTemplate" to target-view "DefaultConfigurationDetailView" and navigate to "DefaultConfigurationDetailsRegion" region
        /// </summary>
        /// <param name="defaultConfigurationTemplate"></param>
        private void TemplateSelected(DefaultConfigurationTemplate defaultConfigurationTemplate)
        {
            if (defaultConfigurationTemplate == null)
                return;

            var parameters = new NavigationParameters();
            parameters.Add("defaultConfigurationTemplate", defaultConfigurationTemplate);

            if (defaultConfigurationTemplate != null)
            {
                _regionManager.RequestNavigate("DefaultConfigurationDetailsRegion", "DefaultConfigurationsDetailView",
                    parameters);
            }
        }

        /// <summary>
        /// Here is the logic defined what should happen if the regionManager navigates to ViewModel/ View
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            GetTemplatesFromDatabase();

            DefaultConfigurationGroups = new ListCollectionView(DefaultConfigurationTemplates.ToList());
            DefaultConfigurationGroups.GroupDescriptions.Add(new PropertyGroupDescription("Test"));
        }

        public ObservableCollection<DefaultConfigurationTemplate> GetTemplatesFromDatabase()
        {
            //TODO: Resolve InvalidCastException: SelectedChangedEventArgs =/= DefaultConfigurationTemplate
            DefaultConfigurationTemplates.Clear();

            foreach (var item in _database.GetDefaultConfigurationTemplates().ToList())
            {
                DefaultConfigurationTemplates.Add(item);
            }

            return DefaultConfigurationTemplates;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        

        #endregion // Methods
    }
}
