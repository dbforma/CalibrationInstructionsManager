namespace CalibrationInstructionsManager.Core.Models.ValueTypes
{
    public class ChannelSettingParameters 
    {
        private int _parameterId;
        private int _templateId;
        private double? _defaultValue;
        private double? _uncertaintyValue;
        private int _parameterIndex;
        private int _parameterQuantity;
        private string _typeName;
        private int _typeId;

        public int ParameterId { get { return _parameterId; } set { _parameterId = value;  } }
        public int TemplateId { get { return _templateId; } set { _templateId = value;  } }
        public double? DefaultValue { get { return _defaultValue; } set { _defaultValue = value;  } }
        public double? UncertaintyValue { get { return _uncertaintyValue; } set { _uncertaintyValue = value;  } }
        public int ParameterIndex { get { return _parameterIndex; } set { _parameterIndex = value;  } }
        public int ParameterQuantity { get { return _parameterQuantity; } set { _parameterQuantity = value;  } }
        public string TypeName { get { return _typeName; } set { _typeName = value;  } }
        public int TypeId { get { return _typeId; } set { _typeId = value;  } }
    }
}
