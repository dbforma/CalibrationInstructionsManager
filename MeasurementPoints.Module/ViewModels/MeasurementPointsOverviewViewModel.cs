using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using CalibrationInstructionsManager.Core;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Models.Templates;
using CalibrationInstructionsManager.Core.Extensions;
using Prism.Commands;
using Prism.Regions;

namespace MeasurementPoints.Module.ViewModels
{
    public class MeasurementPointsOverviewViewModel : ViewModelBase, INavigationAware
    {
        #region Properties & Commands
        private IRegionManager _regionManager;
        private IPostgreSQLDatabase _database;

        private ObservableCollection<IMeasurementPointTemplate> _measurementPointTemplates;
        public ObservableCollection<IMeasurementPointTemplate> MeasurementPointTemplates { get { return _measurementPointTemplates; } set { SetProperty(ref _measurementPointTemplates, value); } }

        private ICollectionView _measurementPointCollectionView;
        public ICollectionView MeasurementPointCollectionView { get { return _measurementPointCollectionView; } set { SetProperty(ref _measurementPointCollectionView, value); } }

        public DelegateCommand<object> SelectedTemplateCommand { get; set; }

        /// <summary>
        /// Filter logic for search bar
        /// </summary>
        /// <param name="defaultConfiguration"></param>
        /// <returns></returns>
        private string _userInputKeyword = string.Empty;
        public string UserInputKeyword { get { return _userInputKeyword; } set { SetProperty(ref _userInputKeyword, value); MeasurementPointCollectionView.Filter += Filter; } }

        private bool Filter(object measurementPoint)
        {
            IMeasurementPointTemplate measurementPointTemplate = measurementPoint as IMeasurementPointTemplate;

            if (!string.IsNullOrEmpty(UserInputKeyword))
            {
                return measurementPointTemplate.FullName.Contains(UserInputKeyword, StringComparison.OrdinalIgnoreCase) || measurementPointTemplate.Commentary.Contains(UserInputKeyword, StringComparison.OrdinalIgnoreCase);
            }
            return true;
        }

        #endregion // Properties & Commands

        public MeasurementPointsOverviewViewModel(IPostgreSQLDatabase database, IRegionManager regionManager)
        {
            _database = database;
            _regionManager = regionManager;
            
            SelectedTemplateCommand = new DelegateCommand<object>(TemplateSelected);
            MeasurementPointTemplates = new ObservableCollection<IMeasurementPointTemplate>(database.GetMeasurementPointTemplates().ToList());
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
                _measurementPointTemplates.Add(new MeasurementPointTemplate());
                return;
            }

            var parameters = new NavigationParameters();
            parameters.Add("selectedTemplate", selectedTemplate);

            if (selectedTemplate != null)
            {
                //TODO: Handle InvalidCastException in a more proper manner
                try
                {
                    _regionManager.RequestNavigate("MeasurementPointDetailsRegion", "MeasurementPointsDetailView",
                        parameters);
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
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            GetTemplatesFromDatabase();
            GroupByCommentary();
        }

        public ObservableCollection<IMeasurementPointTemplate> GetTemplatesFromDatabase()
        {
            MeasurementPointTemplates.Clear();

            foreach (var item in _database.GetMeasurementPointTemplates().ToList())
            {
                MeasurementPointTemplates.Add(item);
            }

            return MeasurementPointTemplates;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void GroupByCommentary()
        {
            MeasurementPointCollectionView = CollectionViewSource.GetDefaultView(MeasurementPointTemplates);
            var groupDescription = new PropertyGroupDescription("Commentary");

            MeasurementPointCollectionView.GroupDescriptions.Clear();

            MeasurementPointCollectionView.GroupDescriptions.Add(groupDescription);
        }

        #endregion // Methods
    }
}
