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
        public ObservableCollection<IChannelSettingTemplate> ChannelSettingTemplates { get { return _channelSettingTemplates; } set { _channelSettingTemplates = value; } }

        public ChannelSettingsOverviewService(IPostgresql database)
        {
            //_eventAggregator = eventAggregator;
            _database = database;
            _idDictionary = new Dictionary<string, int>();
        }

        public void CopyTemplate(IChannelSettingTemplate selectedItem)
        {
            int generatedId = _database.CopyChannelSettingTemplate(selectedItem);

            _idDictionary.Clear(); //TODO: Gleicher Schlüssel wurde bereits verwendet, wenn nicht Clear benutzt wird
            _idDictionary.Add("oldId", selectedItem.Id);
            _idDictionary.Add("newId", generatedId);

            CopyParameters();
        }

        public void CopyParameters()
        {
            ChannelSettingsDetailService detailService = new ChannelSettingsDetailService(_database); // TODO: Konstruktor Injection

            detailService.PassChannelSettingParametersToDatabase(_idDictionary);

            // Event für detailservice benachrichtigen und vice versa
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
