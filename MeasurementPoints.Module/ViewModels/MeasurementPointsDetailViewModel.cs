using System;
using System.Collections.ObjectModel;
using System.Linq;
using CalibrationInstructionsManager.Core;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Models;
using CalibrationInstructionsManager.Core.Models.Templates;
using CalibrationInstructionsManager.Core.Models.ValueTypes;
using Prism.Regions;
using System.Globalization;

namespace MeasurementPoints.Module.ViewModels
{
    public class MeasurementPointsDetailViewModel : ViewModelBase, INavigationAware
    {
        #region Properties
        private MeasurementPointTemplate _selectedMeasurementPointTemplate;
        public MeasurementPointTemplate SelectedMeasurementPointTemplate { get { return _selectedMeasurementPointTemplate; } set { SetProperty(ref _selectedMeasurementPointTemplate, value); } }

        private ObservableCollection<MeasurementPointValueType> _selectedValuesAndTypes;
        public ObservableCollection<MeasurementPointValueType> SelectedValuesAndTypes { get { return _selectedValuesAndTypes; } set { SetProperty(ref _selectedValuesAndTypes, value); } }

        private ObservableCollection<MeasurementPointValueType> _observableValuesAndTypes;
        public ObservableCollection<MeasurementPointValueType> ObservableValuesAndTypes { get { return _observableValuesAndTypes; } set { SetProperty(ref _observableValuesAndTypes, value); } }

        private IPostgreSQLDatabase _database;

        #endregion // Properties

        public MeasurementPointsDetailViewModel(IPostgreSQLDatabase database)
        {
            _database = database;
            ObservableValuesAndTypes = new ObservableCollection<MeasurementPointValueType>();
            SelectedValuesAndTypes = new ObservableCollection<MeasurementPointValueType>();
            GetValuesAndTypesFromDatabase();
        }

        #region Methods

        public ObservableCollection<MeasurementPointValueType> GetValuesAndTypesFromDatabase()
        {
            ObservableValuesAndTypes.Clear();

            foreach (var item in _database.GetMeasurementPointValueTypeParameters().ToList())
            {
                ObservableValuesAndTypes.Add(item);
            }

            return ObservableValuesAndTypes;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("selectedTemplate"))
            {
                try
                {
                    SelectedMeasurementPointTemplate = navigationContext.Parameters.GetValue<MeasurementPointTemplate>("selectedTemplate");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }
                

                SelectedValuesAndTypes.Clear();

                for (int i = 0; i < ObservableValuesAndTypes.Count; i++)
                {
                    if (SelectedMeasurementPointTemplate.Id == ObservableValuesAndTypes[i].TemplateId)
                    {
                        SelectedValuesAndTypes.Add(ObservableValuesAndTypes[i]);
                    }
                }

            }

            var measurementPoint = navigationContext.Parameters["selectedTemplate"] as MeasurementPointTemplate;

            if (measurementPoint != null)
            {
                SelectedMeasurementPointTemplate = measurementPoint;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var measurementPoint = navigationContext.Parameters["selectedTemplate"] as MeasurementPointTemplate;

            if (measurementPoint != null)
            {
                // Create new instance if FullName does not match
                return SelectedMeasurementPointTemplate != null && SelectedMeasurementPointTemplate.FullName.ToLower() == measurementPoint.FullName.ToLower();
            }
            else
            {
                return true;
            }
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);

        }

        #endregion // Methods
    }
}

