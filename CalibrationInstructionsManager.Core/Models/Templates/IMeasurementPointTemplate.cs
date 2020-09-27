namespace CalibrationInstructionsManager.Core.Models.Templates
{
    public interface IMeasurementPointTemplate
    {
        int Id { get; set; }
        string Name { get; set; }
        string Commentary { get; set; }
    }
}