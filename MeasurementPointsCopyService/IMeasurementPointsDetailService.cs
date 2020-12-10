using System.Collections.Generic;
using System.Collections.ObjectModel;
using CalibrationInstructionsManager.Core.Models.Parameters;

namespace MeasurementPointsCopyService
{
    public interface IMeasurementPointsDetailService
    {
        void PassMeasurementPointParametersToDatabase(Dictionary<string, int> dict);
        void GetParameters(ObservableCollection<IMeasurementPointParameters> measurementPointParameters);
    }
}