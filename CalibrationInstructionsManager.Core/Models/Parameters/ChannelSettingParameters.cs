using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CalibrationInstructionsManager.Core.Models.Parameters.Contract;

namespace CalibrationInstructionsManager.Core.Models.Parameters
{
    public class ChannelSettingParameters : IChannelSettingParameters
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

        // public ChannelSettingParameters()
        // {
        //     
        // }

        // public ChannelSettingParameters(int parameterId, int templateId, double? defaultValue, double? uncertaintyValue, int parameterIndex, int parameterQuantity, string typeName, int typeId)
        // {
        //     _parameterId = parameterId;
        //     _templateId = templateId;
        //     _defaultValue = defaultValue;
        //     _uncertaintyValue = uncertaintyValue;
        //     _parameterIndex = parameterIndex;
        //     _parameterQuantity = parameterQuantity;
        //     _typeName = typeName;
        //     _typeId = typeId;
        // }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        // protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        // {
        //     PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        // }

        #endregion // INotifyPropertyChanged

        #region Methods

        public override string ToString()
        {
            return String.Format($"{ParameterId} {TemplateId} {DefaultValue} {TemplateId} {UncertaintyValue} {ParameterIndex} {ParameterQuantity} {TypeName} {TypeId}");
        }

        #endregion

    }
}
