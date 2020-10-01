using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using CalibrationInstructionsManager.Core;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Models.Templates;
using CalibrationInstructionsManager.Core.Extensions;
using Prism.Commands;
using Prism.Regions;

namespace DefaultConfigurations.Module.ViewModels
{
    public class DefaultConfigurationsOverviewViewModel : ViewModelBase
    {
        #region Properties & Commands

        private ObservableCollection<IDefaultConfigurationTemplate> _defaultConfigurationTemplates;

        public ObservableCollection<IDefaultConfigurationTemplate> DefaultConfigurationTemplates { get { return _defaultConfigurationTemplates; } set { SetProperty(ref _defaultConfigurationTemplates, value); } }

        public DelegateCommand<object> SelectedTemplateCommand { get; set; }

        private IRegionManager _regionManager;
        private IPostgreSQLDatabase _database;

        private ICollectionView _defaultConfigurationCollectionView;
        public ICollectionView DefaultConfigurationCollectionView { get { return _defaultConfigurationCollectionView; } set { SetProperty(ref _defaultConfigurationCollectionView, value); } }
        

        /// <summary>
        /// Filter logic for search bar
        /// </summary>
        /// <param name="defaultConfiguration"></param>
        /// <returns></returns>
        private string _userInputKeyword = string.Empty;
        public string UserInputKeyword { get { return _userInputKeyword; } set { SetProperty(ref _userInputKeyword, value); DefaultConfigurationCollectionView.Filter += Filter; } }

        private bool Filter(object defaultConfiguration)
        {
            IDefaultConfigurationTemplate defaultConfigurationTemplate = defaultConfiguration as IDefaultConfigurationTemplate;

            if (!string.IsNullOrEmpty(UserInputKeyword))
            {
                return defaultConfigurationTemplate.FullName.Contains(UserInputKeyword, StringComparison.OrdinalIgnoreCase) || defaultConfigurationTemplate.Commentary.Contains(UserInputKeyword, StringComparison.OrdinalIgnoreCase);
            }
            return true;
        }

        #endregion // Properties & Commands

        public DefaultConfigurationsOverviewViewModel(IPostgreSQLDatabase database, IRegionManager regionManager)
        {
            _database = database;
            _regionManager = regionManager;

            SelectedTemplateCommand = new DelegateCommand<object>(TemplateSelected);
            DefaultConfigurationTemplates = new ObservableCollection<IDefaultConfigurationTemplate>(database.GetDefaultConfigurationTemplates());
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
                //TODO: Handle InvalidCastException in a more proper manner
                try
                {
                    _regionManager.RequestNavigate("DefaultConfigurationDetailsRegion", "DefaultConfigurationsDetailView", parameters);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }
            }
        }

        /// <summary>
        /// Here is the logic defined what should happen if the regionManager navigates to ViewModel/ View
        /// </summary>
        /// <param name="navigationContext"></param>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            GetTemplatesFromDatabase();
            GroupByCommentary();
        }

        public ObservableCollection<IDefaultConfigurationTemplate> GetTemplatesFromDatabase()
        {
            DefaultConfigurationTemplates.Clear();

            foreach (var item in _database.GetDefaultConfigurationTemplates())
            {
                DefaultConfigurationTemplates.Add(item);
            }

            return DefaultConfigurationTemplates;
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void GroupByCommentary()
        {
            DefaultConfigurationCollectionView = CollectionViewSource.GetDefaultView(DefaultConfigurationTemplates);
            var groupDescription = new PropertyGroupDescription("Commentary");

            DefaultConfigurationCollectionView.GroupDescriptions.Clear();

            DefaultConfigurationCollectionView.GroupDescriptions.Add(groupDescription);
        }

        #endregion // Methods
    }

    
    
}
