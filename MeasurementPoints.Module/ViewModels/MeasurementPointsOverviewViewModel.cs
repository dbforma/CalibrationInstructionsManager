using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using CalibrationInstructionsManager.Core;
using CalibrationInstructionsManager.Core.Business;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Models;
using CalibrationInstructionsManager.Core.Models.Templates;
using Prism.Commands;
using Prism.Regions;

namespace MeasurementPoints.Module.ViewModels
{
    public class MeasurementPointsOverviewViewModel : ViewModelBase, INavigationAware
    {
        #region Properties & Commands

        private ObservableCollection<MeasurementPointTemplate> _measurementPointTemplates;

        public ObservableCollection<MeasurementPointTemplate> MeasurementPointTemplates { get { return _measurementPointTemplates; } set { SetProperty(ref _measurementPointTemplates, value); } }

        public DelegateCommand<object> SelectedTemplateCommand { get; set; }

        private IRegionManager _regionManager;
        private IPostgreSQLDatabase _database;

        private ListCollectionView _measurementPointGroups;
        public ListCollectionView MeasurementPointGroups { get { return _measurementPointGroups; } set { SetProperty(ref _measurementPointGroups, value); } }

        #endregion // Properties & Commands

        public MeasurementPointsOverviewViewModel(IPostgreSQLDatabase database, IRegionManager regionManager)
        {
            _database = database;
            _regionManager = regionManager;
            
            SelectedTemplateCommand = new DelegateCommand<object>(TemplateSelected);
            MeasurementPointTemplates = new ObservableCollection<MeasurementPointTemplate>(database.GetMeasurementPointTemplates().ToList());
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
                _regionManager.RequestNavigate("MeasurementPointDetailsRegion", "MeasurementPointsDetailView",
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

        public ObservableCollection<MeasurementPointTemplate> GetTemplatesFromDatabase()
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
            MeasurementPointGroups = new ListCollectionView(MeasurementPointTemplates);
            var groupDescription = new PropertyGroupDescription("Commentary");

            MeasurementPointGroups.GroupDescriptions.Clear();

            MeasurementPointGroups.GroupDescriptions.Add(groupDescription);
        }

        #endregion // Methods
    }
}
