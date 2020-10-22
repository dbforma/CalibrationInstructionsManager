using System.Collections.Generic;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Models.Parameters;

namespace ChannelSettings.Module.Service
{
    /// <summary>
    /// oldId contains the Id of the selected item that shall be copied
    /// newId contains the generated Id from returned value from PostgreSQL statement and is used in this class as a foreign key
    /// </summary>
    public class ChannelSettingsDetailService
    {
        private IPostgreSQLDatabase _database;

        public ChannelSettingsDetailService(IPostgreSQLDatabase database)
        {
            _database = database;
        }

        public void PassChannelSettingParametersToDatabase(Dictionary<string, int> dict)
        {
            ChannelSettingParameters _channelSettingParameters;

            int oldId;
            dict.TryGetValue("oldId", out oldId);

            int newId;
            dict.TryGetValue("newId", out newId);

            var selectedChannelSettingParameters = _database.GetSelectedChannelSettingParameters(oldId);

            foreach (var item in selectedChannelSettingParameters)
            {
                _channelSettingParameters = new ChannelSettingParameters();

                _channelSettingParameters.TemplateId = newId;
                _channelSettingParameters.DefaultValue = item.DefaultValue;
                _channelSettingParameters.UncertaintyValue = item.UncertaintyValue;
                _channelSettingParameters.ParameterIndex = item.ParameterIndex;
                _channelSettingParameters.ParameterQuantity = item.ParameterQuantity;
                _channelSettingParameters.TypeId = item.TypeId;

                _database.CopyExistingChannelSettingParameters(_channelSettingParameters);

            }
        }
    }
}
