// Note: System.Threading.Tasks.Extensions must be <= 4.5.2
// TODO: Seperate PostgreSQLDatabase into GetChannelSettingsCommand/ UpdateChannelSettingsCommands, GetMeasurementPointsCommand/ UpdateMeasurementPointsCommand and GetDefaultConfigurationsCommand/ UpdateDefaultConfigurationsCommand

using System;
using System.Collections.Generic;
using System.Data;
using CalibrationInstructionsManager.Core.Models.Parameters;
using CalibrationInstructionsManager.Core.Models.Templates;
using CalibrationInstructionsManager.Core.Models.Types;
using Npgsql;
using NpgsqlTypes;

namespace CalibrationInstructionsManager.Core.Data
{
    public class PostgreSQLDatabase : IPostgreSQLDatabase
    {
        private string _connectionString;
        private string _host;
        private string _port;
        private string _username;
        private string _password;
        private string _database;
        private bool _isConnected = false;


        /// <summary>
        /// Sets the Connection-string that is being used for NpgsqlConnection object
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="database"></param>
        public void SetDatabaseLoginData(string host, string port, string username, string password, string database)
        {
            _host = host;
            _port = port;
            _username = username;
            _password = password;
            _database = database;
            _connectionString = $"Host={host};Port={port};Username={username};Password={password};Database={database}";
        }

