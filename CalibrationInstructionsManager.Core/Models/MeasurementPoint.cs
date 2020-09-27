using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CalibrationInstructionsManager.Core.Models
{
    public class MeasurementPoint : IMeasurementPoint
    {
        #region Properties

        private int _id;
        private string _fullName;
        private string _commentary;
        private string _properties;
        private List<string> _propertiesList;
        private string _propertiesDefaultValue;
        private string _propertiesDefaultType;
        private DateTime? _lastUpdated;

        public int Id { get { return _id; } set { _id = value; OnPropertyChanged(); } }
        public string FullName { get { return _fullName; } set { _fullName = value; OnPropertyChanged(); } }
        public string Commentary { get { return _commentary; } set { _commentary = value; OnPropertyChanged(); } }
        public string Properties { get { return _properties; } set { _properties = value; OnPropertyChanged(); } }
        public List<string> PropertiesList { get { return _propertiesList; } set { _propertiesList = value; OnPropertyChanged(); } }
        public string PropertiesDefaultValue { get { return _propertiesDefaultValue; } set { _propertiesDefaultValue = value; OnPropertyChanged(); } }
        public string PropertiesDefaultType { get { return _propertiesDefaultType; } set { _propertiesDefaultType = value; OnPropertyChanged(); } }
        public DateTime? LastUpdated { get { return _lastUpdated; } set { _lastUpdated = value; OnPropertyChanged(); } }

        #endregion // Properties

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged

        #region Methods

        public override string ToString()
        {
            return String.Format($"{FullName} {Commentary} {Properties} {PropertiesList} {PropertiesDefaultValue} {PropertiesDefaultType} {LastUpdated}");
        }

        #endregion // Methods
    }
}
