using System.Collections.Generic;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Models.Templates;

namespace ChannelSettings.Module.Service
{
    /// <summary>
    /// First step is to copy the selected template (based on oldId) and use the postgres returning statement to save the new auto generated Id to our dictionary
    /// Second step is to copy parameters and use newId therefor
    /// </summary>
    public class ChannelSettingsOverviewService
    {
        private readonly IPostgreSQLDatabase _database;
        private readonly Dictionary<string, int> _idDictionary;

        public ChannelSettingsOverviewService(IPostgreSQLDatabase database)
        {
            _database = database;
            _idDictionary = new Dictionary<string, int>();
        }

        public void CopyTemplate(ChannelSettingTemplate selectedItem)
        {
            int generatedId = _database.CopyChannelSettingTemplate(selectedItem);

            _idDictionary.Add("oldId", selectedItem.Id);
            _idDictionary.Add("newId", generatedId);

            CopyParameters();
        }

        public void CopyParameters()
        {
            ChannelSettingsDetailService detailService = new ChannelSettingsDetailService(_database); // Konstruktor

            detailService.PassChannelSettingParametersToDatabase(_idDictionary);

            // event für detailservice benachrichtigen und vice versa
        }
    }
}
