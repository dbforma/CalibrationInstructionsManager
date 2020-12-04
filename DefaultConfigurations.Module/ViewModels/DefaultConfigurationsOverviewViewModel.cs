using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
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

        private ICollectionView _defaultConfigurationCollectionView;
        public ICollectionView DefaultConfigurationCollectionView { get { return _defaultConfigurationCollectionView; } set { SetProperty(ref _defaultConfigurationCollectionView, value); } }

        private IRegionManager _regionManager;
        private IPostgresql _database;
        private NavigationParameters _navigationParameter;
        private SelectionChangedEventArgs _selectionChangedEvent;
        public DelegateCommand<object> SelectedTemplateCommand { get; set; }

        /// <summary>
        /// Filter logic for search bar
        /// </summary>
        /// <_navigationParameter name="defaultConfiguration"></_navigationParameter>
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

        public DefaultConfigurationsOverviewViewModel(IPostgresql database, IRegionManager regionManager)
        {
            _database = database;
            _regionManager = regionManager;

            SelectedTemplateCommand = new DelegateCommand<object>(checkSelectedItem);
            DefaultConfigurationTemplates = new ObservableCollection<IDefaultConfigurationTemplate>(database.GetDefaultConfigurationTemplates());
        }

        #region Methods

        
        private void checkSelectedItem(object selectedItem)
        {
            // If SelectedItem is NewItemPlaceholder (new row in Datagrid), create a new object of DefaultConfigurationTemplate and add it to the Observable Collection
            if (selectedItem.ToString() == "{NewItemPlaceholder}")
            {
                _defaultConfigurationTemplates.Add(new DefaultConfigurationTemplate());
            }

            // TODO: Implement logic if selectedItem is typeof SelectionChangedEventArgs
            if (selectedItem.GetType() == typeof(SelectionChangedEventArgs))
            {
                _selectionChangedEvent = (SelectionChangedEventArgs)selectedItem;
                // BEGIN TEST
                var test = (DefaultConfigurationTemplate)_selectionChangedEvent.RemovedItems[0];
                Console.WriteLine(test.FullName);
                // END TEST
            }

            if (selectedItem.GetType() == typeof(DefaultConfigurationTemplate))
            {
                _navigationParameter = new NavigationParameters();
                _navigationParameter.Add("selectedTemplate", selectedItem);
                Navigate();
            }
        }

        /// <summary>
        /// Sending parameter "selectedItem" to target-view "DefaultConfigurationDetailView" and navigate to "DefaultConfigurationDetailsRegion" region
        /// </summary>
        /// <_navigationParameter name="selectedTemplate"></_navigationParameter>
        private void Navigate()
        {
            try
            {
                _regionManager.RequestNavigate("DefaultConfigurationDetailsRegion", "DefaultConfigurationsDetailView",
                    _navigationParameter);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Here is the logic defined what should happen if the regionManager navigates to ViewModel/ View
        /// </summary>
        /// <_navigationParameter name="navigationContext"></_navigationParameter>
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
