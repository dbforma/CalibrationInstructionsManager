using System.Collections.Generic;

namespace ChannelSettings.Module.Service
{
    public interface IChannelSettingsDetailService
    {
        void PassChannelSettingParametersToDatabase(Dictionary<string, int> dict);
    }
}