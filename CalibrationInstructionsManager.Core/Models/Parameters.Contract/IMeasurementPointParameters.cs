namespace CalibrationInstructionsManager.Core.Models.Parameters
{
    public interface IMeasurementPointParameters 
    {
        int TemplateId { get; set; }
        int TypeId { get; set; }
        string ParameterName { get; set; }
        double? DefaultValue { get; set; }
        string ValueDescription { get; set; }
    }
}
