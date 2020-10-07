using System.ComponentModel;

namespace CalibrationInstructionsManager.Core.Models.Templates
{
    public interface IChannelSettingTemplate : INotifyPropertyChanged
    {
        #region Properties

        int? Id { get; set; }
        string FullName { get; set; }

        #endregion // Properties

        #region INotifyPropertyChanged

        event PropertyChangedEventHandler PropertyChanged;

        #endregion // INotifyPropertyChanged

        #region Methods

        string ToString();

        #endregion // Methods
    }
}