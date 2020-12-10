using CalibrationInstructionsManager.Core.Models.Parameters.Contract;

namespace CalibrationInstructionsManager.Core.Models.Parameters
{
    public class DefaultConfigurationParameters : IDefaultConfigurationParameters
    {
        private int _parameterId;
        private int _templateId;
        private int _typeId;
        private string _typeName;
        private double? _value;

        public int ParameterId { get { return _parameterId; } set { _parameterId = value;  } }
        public int TemplateId { get { return _templateId; } set { _templateId = value;  } }
        public int TypeId { get { return _typeId; } set { _typeId = value;  } }
        public string TypeName { get { return _typeName; } set { _typeName = value;  } }
        public double? Value { get { return _value; } set { _value = value;  } }
    }
}
