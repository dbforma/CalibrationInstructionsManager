using System;
using System.Collections.ObjectModel;
using System.Linq;
using CalibrationInstructionsManager.Core;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Models.Parameters;
using CalibrationInstructionsManager.Core.Models.Templates;
using Prism.Regions;

namespace MeasurementPoints.Module.ViewModels
{
    public class MeasurementPointsDetailViewModel : ViewModelBase
    {
        #region Properties
        private IMeasurementPointTemplate _selectedMeasurementPointTemplate;
        public IMeasurementPointTemplate SelectedMeasurementPointTemplate { get { return _selectedMeasurementPointTemplate; } set { SetProperty(ref _selectedMeasurementPointTemplate, value); } }

        private ObservableCollection<MeasurementPointParameters> _observableSelectedParameters;
        public ObservableCollection<MeasurementPointParameters> ObservableSelectedParameters { get { return _observableSelectedParameters; } set { SetProperty(ref _observableSelectedParameters, value); } }

        private ObservableCollection<MeasurementPointParameters> _observableParameterCollection;
        public ObservableCollection<MeasurementPointParameters> ObservableParameterCollection { get { return _observableParameterCollection; } set { SetProperty(ref _observableParameterCollection, value); } }

        private IPostgresql _database;

        #endregion // Properties

        public MeasurementPointsDetailViewModel(IPostgresql database)
        {
            _database = database;
            ObservableParameterCollection = new ObservableCollection<MeasurementPointParameters>();
            ObservableSelectedParameters = new ObservableCollection<MeasurementPointParameters>();
            GetValuesAndTypesFromDatabase();
        }

        #region Methods

        public ObservableCollection<MeasurementPointParameters> GetValuesAndTypesFromDatabase()
        {
            ObservableParameterCollection.Clear();

            foreach (var item in _database.GetMeasurementPointValueTypeParameters().ToList())
            {
                ObservableParameterCollection.Add(item);
            }

            return ObservableParameterCollection;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("selectedTemplate"))
            {
                //TODO: Handle InvalidCastException in a more proper manner
                try
                {
                    SelectedMeasurementPointTemplate = navigationContext.Parameters.GetValue<IMeasurementPointTemplate>("selectedTemplate");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }
                

                ObservableSelectedParameters.Clear();

                for (int i = 0; i < ObservableParameterCollection.Count; i++)
                {
                    if (SelectedMeasurementPointTemplate.Id == ObservableParameterCollection[i].TemplateId)
                    {
                        ObservableSelectedParameters.Add(ObservableParameterCollection[i]);
                    }
                }

            }

            var measurementPoint = navigationContext.Parameters["selectedTemplate"] as IMeasurementPointTemplate;

            if (measurementPoint != null)
            {
                SelectedMeasurementPointTemplate = measurementPoint;
            }
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var measurementPoint = navigationContext.Parameters["selectedTemplate"] as IMeasurementPointTemplate;

            if (measurementPoint != null)
            {
                // Create new instance if Id does not match and parameter is not null
                return SelectedMeasurementPointTemplate != null && SelectedMeasurementPointTemplate.Id == measurementPoint.Id;
            }

            return true;
            
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
        }

        #endregion // Methods
    }
}





//
//
//
//
// using System;
// using System.Collections.ObjectModel;
// using System.Linq;
// using CalibrationInstructionsManager.Core;
// using CalibrationInstructionsManager.Core.Data;
// using CalibrationInstructionsManager.Core.Models.Parameters;
// using CalibrationInstructionsManager.Core.Models.Templates;
// using MeasurementPointsCopyService;
// using Prism.Regions;
//
// namespace MeasurementPoints.Module.ViewModels
// {
//     public class MeasurementPointsDetailViewModel : ViewModelBase
//     {
//         #region Properties
//         private IMeasurementPointTemplate _selectedMeasurementPointTemplate;
//         public IMeasurementPointTemplate SelectedMeasurementPointTemplate { get { return _selectedMeasurementPointTemplate; } set { SetProperty(ref _selectedMeasurementPointTemplate, value); } }
//
//         private ObservableCollection<IMeasurementPointParameters> _observableSelectedParameters;
//         public ObservableCollection<IMeasurementPointParameters> ObservableSelectedParameters { get { return _observableSelectedParameters; } set { SetProperty(ref _observableSelectedParameters, value); } }
//
//         private ObservableCollection<IMeasurementPointParameters> _observableParameterCollection;
//         public ObservableCollection<IMeasurementPointParameters> ObservableParameterCollection { get { return _observableParameterCollection; } set { SetProperty(ref _observableParameterCollection, value); } }
//
//         private IPostgresql _database;
//
//         #endregion // Properties
//
//         public MeasurementPointsDetailViewModel(IPostgresql database)
//         {
//             _database = database;
//             ObservableParameterCollection = new ObservableCollection<IMeasurementPointParameters>();
//             ObservableSelectedParameters = new ObservableCollection<IMeasurementPointParameters>();
//             //GetValuesAndTypesFromDatabase();
//             PopulateObservableCollection();
//         }
//
//         #region Methods
//
//         private void PopulateObservableCollection()
//         {
//             //TODO: 
//             IMeasurementPointsDetailService detailService = new MeasurementPointsDetailService(_database);
//             detailService.GetParameters(_observableParameterCollection);
//         }
//
//         // public ObservableCollection<MeasurementPointParameters> GetValuesAndTypesFromDatabase()
//         // {
//         //     ObservableParameterCollection.Clear();
//         //
//         //     foreach (var item in _database.GetMeasurementPointValueTypeParameters().ToList())
//         //     {
//         //         ObservableParameterCollection.Add(item);
//         //     }
//         //
//         //     return ObservableParameterCollection;
//         // }
//
//         public override void OnNavigatedTo(NavigationContext navigationContext)
//         {
//             if (navigationContext.Parameters.ContainsKey("selectedTemplate"))
//             {
//                 //TODO: Handle InvalidCastException in a more proper manner
//                 try
//                 {
//                     SelectedMeasurementPointTemplate = navigationContext.Parameters.GetValue<IMeasurementPointTemplate>("selectedTemplate");
//                 }
//                 catch (Exception e)
//                 {
//                     Console.WriteLine(e);
//                     return;
//                 }
//
//
//                 ObservableSelectedParameters.Clear();
//
//                 for (int i = 0; i < ObservableParameterCollection.Count; i++)
//                 {
//                     if (SelectedMeasurementPointTemplate.Id == ObservableParameterCollection[i].TemplateId)
//                     {
//                         ObservableSelectedParameters.Add(ObservableParameterCollection[i]);
//                     }
//                 }
//
//             }
//
//             var measurementPoint = navigationContext.Parameters["selectedTemplate"] as IMeasurementPointTemplate;
//
//             if (measurementPoint != null)
//             {
//                 SelectedMeasurementPointTemplate = measurementPoint;
//             }
//         }
//
//         public override bool IsNavigationTarget(NavigationContext navigationContext)
//         {
//             var measurementPoint = navigationContext.Parameters["selectedTemplate"] as IMeasurementPointTemplate;
//
//             if (measurementPoint != null)
//             {
//                 // Create new instance if Id does not match and parameter is not null
//                 return SelectedMeasurementPointTemplate != null && SelectedMeasurementPointTemplate.Id == measurementPoint.Id;
//             }
//
//             return true;
//
//         }
//
//         public void OnNavigatedFrom(NavigationContext navigationContext)
//         {
//             base.OnNavigatedFrom(navigationContext);
//         }
//
//         #endregion // Methods
//     }
// }
//
