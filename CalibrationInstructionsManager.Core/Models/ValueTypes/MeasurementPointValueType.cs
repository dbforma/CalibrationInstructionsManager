using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationInstructionsManager.Core.Models.ValueTypes
{
    public class MeasurementPointValueType : ViewModelBase
    {
        private int _valueId;
        private int _templateId;
        private int _typeId;
        private string _parameterName;
        private double? _defaultValue;
        private string _valueDescription;

        public int ValueId { get { return _valueId; } set { _valueId = value; OnPropertyChanged(); } }
        public int TemplateId { get { return _templateId; } set { _templateId = value; OnPropertyChanged(); } }
        public int TypeId { get { return _typeId; } set { _typeId = value; OnPropertyChanged(); } }
        public string ParameterName { get { return _parameterName; } set { _parameterName = value; OnPropertyChanged(); } }
        public double? DefaultValue { get { return _defaultValue; } set { _defaultValue = value; OnPropertyChanged(); } }
        public string ValueDescription { get { return _valueDescription; } set { _valueDescription = value; OnPropertyChanged(); } }
    }
}
