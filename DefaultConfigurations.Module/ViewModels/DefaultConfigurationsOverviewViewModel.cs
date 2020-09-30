using System;
using System.Collections;
using System.Collections.Generic;
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

        private ICollectionView _defaultConfigurationCollection;
        public ICollectionView DefaultConfigurationCollection { get { return _defaultConfigurationCollection; } set { SetProperty(ref _defaultConfigurationCollection, value); } }
        

        /// <summary>
        /// Filter logic for search bar
        /// </summary>
        /// <param name="defaultConfiguration"></param>
        /// <returns></returns>
        private string _fullNameFilter = string.Empty;
        public string FullNameFilter { get { return _fullNameFilter; } set { SetProperty(ref _fullNameFilter, value); DefaultConfigurationCollection.Filter += Filter; } }

        private bool Filter(object defaultConfiguration)

        {
            DefaultConfigurationTemplate defaultConfigurationTemplate = defaultConfiguration as DefaultConfigurationTemplate;
            if (!string.IsNullOrEmpty(FullNameFilter))
            {
                return defaultConfigurationTemplate.FullName.Contains(FullNameFilter) ||
                       defaultConfigurationTemplate.Commentary.Contains(FullNameFilter);
            }
            else return true;
        }


        //
        // private ListCollectionView _defaultConfigurationCollection;
        // public ListCollectionView DefaultConfigurationCollection { get { return _defaultConfigurationCollection; } set { SetProperty(ref _defaultConfigurationCollection, value); } }

        #endregion // Properties & Commands

        public DefaultConfigurationsOverviewViewModel(IPostgreSQLDatabase database, IRegionManager regionManager)
        {
            _database = database;
            _regionManager = regionManager;

            SelectedTemplateCommand = new DelegateCommand<object>(TemplateSelected);
            DefaultConfigurationTemplates = new ObservableCollection<DefaultConfigurationTemplate>(database.GetDefaultConfigurationTemplates());
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
                try
                {
                    _regionManager.RequestNavigate("DefaultConfigurationDetailsRegion",
                        "DefaultConfigurationsDetailView",
                        parameters);
                }
                catch
                {
                    return;
                }
            }
        }


        /// <summary>
        /// Here is the logic defined what should happen if the regionManager navigates to ViewModel/ View
        /// </summary>
        /// <param name="navigationContext"></param>
        public new void OnNavigatedTo(NavigationContext navigationContext)
        {
            GetTemplatesFromDatabase();
            GroupByCommentary();
        }

        public ObservableCollection<DefaultConfigurationTemplate> GetTemplatesFromDatabase()
        {
            DefaultConfigurationTemplates.Clear();

            foreach (var item in _database.GetDefaultConfigurationTemplates())
            {
                DefaultConfigurationTemplates.Add(item);
            }

            return DefaultConfigurationTemplates;
        }

        public new bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void GroupByCommentary()
        {
            // DefaultConfigurationCollection = new ListCollectionView(DefaultConfigurationTemplates);
            DefaultConfigurationCollection = CollectionViewSource.GetDefaultView(DefaultConfigurationTemplates);
            var groupDescription = new PropertyGroupDescription("Commentary");

            DefaultConfigurationCollection.GroupDescriptions.Clear();

            DefaultConfigurationCollection.GroupDescriptions.Add(groupDescription);
        }

        #endregion // Methods
    }
}
