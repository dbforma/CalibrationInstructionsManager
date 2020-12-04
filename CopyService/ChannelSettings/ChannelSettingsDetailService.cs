using System.Collections.Generic;
using System.Collections.ObjectModel;
using CalibrationInstructionsManager.Core.Data;
using CalibrationInstructionsManager.Core.Models.Parameters;
//using Prism.Mvvm;

namespace ChannelSettings.Module.Service
{
    /// <summary>
    /// oldId contains the Id of the selected item that shall be copied
    /// newId contains the generated Id from returned value from PostgreSQL statement and is used in this class as a foreign key
    /// </summary>
    public class ChannelSettingsDetailService : IChannelSettingsDetailService
    {
        private IPostgresql _database;

        private ObservableCollection<ChannelSettingParameters> _observableParameterCollection { get; }

        public ChannelSettingsDetailService(IPostgresql database)
        {
            _observableParameterCollection = new ObservableCollection<ChannelSettingParameters>();
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

        // TODO: Fix 
        // public void GetParameters(ObservableCollection<ChannelSettingParameters> channelSettingParameters)
        // {
        //     _observableParameterCollection.Clear();
        //
        //     foreach (var item in _database.GetChannelSettingParameters())
        //     {
        //         _observableParameterCollection.Add(item);
        //     }
        // }
    }
}
