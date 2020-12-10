namespace CalibrationInstructionsManager.Core.Models.Parameters
{
    public class MeasurementPointParameters : IMeasurementPointParameters
    {
        private int _templateId;
        private int _typeId;
        private string _parameterName;
        private double? _defaultValue;
        private string _valueDescription;

        public int TemplateId { get { return _templateId; } set { _templateId = value; } }
        public int TypeId { get { return _typeId; } set { _typeId = value; } }
        public string ParameterName { get { return _parameterName; } set { _parameterName = value; } }
        public double? DefaultValue { get { return _defaultValue; } set { _defaultValue = value; } }
        public string ValueDescription { get { return _valueDescription; } set { _valueDescription = value; } }
    }
}
