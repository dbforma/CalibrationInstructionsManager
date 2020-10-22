using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using CalibrationInstructionsManager.Core;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Models.Templates;
using CalibrationInstructionsManager.Core.Extensions;
using ChannelSettings.Module.Service;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace ChannelSettings.Module.ViewModels
{
    public class ChannelSettingsOverviewViewModel : ViewModelBase
    {
        #region Properties & Commands

        private ObservableCollection<IChannelSettingTemplate> _channelSettingTemplates;
        public ObservableCollection<IChannelSettingTemplate> ChannelSettingTemplates { get { return _channelSettingTemplates; } set { SetProperty(ref _channelSettingTemplates, value); } }

        private ICollectionView _channelSettingCollectionView;
        public ICollectionView ChannelSettingCollectionView { get { return _channelSettingCollectionView; } set { SetProperty(ref _channelSettingCollectionView, value); } }

        private IRegionManager _regionManager;
        private IPostgreSQLDatabase _database;
        private NavigationParameters _navigationParameter;
        private SelectionChangedEventArgs _selectionChangedEvent;

        public DelegateCommand<object> SelectedTemplateCommand { get; set; }
        public DelegateCommand<ChannelSettingTemplate> PassItemCommand { get; }

        private readonly IEventAggregator _eventAggregator;

        private IChannelSettingsOverviewService _overviewService;

        /// <summary>
        /// Filter logic for search bar
        /// </summary>
        /// <_navigationParameter name="channelSetting"></_navigationParameter>
        /// <returns></returns>
        private string _userInputKeyword = string.Empty;
        public string UserInputKeyword { get { return _userInputKeyword; } set { SetProperty(ref _userInputKeyword, value); ChannelSettingCollectionView.Filter += Filter; } }

        private bool Filter(object channelSetting)
        {
            IChannelSettingTemplate channelSettingTemplate = channelSetting as IChannelSettingTemplate;

            if (!string.IsNullOrEmpty(UserInputKeyword))
            {
                return channelSettingTemplate.FullName.Contains(UserInputKeyword, StringComparison.OrdinalIgnoreCase);
            }
            return true;
        }

        #endregion // Properties & Commands

        public ChannelSettingsOverviewViewModel(IPostgreSQLDatabase database, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _database = database;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _overviewService = new ChannelSettingsOverviewService(_database); //TODO: Erzeugungsabhängigkeit auflösen

            ChannelSettingTemplates = new ObservableCollection<IChannelSettingTemplate>(database.GetChannelSettingTemplates());

            SelectedTemplateCommand = new DelegateCommand<object>(checkSelectedItem);
            PassItemCommand = new DelegateCommand<ChannelSettingTemplate>(PassSelectedItemToService);
        }

        #region Methods

        /// <summary>
        /// Passes selectedItem to Service. This function gets enabled when the "C O P Y" button is clicked
        /// </summary>
        /// <param name="selectedItem"></param>
        private void PassSelectedItemToService(ChannelSettingTemplate selectedItem)
        {
            //var overviewService = new ChannelSettingsOverviewService(_database); // database für collection view zeugs service ausgliedern
            _overviewService.CopyTemplate(selectedItem);
            PopulateObservableCollection();
        }

        /// <summary>
        /// If SelectedItem is NewItemPlaceholder (new row in Datagrid), create a new object of ChannelSettingTemplate and add it to the Observable Collection
        /// </summary>
        /// <param name="selectedItem"></param>
        private void checkSelectedItem(object selectedItem)
        {
            if (selectedItem.ToString() == "{NewItemPlaceholder}")
            {
                _channelSettingTemplates.Add(new ChannelSettingTemplate());
            }

            // TODO: Implement logic if selectedItem is typeof SelectionChangedEventArgs
            if (selectedItem.GetType() == typeof(SelectionChangedEventArgs))
            {
                _selectionChangedEvent = (SelectionChangedEventArgs)selectedItem;
                // BEGIN TEST
                var test = (ChannelSettingTemplate)_selectionChangedEvent.RemovedItems[0];
                Console.WriteLine(test.FullName);
                // END TEST
            }

            if (selectedItem.GetType() == typeof(ChannelSettingTemplate))
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
                _regionManager.RequestNavigate("ChannelSettingDetailsRegion", "ChannelSettingsDetailView", _navigationParameter);
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
            PopulateObservableCollection();
            PopulateCollectionView();
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <summary>
        /// ICollectionView is being used to enable grouping, filtering
        /// </summary>
        private void PopulateCollectionView()
        {
            ChannelSettingCollectionView = CollectionViewSource.GetDefaultView(_channelSettingTemplates);
            var groupDescription = new PropertyGroupDescription("FullName");
            ChannelSettingCollectionView.GroupDescriptions.Clear();
            ChannelSettingCollectionView.GroupDescriptions.Add(groupDescription);
        }

        /// <summary>
        /// Populates the Observable Collection
        /// </summary>
        private void PopulateObservableCollection()
        {
            _overviewService.GetTemplates(ChannelSettingTemplates);
        }

        #endregion // Methods
    }
}