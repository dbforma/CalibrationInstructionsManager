namespace CalibrationInstructionsManager.Core.Models.Templates
{
    public class MeasurementPointTemplate : IMeasurementPointTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Commentary { get; set; }
    }
}
