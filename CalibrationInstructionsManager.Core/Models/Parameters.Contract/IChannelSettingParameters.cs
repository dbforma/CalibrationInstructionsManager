using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationInstructionsManager.Core.Models.Parameters.Contract
{
    public interface IChannelSettingParameters : INotifyPropertyChanged
    {
        #region Properties

        int ParameterId { get; set; }
        int TemplateId { get; set; }
        double? DefaultValue { get; set; }
        double? UncertaintyValue { get; set; }
        int ParameterIndex { get; set; }
        int ParameterQuantity { get; set; }
        string TypeName { get; set; }
        int TypeId { get; set; }

        #endregion // Properties

        #region INotifyPropertyChanged

        event PropertyChangedEventHandler PropertyChanged;

        #endregion // INotifyPropertyChanged

        #region Methods

        string ToString();

        #endregion // Methods
    }
}
