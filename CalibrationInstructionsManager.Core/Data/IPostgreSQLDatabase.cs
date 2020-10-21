using System.Collections.Generic;
using CalibrationInstructionsManager.Core.Models.Parameters;
using CalibrationInstructionsManager.Core.Models.Templates;
using CalibrationInstructionsManager.Core.Models.Types;


namespace CalibrationInstructionsManager.Core.Data
{
    public interface IPostgreSQLDatabase
    {
        void SetDatabaseLoginData(string host, string port, string username, string password, string database);
        bool IsConnectionEstablished();
        LinkedList<Models.Templates.DefaultConfigurationTemplate> GetDefaultConfigurationTemplates();
        LinkedList<DefaultConfigurationParameters> GetDefaultConfigurationValueTypeParameters();
        LinkedList<MeasurementPointTemplate> GetMeasurementPointTemplates();
        LinkedList<MeasurementPointParameters> GetMeasurementPointValueTypeParameters();
        LinkedList<ChannelSettingTemplate> GetChannelSettingTemplates();
        LinkedList<ChannelSettingParameters> GetChannelSettingParameters();
        LinkedList<IChannelSettingType> GetChannelSettingTypes();
        int CopyExistingChannelSettingTemplate(ChannelSettingTemplate template);
        void CopyExistingChannelSettingParameters(ChannelSettingParameters parameter);
        LinkedList<ChannelSettingParameters> GetSelectedChannelSettingParameters(int? selectedId);
    }
}