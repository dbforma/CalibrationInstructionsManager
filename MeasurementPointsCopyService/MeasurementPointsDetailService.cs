using System.Collections.Generic;
using System.Collections.ObjectModel;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Models.Parameters;

namespace MeasurementPointsCopyService
{
    /// <summary>
    /// oldId contains the Id of the selected item that shall be copied
    /// newId contains the generated Id from returned value from PostgreSQL statement and is used in this class as a foreign key
    /// </summary>
    public class MeasurementPointsDetailService : IMeasurementPointsDetailService
    {
        private IPostgresql _database;

        private ObservableCollection<IMeasurementPointParameters> _observableParameterCollection { get; }

        public MeasurementPointsDetailService(IPostgresql database)
        {
            _observableParameterCollection = new ObservableCollection<IMeasurementPointParameters>();
            _database = database;
        }

        public void PassMeasurementPointParametersToDatabase(Dictionary<string, int> dict)
        {
            MeasurementPointParameters _measurementPointParameters;

            int oldId;
            dict.TryGetValue("oldId", out oldId);

            int newId;
            dict.TryGetValue("newId", out newId);

            var selectedChannelSettingParameters = _database.GetSelectedMeasurementPointParameters(oldId);

            foreach (var item in selectedChannelSettingParameters)
            {
                _measurementPointParameters = new MeasurementPointParameters();

                _measurementPointParameters.TemplateId = newId;
                _measurementPointParameters.DefaultValue = item.DefaultValue;
                _measurementPointParameters.ParameterName = item.ParameterName;
                _measurementPointParameters.ValueDescription = item.ValueDescription;
                _measurementPointParameters.TypeId = item.TypeId;

                _database.CopyExistingMeasurementPointParameters(_measurementPointParameters);

            }
        }


        public void GetParameters(ObservableCollection<IMeasurementPointParameters> measurementPointParameters)
        {
            _observableParameterCollection.Clear();
        
            foreach (var item in _database.GetMeasurementPointValueTypeParameters())
            {
                _observableParameterCollection.Add(item);
            }
        }
    }
}
