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

        public DelegateCommand<object> SelectedTemplateCommand { get; set; }

        private IRegionManager _regionManager;
        private IPostgreSQLDatabase _database;

        private ListCollectionView _defaultConfigurationGroups;
        public ListCollectionView DefaultConfigurationGroups { get { return _defaultConfigurationGroups; } set { SetProperty(ref _defaultConfigurationGroups, value); } }

        #endregion // Properties & Commands

        public DefaultConfigurationsOverviewViewModel(IPostgreSQLDatabase database, IRegionManager regionManager)
        {
            _database = database;
            _regionManager = regionManager;

            SelectedTemplateCommand = new DelegateCommand<object>(TemplateSelected);
            DefaultConfigurationTemplates = new ObservableCollection<DefaultConfigurationTemplate>(database.GetDefaultConfigurationTemplates().ToList());
        }

        #region Methods

        /// <summary>
        /// Sending parameter "selectedTemplate" to target-view "DefaultConfigurationDetailView" and navigate to "DefaultConfigurationDetailsRegion" region
        /// </summary>
        /// <param name="selectedTemplate"></param>
        private void TemplateSelected(object selectedTemplate)
        {
            // If SelectedItem is NewItemPlaceholder (new row in Datagrid), create a new object of DefaultConfigurationTemplate and add it to the Observable Collection
            if (selectedTemplate.ToString() == "{NewItemPlaceholder}")
            {
                _defaultConfigurationTemplates.Add(new DefaultConfigurationTemplate());
                return;
            }


            var parameters = new NavigationParameters();
            parameters.Add("selectedTemplate", selectedTemplate);

            if (selectedTemplate != null)
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
            GroupByCommentary();
        }

        public ObservableCollection<DefaultConfigurationTemplate> GetTemplatesFromDatabase()
        {
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

        public void GroupByCommentary()
        {
            DefaultConfigurationGroups = new ListCollectionView(DefaultConfigurationTemplates.ToList());
            DefaultConfigurationGroups.GroupDescriptions.Clear();
            DefaultConfigurationGroups.GroupDescriptions.Add(new PropertyGroupDescription("Commentary"));
        }

        #endregion // Methods
    }
}
