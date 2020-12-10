using System.Collections.Generic;
using System.Collections.ObjectModel;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Models.Templates;
// using Prism.Events;
// using Prism.Mvvm;

namespace ChannelSettings.Module.Service
{
    /// <summary>
    /// First step is to copy the selected template (based on oldId) and use the postgres returning statement to save the new auto generated Id to our dictionary
    /// Second step is to copy parameters and use newId therefor
    /// </summary>
    public class ChannelSettingsOverviewService : IChannelSettingsOverviewService
    {
        private readonly IPostgresql _database;
        private readonly Dictionary<string, int> _idDictionary;

        private ObservableCollection<IChannelSettingTemplate> _channelSettingTemplates;
        public ObservableCollection<IChannelSettingTemplate> ChannelSettingTemplates { get { return _channelSettingTemplates; } }

        private readonly IChannelSettingsDetailService _detailService;

        public ChannelSettingsOverviewService(IPostgresql database, IChannelSettingsDetailService detailService)
        {
            _database = database;
            _idDictionary = new Dictionary<string, int>();
            _detailService = detailService;
        }

        public void CopyTemplate(IChannelSettingTemplate selectedItem)
        {
            int generatedId = _database.CopyChannelSettingTemplate(selectedItem);

            //_idDictionary needs to be cleared, otherwise results in key constraint
            _idDictionary.Clear(); 
            _idDictionary.Add("oldId", selectedItem.Id);
            _idDictionary.Add("newId", generatedId);

            CopyParameters();
        }

        public void CopyParameters()
        {
            _detailService.PassChannelSettingParametersToDatabase(_idDictionary);
        }

        public void GetTemplates(ObservableCollection<IChannelSettingTemplate> ChannelSettingTemplates)
        {
            ChannelSettingTemplates.Clear();

            foreach (var item in _database.GetChannelSettingTemplates())
            {
                ChannelSettingTemplates.Add(item);
            }
        }
    }
}
