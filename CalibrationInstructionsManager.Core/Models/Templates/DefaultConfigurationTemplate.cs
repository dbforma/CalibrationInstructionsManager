using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Documents;

namespace CalibrationInstructionsManager.Core.Models.Templates
{
    [Serializable]
    public class DefaultConfigurationTemplate : IDefaultConfigurationTemplate
    {
        #region Properties

        private int _id;
        private string _fullName;
        private string _commentary;
        // private DefaultConfigurationType _defaultConfigurationType;
        
        public int Id { get { return _id; } set { _id = value; OnPropertyChanged(); } }
        public string FullName { get { return _fullName; } set { _fullName = value; OnPropertyChanged(); } }
        public string Commentary { get { return _commentary; } set { _commentary = value; OnPropertyChanged(); } }
        // public DefaultConfigurationType DefaultConfigurationType { get { return _defaultConfigurationType; } set { _defaultConfigurationType = value; OnPropertyChanged(); } }

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
            return String.Format($"{Id} {FullName} {Commentary}");
        }

        #endregion // Methods
    }

    //TODO: Implement more types, database needs new column: type
    // public enum DefaultConfigurationType
    // {
    //     Bandpassclockfrequency,
    //     Integratormodus
    // }
}
