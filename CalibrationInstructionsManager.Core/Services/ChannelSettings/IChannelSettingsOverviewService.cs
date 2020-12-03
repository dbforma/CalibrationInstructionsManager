using System.Collections.ObjectModel;
using CalibrationInstructionsManager.Core.Models.Templates;

namespace ChannelSettings.Module.Service
{
    public interface IChannelSettingsOverviewService
    {
        void CopyTemplate(ChannelSettingTemplate selectedItem);
        void CopyParameters();
        void GetTemplates(ObservableCollection<IChannelSettingTemplate> ChannelSettingTemplates);
    }
}