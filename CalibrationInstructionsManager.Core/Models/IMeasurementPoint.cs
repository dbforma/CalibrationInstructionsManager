using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CalibrationInstructionsManager.Core.Models
{
    public interface IMeasurementPoint : INotifyPropertyChanged
    {
        #region Properties

        int Id { get; set; }
        string FullName { get; set; }
        string Commentary { get; set; }
        string Properties { get; set; }
        List<string> PropertiesList { get; set; }
        string PropertiesDefaultValue { get; set; }
        string PropertiesDefaultType { get; set; }
        DateTime? LastUpdated { get; set; }

        #endregion // Properties

        event PropertyChangedEventHandler PropertyChanged;

        #region Methods

        string ToString();

        #endregion // Methods
    }
}