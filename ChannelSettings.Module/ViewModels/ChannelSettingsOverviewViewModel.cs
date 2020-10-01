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

namespace ChannelSettings.Module.ViewModels
{
    public class ChannelSettingsOverviewViewModel : ViewModelBase
    {
        #region Properties & Commands

        private ObservableCollection<IChannelSettingTemplate> _channelSettingTemplates;

        public ObservableCollection<IChannelSettingTemplate> ChannelSettingTemplates { get { return _channelSettingTemplates; } set { SetProperty(ref _channelSettingTemplates, value); } }

        public DelegateCommand<object> SelectedTemplateCommand { get; set; }

        private IRegionManager _regionManager;
        private IPostgreSQLDatabase _database;

        private ICollectionView _channelSettingCollectionView;
        public ICollectionView ChannelSettingCollectionView { get { return _channelSettingCollectionView; } set { SetProperty(ref _channelSettingCollectionView, value); } }


        /// <summary>
        /// Filter logic for search bar
        /// </summary>
        /// <param name="channelSetting"></param>
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

        public ChannelSettingsOverviewViewModel(IPostgreSQLDatabase database, IRegionManager regionManager)
        {
            _database = database;
            _regionManager = regionManager;

            SelectedTemplateCommand = new DelegateCommand<object>(TemplateSelected);
            ChannelSettingTemplates = new ObservableCollection<IChannelSettingTemplate>(database.GetChannelSettingTemplates());
        }

        #region Methods

        /// <summary>
        /// Sending parameter "selectedTemplate" to target-view "ChannelSettingDetailView" and navigate to "ChannelSettingDetailsRegion" region
        /// </summary>
        /// <param name="selectedTemplate"></param>
        private void TemplateSelected(object selectedTemplate)
        {
            // If SelectedItem is NewItemPlaceholder (new row in Datagrid), create a new object of ChannelSettingTemplate and add it to the Observable Collection
            if (selectedTemplate.ToString() == "{NewItemPlaceholder}")
            {
                _channelSettingTemplates.Add(new ChannelSettingTemplate());
                return;
            }


            var parameters = new NavigationParameters();
            parameters.Add("selectedTemplate", selectedTemplate);

            if (selectedTemplate != null)
            {
                //TODO: Handle InvalidCastException in a more proper manner
                try
                {
                    _regionManager.RequestNavigate("ChannelSettingDetailsRegion", "ChannelSettingsDetailView", parameters);
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
            GroupByName();
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

        public void GroupByName()
        {
            ChannelSettingCollectionView = CollectionViewSource.GetDefaultView(ChannelSettingTemplates);
            var groupDescription = new PropertyGroupDescription("FullName");

            ChannelSettingCollectionView.GroupDescriptions.Clear();

            ChannelSettingCollectionView.GroupDescriptions.Add(groupDescription);
        }

        #endregion // Methods
    }
}
