using System.Collections.ObjectModel;
using CalibrationInstructionsManager.Core;
using CalibrationInstructionsManager.Core.Models;
using Prism.Regions;

namespace MeasurementPoints.Module.ViewModels
{
    public class MeasurementPointsDetailViewModel : ViewModelBase
    {
        #region Properties

        private MeasurementPoint _selectedMeasurementPoint;
        public MeasurementPoint SelectedMeasurementPoint { get { return _selectedMeasurementPoint; } set { SetProperty(ref _selectedMeasurementPoint, value); } }

        private ObservableCollection<MeasurementPoint> _observableMeasurementPoint;
        public ObservableCollection<MeasurementPoint> ObservableMeasurementPoint { get { return _observableMeasurementPoint; } set { SetProperty(ref _observableMeasurementPoint, value); } }


        #endregion // Properties

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("measurementPoint"))
            {
                SelectedMeasurementPoint = navigationContext.Parameters.GetValue<MeasurementPoint>("measurementPoint");
            }
            var measurementPoint = navigationContext.Parameters["measurementPoint"] as MeasurementPoint;
            if (measurementPoint != null)
            {
                SelectedMeasurementPoint = measurementPoint;
            }
            ObservableMeasurementPoint = new ObservableCollection<MeasurementPoint>();
            ObservableMeasurementPoint.Add(SelectedMeasurementPoint);
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var measurementPoint = navigationContext.Parameters["measurementPoint"] as MeasurementPoint;
            if (measurementPoint != null)
            {
                // Create new instance if FullName does not match
                return SelectedMeasurementPoint != null && SelectedMeasurementPoint.FullName == measurementPoint.FullName;
            }
            else
            {
                return true;
            }
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
        }

        #endregion // INavigationAware

        #region NavigationCommand

        // public DelegateCommand<string> NavigateCommand { get; set; }
        //
        // public MeasurementPointDetailViewModel() 
        // {
        //     Title = "Measurement Point Detail ViewModel";
        //     NavigateCommand = new DelegateCommand<string>(Navigate);
        // }
        //
        // private void Navigate(string navigationPath)
        // {
        //    
        // }

        #endregion // NavigationCommand
    }
}

