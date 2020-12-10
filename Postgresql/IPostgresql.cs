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
        LinkedList<DefaultConfigurationParameters> GetDefaultConfigurationValueTypeParameters();
        
        LinkedList<IChannelSettingType> GetChannelSettingTypes();

        LinkedList<IChannelSettingTemplate> GetChannelSettingTemplates();
        LinkedList<IChannelSettingParameters> GetChannelSettingParameters();

        LinkedList<IMeasurementPointTemplate> GetMeasurementPointTemplates();
        LinkedList<MeasurementPointParameters> GetMeasurementPointValueTypeParameters();

        LinkedList<IDefaultConfigurationTemplate> GetDefaultConfigurationTemplates();
        LinkedList<IChannelSettingParameters> GetDefaultConfigurationParameters();

        int CopyChannelSettingTemplate(IChannelSettingTemplate template);
        void CopyExistingChannelSettingParameters(IChannelSettingParameters parameter);
        LinkedList<IChannelSettingParameters> GetSelectedChannelSettingParameters(int? selectedId);

        int CopyMeasurementPointTemplate(IMeasurementPointTemplate template);
        void CopyExistingMeasurementPointParameters(IMeasurementPointParameters parameter);
        LinkedList<IMeasurementPointParameters> GetSelectedMeasurementPointParameters(int? selectedId);

        int CopyDefaultConfigurationTemplate(IDefaultConfigurationTemplate template);
        void CopyExistingDefaultConfigurationParameters(IDefaultConfigurationParameters parameter);
        LinkedList<IDefaultConfigurationParameters> GetSelectedDefaultConfigurationParameters(int? selectedId);
    }
}