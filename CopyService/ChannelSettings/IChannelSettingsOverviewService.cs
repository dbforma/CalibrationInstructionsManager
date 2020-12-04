using System.Collections.ObjectModel;
using CalibrationInstructionsManager.Core.Models.Templates;

namespace ChannelSettings.Module.Service
{
    public interface IChannelSettingsOverviewService
    {
        void CopyTemplate(IChannelSettingTemplate selectedItem);
        void CopyParameters();
        void GetTemplates(ObservableCollection<IChannelSettingTemplate> ChannelSettingTemplates);
    }
}