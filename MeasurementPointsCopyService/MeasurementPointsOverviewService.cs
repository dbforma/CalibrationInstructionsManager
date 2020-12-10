using System.Collections.Generic;
using System.Collections.ObjectModel;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Models.Templates;
// using Prism.Events;
// using Prism.Mvvm;

namespace MeasurementPointsCopyService
{
    /// <summary>
    /// First step is to copy the selected template (based on oldId) and use the postgres returning statement to save the new auto generated Id to our dictionary
    /// Second step is to copy parameters and use newId therefor
    /// </summary>
    public class MeasurementPointsOverviewService : IMeasurementPointsOverviewService
    {
        private readonly IPostgresql _database;
        private readonly Dictionary<string, int> _idDictionary;

        private ObservableCollection<IMeasurementPointTemplate> _measurementPointTemplates;
        public ObservableCollection<IMeasurementPointTemplate> MeasurementPointTemplates { get { return _measurementPointTemplates; } set { _measurementPointTemplates = value; } }

        private readonly IMeasurementPointsDetailService _detailService;

        public MeasurementPointsOverviewService(IPostgresql database, IMeasurementPointsDetailService detailService)
        {
            _database = database;
            _idDictionary = new Dictionary<string, int>();
            _detailService = detailService;
        }

        public void CopyTemplate(IMeasurementPointTemplate selectedItem)
        {
            int generatedId = _database.CopyMeasurementPointTemplate(selectedItem);

            //_idDictionary needs to be cleared, otherwise results in key constraint
            _idDictionary.Clear(); 
            _idDictionary.Add("oldId", selectedItem.Id);
            _idDictionary.Add("newId", generatedId);

            CopyParameters();
        }

        public void CopyParameters()
        {
            _detailService.PassMeasurementPointParametersToDatabase(_idDictionary);
        }

        public void GetTemplates(ObservableCollection<IMeasurementPointTemplate> MeasurementPointTemplates)
        {
            MeasurementPointTemplates.Clear();

            foreach (var item in _database.GetMeasurementPointTemplates())
            {
                MeasurementPointTemplates.Add(item);
            }
        }
    }
}
