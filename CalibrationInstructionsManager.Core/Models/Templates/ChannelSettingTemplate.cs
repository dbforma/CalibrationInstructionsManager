using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CalibrationInstructionsManager.Core.Models.Templates
{
    public class ChannelSettingTemplate : IChannelSettingTemplate
    {
        #region Properties

        private int _id;
        private string _fullName;

        public int Id { get { return _id; } set { _id = value; OnPropertyChanged(); } }
        public string FullName { get { return _fullName; } set { _fullName = value; OnPropertyChanged(); } }

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
            return String.Format($"{Id} {FullName}");
        }

        #endregion // Methods
    }
}
