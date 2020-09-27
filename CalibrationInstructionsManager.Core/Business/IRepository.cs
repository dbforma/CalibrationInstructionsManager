using System.Collections.Generic;
using CalibrationInstructionsManager.Core.Models;

namespace CalibrationInstructionsManager.Core.Business
{
    public interface IRepository
    {
        #region Methods

        List<MeasurementPoint> GetMeasurementPoints(int total);
        MeasurementPoint GetMeasurementPoint(int id);

        List<string> GetPropertiesList();

        #endregion
    }
}