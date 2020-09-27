using System.Collections.Generic;
using CalibrationInstructionsManager.Core.Models.ValueTypes;


namespace CalibrationInstructionsManager.Core.Data
{
    public interface IPostgreSQLDatabase
    {
        void SetDatabaseLoginData(string host, string port, string username, string password, string database);
        bool IsConnectionEstablished();
        LinkedList<Models.Templates.DefaultConfigurationTemplate> GetDefaultConfigurationTemplates();
        LinkedList<DefaultConfigurationValueType> GetDefaultConfigurationValueTypeParameters();
    }
}