        /// <summary>
        /// Verifies if connection is established
        /// </summary>
        public bool IsConnectionEstablished()
        {
            try
            {
                if (_connectionString != null)
                {
                    try
                    {
                        using (var connection = new NpgsqlConnection(_connectionString))
                        {
                            connection.Open();
                            if (connection != null && connection.State == ConnectionState.Open)
                            {
                                return _isConnected = true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (TimeoutException e)
            {
                Console.WriteLine("Timeout! " + e.Message);
            }

            return _isConnected = false;
        }

        /// <summary>
        /// Queries the table kkonfig to get corresponding attributes of each default configuration dataset
        /// Casts datarecordInternal to POCO object defined in CalibrationInstructionsManager.Core.Models.Templates
        /// </summary>
        public LinkedList<Models.Templates.DefaultConfigurationTemplate> GetDefaultConfigurationTemplates()
        {
            var defaultConfigurationTemplates = new LinkedList<Models.Templates.DefaultConfigurationTemplate>();
            string queryStatement = @" 
                                        SELECT kd.idkkonfig, kd.name, kd.kommentar
                                        FROM kkonfig kd
                                        ORDER BY idkkonfig";
            IsConnectionEstablished();

            if (_isConnected)
            {
                try
                {
                    using (var connection = new NpgsqlConnection(_connectionString))
                    {
                        using (var command = new NpgsqlCommand(queryStatement, connection))
                        {
                            connection.Open();
                            using (NpgsqlDataReader dataReader = command.ExecuteReader())
                            {
                                var indexId = dataReader.GetOrdinal("idkkonfig");
                                var indexName = dataReader.GetOrdinal("name");
                                var indexCommentary = dataReader.GetOrdinal("kommentar");

                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        var defaultConfigurationTemplate = new Models.Templates.DefaultConfigurationTemplate();

                                        defaultConfigurationTemplate.Id = (int)dataReader.GetValue(indexId);
                                        defaultConfigurationTemplate.FullName = dataReader.GetValue(indexName) as string;
                                        defaultConfigurationTemplate.Commentary = dataReader.GetValue(indexCommentary) as string;

                                        defaultConfigurationTemplates.AddLast(defaultConfigurationTemplate);
                                    }
                                }
                            }
                        }

                        connection.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return defaultConfigurationTemplates;
        }

        /// <summary>
        /// Queries the table kkonfigtyp and kkonfigliste to get corresponding attributes of each default configuration dataset
        /// Casts datarecordInternal to POCO object defined in CalibrationInstructionsManager.Core.Models.Parameters
        /// </summary>
        public LinkedList<DefaultConfigurationParameters> GetDefaultConfigurationValueTypeParameters()
        {
            var defaultConfigurationValueTypeCatalog = new LinkedList<DefaultConfigurationParameters>();
            string queryStatement = @"  
                                        SELECT kl.idkkonfigliste AS templateId, kl.kkonfig_id AS parameterId, kl.kkonfigtyp_id, kl.wert AS parameterValue, kt.idkkonfigtyp AS typeId, kt.name AS typeName
                                        FROM kkonfigliste kl
                                        JOIN kkonfigtyp kt
                                        ON kt.idkkonfigtyp = kl.kkonfigtyp_id";
            IsConnectionEstablished();

            if (_isConnected)
            {
                try
                {
                    using (var connection = new NpgsqlConnection(_connectionString))
                    {
                        using (var command = new NpgsqlCommand(queryStatement, connection))
                        {
                            connection.Open();
                            using (NpgsqlDataReader dataReader = command.ExecuteReader())
                            {

                                var indexParameterId = dataReader.GetOrdinal("parameterId");
                                var indexTemplateId = dataReader.GetOrdinal("templateId");
                                var indexTypeId = dataReader.GetOrdinal("typeId");
                                var indexTypeName = dataReader.GetOrdinal("typeName");
                                var indexValue = dataReader.GetOrdinal("parameterValue");

                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        var defaultConfigurationValueType = new DefaultConfigurationParameters();

                                        defaultConfigurationValueType.ParameterId = dataReader.GetInt32(indexParameterId);
                                        defaultConfigurationValueType.TemplateId = dataReader.GetInt32(indexTemplateId);
                                        defaultConfigurationValueType.TypeId = dataReader.GetInt32(indexTypeId);
                                        defaultConfigurationValueType.TypeName = dataReader.GetString(indexTypeName);
                                        defaultConfigurationValueType.Value = dataReader.GetDouble(indexValue);

                                        defaultConfigurationValueTypeCatalog.AddLast(defaultConfigurationValueType);

                                    }
                                }
                            }
                        }

                        connection.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return defaultConfigurationValueTypeCatalog;
        }


        /// <summary>
        /// Queries the table mkonfig to get corresponding attributes of each measurement point dataset
        /// Casts datarecordInternal to POCO object defined in CalibrationInstructionsManager.Core.Models.Templates
        /// </summary>
        /// <returns></returns>
        public LinkedList<MeasurementPointTemplate> GetMeasurementPointTemplates()
        {
            var measurementPointTemplates = new LinkedList<MeasurementPointTemplate>();
            string queryStatement = @" 
                                        SELECT md.idmkonfig, md.name, md.kommentar
                                        FROM mkonfig md
                                        ORDER BY idmkonfig";
            IsConnectionEstablished();

            if (_isConnected)
            {
                try
                {
                    using (var connection = new NpgsqlConnection(_connectionString))
                    {
                        using (var command = new NpgsqlCommand(queryStatement, connection))
                        {
                            connection.Open();
                            using (NpgsqlDataReader dataReader = command.ExecuteReader())
                            {
                                var indexId = dataReader.GetOrdinal("idmkonfig");
                                var indexName = dataReader.GetOrdinal("name");
                                var indexCommentary = dataReader.GetOrdinal("kommentar");

                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        var measurementPointTemplate = new MeasurementPointTemplate();

                                        measurementPointTemplate.Id = (int)dataReader.GetValue(indexId);
                                        measurementPointTemplate.FullName = dataReader.GetValue(indexName) as string;
                                        measurementPointTemplate.Commentary = dataReader.GetValue(indexCommentary) as string;

                                        measurementPointTemplates.AddLast(measurementPointTemplate);
                                    }
                                }
                            }
                        }

                        connection.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return measurementPointTemplates;
        }

        /// <summary>
        /// Queries the table konfigliste, mkonfig, mkonfigtyp and mkonfigvalues to get corresponding attributes of each measurement point dataset
        /// Casts datarecordInternal to POCO object defined in CalibrationInstructionsManager.Core.Models.Parameters
        /// </summary>
        public LinkedList<MeasurementPointParameters> GetMeasurementPointValueTypeParameters()
        {
            var measurementPointTemplates = new LinkedList<MeasurementPointParameters>();
            string queryStatement = @"  
                                        SELECT mk.idmkonfig AS templateId, kl.wert AS parameterDefaultValue, mkv.beschreibung AS valueDescription, mkt.name AS parameterName, mkt.idmkonfigtyp AS typeId
                                        FROM konfigliste kl
                                        JOIN mkonfig mk
                                        ON mk.idmkonfig = kl.mkonfig_id
                                        JOIN mkonfigtyp mkt
                                        ON mkt.idmkonfigtyp = kl.mkonfigtyp_id
                                        JOIN mkonfigvalues mkv
                                        ON mkv.idmkonfigvalues = kl.mkonfigvalues_id
                                        ORDER BY kl.mkonfig_id";
            IsConnectionEstablished();

            if (_isConnected)
            {
                try
                {
                    using (var connection = new NpgsqlConnection(_connectionString))
                    {
                        using (var command = new NpgsqlCommand(queryStatement, connection))
                        {
                            connection.Open();
                            using (NpgsqlDataReader dataReader = command.ExecuteReader())
                            {

                                var indexTemplateId = dataReader.GetOrdinal("templateId");
                                var indexTypeId = dataReader.GetOrdinal("typeId");
                                var indexParameterName = dataReader.GetOrdinal("parameterName");
                                var indexDefaultValue = dataReader.GetOrdinal("parameterDefaultValue");
                                var indexValueDescription = dataReader.GetOrdinal("valueDescription");

                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        var measurementPointValueType = new MeasurementPointParameters();

                                        measurementPointValueType.TemplateId = dataReader.GetInt32(indexTemplateId);
                                        measurementPointValueType.TypeId = dataReader.GetInt32(indexTypeId);
                                        measurementPointValueType.ParameterName = dataReader.GetString(indexParameterName);
                                        if (dataReader.IsDBNull(indexDefaultValue))
                                        {
                                            Console.WriteLine("Value is null");
                                        }
                                        else
                                        {
                                            measurementPointValueType.DefaultValue = dataReader.GetDouble(indexDefaultValue);
                                        }
                                        measurementPointValueType.ValueDescription = dataReader.GetString(indexValueDescription);

                                        measurementPointTemplates.AddLast(measurementPointValueType);

                                    }
                                }
                            }
                        }

                        connection.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return measurementPointTemplates;
        }

        /// <summary>
        /// Queries the table idkvorlage to get corresponding attributes of each channel setting dataset
        /// Casts datarecordInternal to POCO object defined in CalibrationInstructionsManager.Core.Models.Templates
        /// </summary>
        /// <returns></returns>

        public LinkedList<ChannelSettingTemplate> GetChannelSettingTemplates()
        {
            var channelSettingTemplateCatalog = new LinkedList<ChannelSettingTemplate>();
            string queryStatement = @" 
                                        SELECT idkvorlage, name
                                        FROM kvorlage
                                        ORDER BY idkvorlage";
            IsConnectionEstablished();

            if (_isConnected)
            {
                try
                {
                    using (var connection = new NpgsqlConnection(_connectionString))
                    {
                        using (var command = new NpgsqlCommand(queryStatement, connection))
                        {
                            connection.Open();
                            using (NpgsqlDataReader dataReader = command.ExecuteReader())
                            {
                                var indexId = dataReader.GetOrdinal("idkvorlage");
                                var indexName = dataReader.GetOrdinal("name");

                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        var channelSettingTemplate = new ChannelSettingTemplate();

                                        channelSettingTemplate.Id = (int)dataReader.GetValue(indexId);
                                        channelSettingTemplate.FullName = dataReader.GetValue(indexName) as string;

                                        channelSettingTemplateCatalog.AddLast(channelSettingTemplate);
                                    }
                                }
                            }
                        }

                        connection.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return channelSettingTemplateCatalog;
        }

        /// <summary>
        /// Queries the table mpunkt, kvorlage, vorgabetyp to get corresponding attributes of each channel setting dataset
        /// Casts datarecordInternal to POCO object defined in CalibrationInstructionsManager.Core.Models.Parameters
        /// </summary>
        public LinkedList<ChannelSettingParameters> GetChannelSettingParameters()
        {
            var channelSettingParameterCatalog = new LinkedList<ChannelSettingParameters>();
            string queryStatement = @"  
                                        SELECT mp.idmpunkt AS parameterId, kv.idkvorlage AS templateId, mp.vorgabe AS defaultValue, mp.unsicherheit AS uncertaintyValue, 
                                            mp.index AS parameterIndex, mp.anzahl AS parameterQuantity, vt.name AS typeName, vt.idvorgabetyp AS typeId
                                        FROM mpunkt mp
                                        JOIN kvorlage kv
                                        ON kv.idkvorlage = mp.kvorlage_id
                                        JOIN vorgabetyp vt
                                        ON vt.idvorgabetyp = mp.vorgabetyp_id";
            IsConnectionEstablished();

            if (_isConnected)
            {
                try
                {
                    using (var connection = new NpgsqlConnection(_connectionString))
                    {
                        using (var command = new NpgsqlCommand(queryStatement, connection))
                        {
                            connection.Open();
                            using (NpgsqlDataReader dataReader = command.ExecuteReader())
                            {

                                var indexParameterId = dataReader.GetOrdinal("parameterId");
                                var indexTemplateId = dataReader.GetOrdinal("templateId");
                                var indexDefaultValue = dataReader.GetOrdinal("defaultValue");
                                var indexUncertaintyValue = dataReader.GetOrdinal("uncertaintyValue");
                                var indexParameterIndex = dataReader.GetOrdinal("parameterIndex");
                                var indexParameterQuantity = dataReader.GetOrdinal("parameterQuantity");
                                var indexTypeName = dataReader.GetOrdinal("typeName");
                                var indexTypeId = dataReader.GetOrdinal("typeId");

                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        var channelSettingParameter = new ChannelSettingParameters();

                                        channelSettingParameter.ParameterId = dataReader.GetInt32(indexParameterId);
                                        channelSettingParameter.TemplateId = dataReader.GetInt32(indexTemplateId);
                                        channelSettingParameter.ParameterIndex = dataReader.GetInt32(indexParameterIndex);
                                        channelSettingParameter.ParameterQuantity = dataReader.GetInt32(indexParameterQuantity);
                                        channelSettingParameter.TypeName = dataReader.GetString(indexTypeName);
                                        channelSettingParameter.TypeId = dataReader.GetInt32(indexTypeId);

                                        if (dataReader.IsDBNull(indexDefaultValue))
                                        {
                                            Console.WriteLine("Default Value is null");
                                        }
                                        else
                                        {
                                            channelSettingParameter.DefaultValue = dataReader.GetDouble(indexDefaultValue);
                                        }

                                        if (dataReader.IsDBNull(indexUncertaintyValue))
                                        {
                                            Console.WriteLine("Uncertainty Value is null");
                                        }
                                        else
                                        {
                                            channelSettingParameter.UncertaintyValue = dataReader.GetDouble(indexUncertaintyValue);
                                        }

                                        channelSettingParameterCatalog.AddLast(channelSettingParameter);
                                    }
                                }
                            }
                        }

                        connection.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return channelSettingParameterCatalog;
        }

        public LinkedList<IChannelSettingType> GetChannelSettingTypes()
        {
            var channelSettingTypeCatalog = new LinkedList<IChannelSettingType>();
            string queryStatement = @"
                                        SELECT idvorgabetyp, name 
                                        FROM vorgabetyp
                                        ORDER BY idvorgabetyp";

            IsConnectionEstablished();

            if (_isConnected)
            {
                try
                {
                    using (var connection = new NpgsqlConnection(_connectionString))
                    {
                        using (var command = new NpgsqlCommand(queryStatement, connection))
                        {
                            connection.Open();
                            using (NpgsqlDataReader dataReader = command.ExecuteReader())
                            {

                                var indexId = dataReader.GetOrdinal("idvorgabetyp");
                                var indexName = dataReader.GetOrdinal("name");

                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        var channelSettingType = new ChannelSettingType();

                                        channelSettingType.Id = dataReader.GetInt32(indexId);
                                        channelSettingType.Name = dataReader.GetString(indexName);
                                    }
                                }
                            }
                            connection.Close();
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return channelSettingTypeCatalog;
        }

        public void CopyExistingChannelSettingTemplate(ChannelSettingTemplate template)
        {
            string updateCommand = @"INSERT INTO kvorlage (idkvorlage, name) VALUES (@id, @fullName);";

            IsConnectionEstablished();

            if (_isConnected)
            {
                try
                {
                    using (var connection = new NpgsqlConnection(_connectionString))
                    {
                        using (var command = new NpgsqlCommand(updateCommand, connection))
                        {
                            connection.Open();

                            var parameterId = command.CreateParameter();
                            parameterId.ParameterName = "id";
                            parameterId.Value = template.Id;
                            command.Parameters.Add(parameterId);

                            var parameterFullName = command.CreateParameter();
                            parameterFullName.ParameterName = "fullName";
                            parameterFullName.Value = template.FullName;
                            command.Parameters.Add(parameterFullName);

                            command.ExecuteNonQuery();

                            connection.Close();
                        }
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        // TODO: Make selectStatement more performantive with WHERE kv.idkvorlage = @selectedId
        public LinkedList<ChannelSettingParameters> GetSelectedChannelSettingParameters(int? selectedId)
        {
            var selectedChannelSettingParameters = new LinkedList<ChannelSettingParameters>();
            string queryStatement = @"  
                                        SELECT mp.idmpunkt AS parameterId, kv.idkvorlage AS templateId, mp.vorgabe AS defaultValue, mp.unsicherheit AS uncertaintyValue, 
                                            mp.index AS parameterIndex, mp.anzahl AS parameterQuantity, vt.name AS typeName, vt.idvorgabetyp AS typeId
                                        FROM mpunkt mp
                                        JOIN kvorlage kv
                                        ON kv.idkvorlage = mp.kvorlage_id
                                        JOIN vorgabetyp vt
                                        ON vt.idvorgabetyp = mp.vorgabetyp_id";
            IsConnectionEstablished();

            if (_isConnected)
            {
                try
                {
                    using (var connection = new NpgsqlConnection(_connectionString))
                    {
                        using (var command = new NpgsqlCommand(queryStatement, connection))
                        {
                            connection.Open();
                            using (NpgsqlDataReader dataReader = command.ExecuteReader())
                            {

                                var indexParameterId = dataReader.GetOrdinal("parameterId");
                                var indexTemplateId = dataReader.GetOrdinal("templateId");
                                var indexDefaultValue = dataReader.GetOrdinal("defaultValue");
                                var indexUncertaintyValue = dataReader.GetOrdinal("uncertaintyValue");
                                var indexParameterIndex = dataReader.GetOrdinal("parameterIndex");
                                var indexParameterQuantity = dataReader.GetOrdinal("parameterQuantity");
                                var indexTypeName = dataReader.GetOrdinal("typeName");
                                var indexTypeId = dataReader.GetOrdinal("typeId");

                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        var channelSettingParameter = new ChannelSettingParameters();

                                        channelSettingParameter.ParameterId = dataReader.GetInt32(indexParameterId);
                                        channelSettingParameter.TemplateId = dataReader.GetInt32(indexTemplateId);
                                        channelSettingParameter.ParameterIndex =
                                            dataReader.GetInt32(indexParameterIndex);
                                        channelSettingParameter.ParameterQuantity =
                                            dataReader.GetInt32(indexParameterQuantity);
                                        channelSettingParameter.TypeName = dataReader.GetString(indexTypeName);
                                        channelSettingParameter.TypeId = dataReader.GetInt32(indexTypeId);

                                        if (dataReader.IsDBNull(indexDefaultValue))
                                        {
                                            Console.WriteLine("Default Value is null");
                                        }
                                        else
                                        {
                                            channelSettingParameter.DefaultValue =
                                                dataReader.GetDouble(indexDefaultValue);
                                        }

                                        if (dataReader.IsDBNull(indexUncertaintyValue))
                                        {
                                            Console.WriteLine("Uncertainty Value is null");
                                        }
                                        else
                                        {
                                            channelSettingParameter.UncertaintyValue =
                                                dataReader.GetDouble(indexUncertaintyValue);
                                        }

                                        if (channelSettingParameter.TemplateId == selectedId)
                                            selectedChannelSettingParameters.AddLast(channelSettingParameter);
                                    }
                                }
                            }
                        }

                        connection.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return selectedChannelSettingParameters;
        }

        public void CopyExistingChannelSettingParameters(ChannelSettingParameters parameters)
        {
            string updateCommand = @"INSERT INTO mpunkt (idmpunkt, kvorlage_id, vorgabe, unsicherheit, index, anzahl, vorgabetyp_id) VALUES (@parameterId, @templateId, @defaultValue, @uncertaintyValue, @parameterIndex, @parameterQuantity, @typeId);";

            IsConnectionEstablished();

            if (_isConnected)
            {
                try
                {
                    using (var connection = new NpgsqlConnection(_connectionString))
                    {
                        using (var command = new NpgsqlCommand(updateCommand, connection))
                        {
                            connection.Open();

                            var parameterId = command.CreateParameter();
                            parameterId.ParameterName = "parameterId";
                            parameterId.Value = parameters.ParameterId;
                            if (parameterId.Value == null)
                                Console.WriteLine("parameter id is null");
                            else
                                command.Parameters.Add(parameterId);

                            var templateId = command.CreateParameter();
                            templateId.ParameterName = "templateId";
                            templateId.Value = parameters.TemplateId;
                            if (templateId.Value == null)
                                Console.WriteLine("template is null");
                            else
                                command.Parameters.Add(templateId);

                            var defaultValue = command.CreateParameter();
                            defaultValue.ParameterName = "defaultValue";
                            defaultValue.Value = parameters.DefaultValue;
                            if(defaultValue.Value == null)
                                Console.WriteLine("value is null");
                            else
                                command.Parameters.Add(defaultValue);

                            var uncertaintyValue = command.CreateParameter();
                            uncertaintyValue.ParameterName = "uncertaintyValue";
                            uncertaintyValue.Value = parameters.UncertaintyValue;
                            if(uncertaintyValue.Value == null)
                                Console.WriteLine("uncertainty is null");
                            else 
                                command.Parameters.Add(uncertaintyValue);

                            var parameterIndex = command.CreateParameter();
                            parameterIndex.ParameterName = "parameterIndex";
                            parameterIndex.Value = parameters.ParameterIndex;
                            command.Parameters.Add(parameterIndex);

                            var parameterQuantity = command.CreateParameter();
                            parameterQuantity.ParameterName = "parameterQuantity";
                            parameterQuantity.Value = parameters.ParameterQuantity;
                            command.Parameters.Add(parameterQuantity);

                            var typeId = command.CreateParameter();
                            typeId.ParameterName = "typeId";
                            typeId.Value = parameters.TypeId;
                            command.Parameters.Add(typeId);
                            
                            command.ExecuteNonQuery();

                            connection.Close();
                        }
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }



        // TODO: Implement Parameter Copying 
    }
}
