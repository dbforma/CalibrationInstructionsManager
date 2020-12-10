using System.Collections.ObjectModel;
using CalibrationInstructionsManager.Core.Models.Templates;

namespace MeasurementPointsCopyService
{
    public interface IMeasurementPointsOverviewService
    {
        void CopyTemplate(IMeasurementPointTemplate selectedItem);
        void CopyParameters();
        void GetTemplates(ObservableCollection<IMeasurementPointTemplate> MeasurementPointTemplates);
    }
}