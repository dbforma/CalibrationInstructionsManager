using System.Collections.Generic;
using CalibrationInstructionsManager.Core.Models.Parameters;
using CalibrationInstructionsManager.Core.Models.Parameters.Contract;
using CalibrationInstructionsManager.Core.Models.Templates;
using CalibrationInstructionsManager.Core.Models.Types;


namespace CalibrationInstructionsManager.Core.Data
{
    public interface IPostgresql
    {
        void SetDatabaseLoginData(string host, string port, string username, string password, string database);
        bool IsConnectionEstablished();
        LinkedList<Models.Templates.IDefaultConfigurationTemplate> GetDefaultConfigurationTemplates();
        LinkedList<DefaultConfigurationParameters> GetDefaultConfigurationValueTypeParameters();
        LinkedList<IMeasurementPointTemplate> GetMeasurementPointTemplates();
        LinkedList<MeasurementPointParameters> GetMeasurementPointValueTypeParameters();
        LinkedList<IChannelSettingTemplate> GetChannelSettingTemplates();
        LinkedList<IChannelSettingParameters> GetChannelSettingParameters();
        LinkedList<IChannelSettingType> GetChannelSettingTypes();
        int CopyChannelSettingTemplate(IChannelSettingTemplate template);
        void CopyExistingChannelSettingParameters(IChannelSettingParameters parameter);
        LinkedList<IChannelSettingParameters> GetSelectedChannelSettingParameters(int? selectedId);
    }
}