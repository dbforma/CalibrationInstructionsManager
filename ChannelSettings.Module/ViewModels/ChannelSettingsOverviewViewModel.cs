using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using CalibrationInstructionsManager.Core;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Events;
using CalibrationInstructionsManager.Core.Models.Templates;
using CalibrationInstructionsManager.Core.Extensions;
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
        public DelegateCommand<ChannelSettingTemplate> AddCopiedItemCommand { get; }

        private readonly IEventAggregator _eventAggregator;

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

            SelectedTemplateCommand = new DelegateCommand<object>(checkSelectedItem);
            ChannelSettingTemplates = new ObservableCollection<IChannelSettingTemplate>(database.GetChannelSettingTemplates());

            AddCopiedItemCommand = new DelegateCommand<ChannelSettingTemplate>(CopySelectedItemCreateNewDataset);

            _eventAggregator = eventAggregator;
        }

        private void CopySelectedItemCreateNewDataset(ChannelSettingTemplate selectedItem)
        {
            var lastId = _channelSettingTemplates.Last().Id;

            var channelSetting = new ChannelSettingTemplate();
            _channelSettingTemplates.Add(channelSetting);

            if (channelSetting.Id == null || channelSetting.Id == 0)
            {
                channelSetting.Id = ++lastId;
                channelSetting.FullName = selectedItem.FullName;
            }

            _database.CopyExistingChannelSettingTemplate(channelSetting);

            _eventAggregator.GetEvent<PassSelectedItemEvent>().Publish(selectedItem);
            _eventAggregator.GetEvent<PassCreatedTemplateIdEvent>().Publish(channelSetting.Id);

            SetChannelSettingCollectionView();

        }

        #region Methods

        private void checkSelectedItem(object selectedItem)
        {
            // If SelectedItem is NewItemPlaceholder (new row in Datagrid), create a new object of ChannelSettingTemplate and add it to the Observable Collection
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
            GetTemplatesFromDatabase();
            SetChannelSettingCollectionView();
        }

        public ObservableCollection<IChannelSettingTemplate> GetTemplatesFromDatabase()
        {
            ChannelSettingTemplates.Clear();

            foreach (var item in _database.GetChannelSettingTemplates())
            {
                ChannelSettingTemplates.Add(item);
            }

            return ChannelSettingTemplates;
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        private void SetChannelSettingCollectionView()
        {
            ChannelSettingCollectionView = CollectionViewSource.GetDefaultView(_channelSettingTemplates);
            var groupDescription = new PropertyGroupDescription("FullName");

            ChannelSettingCollectionView.GroupDescriptions.Clear();

            ChannelSettingCollectionView.GroupDescriptions.Add(groupDescription);
        }

        #endregion // Methods
    }
}
