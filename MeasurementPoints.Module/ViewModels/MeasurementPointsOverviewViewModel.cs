using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
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
        private IPostgresql _database;
        private NavigationParameters _navigationParameter;
        private SelectionChangedEventArgs _selectionChangedEvent;

        private ObservableCollection<IMeasurementPointTemplate> _measurementPointTemplates;
        public ObservableCollection<IMeasurementPointTemplate> MeasurementPointTemplates { get { return _measurementPointTemplates; } set { SetProperty(ref _measurementPointTemplates, value); } }

        private ICollectionView _measurementPointCollectionView;
        public ICollectionView MeasurementPointCollectionView { get { return _measurementPointCollectionView; } set { SetProperty(ref _measurementPointCollectionView, value); } }

        public DelegateCommand<object> SelectedTemplateCommand { get; set; }

        /// <summary>
        /// Filter logic for search bar
        /// </summary>
        /// <_navigationParameter name="defaultConfiguration"></_navigationParameter>
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

        public MeasurementPointsOverviewViewModel(IPostgresql database, IRegionManager regionManager)
        {
            _database = database;
            _regionManager = regionManager;
            
            SelectedTemplateCommand = new DelegateCommand<object>(checkSelectedItem);
            MeasurementPointTemplates = new ObservableCollection<IMeasurementPointTemplate>(database.GetMeasurementPointTemplates().ToList());
        }

        #region Methods

        private void checkSelectedItem(object selectedItem)
        {
            // If SelectedItem is NewItemPlaceholder (new row in Datagrid), create a new object of MeasurementPointTemplate and add it to the Observable Collection
            if (selectedItem.ToString() == "{NewItemPlaceholder}")
            {
                _measurementPointTemplates.Add(new MeasurementPointTemplate());
            }

            // TODO: Implement logic if selectedItem is typeof SelectionChangedEventArgs
            if (selectedItem.GetType() == typeof(SelectionChangedEventArgs))
            {
                _selectionChangedEvent = (SelectionChangedEventArgs)selectedItem;
                // BEGIN TEST
                var test = (MeasurementPointTemplate)_selectionChangedEvent.RemovedItems[0]; 
                Console.WriteLine(test.FullName);
                // END TEST
            }

            if (selectedItem.GetType() == typeof(MeasurementPointTemplate))
            {
                _navigationParameter = new NavigationParameters();
                _navigationParameter.Add("selectedTemplate", selectedItem);
                Navigate();
            }
        }


        /// <summary>
        /// Sending parameter "selectedItem" to target-view "MeasurementPointsDetailView" and navigate to "MeasurementPointDetailsRegion" region
        /// </summary>
        /// <_navigationParameter name="selectedTemplate"></_navigationParameter>
        private void Navigate()
        {
            try
            {
                _regionManager.RequestNavigate("MeasurementPointDetailsRegion", "MeasurementPointsDetailView",
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
