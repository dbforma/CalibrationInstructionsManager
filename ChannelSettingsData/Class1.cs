using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalibrationInstructionsManager.Core.Models.Parameters;
using CalibrationInstructionsManager.Core.Models.Parameters.Contract;
using CalibrationInstructionsManager.Core.Models.Templates;
using Npgsql;

namespace ChannelSettingsData
{
    public class ChannelSettingsData
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
        /// Queries the table idkvorlage to get corresponding attributes of each channel setting dataset
        /// Casts datarecordInternal to POCO object defined in CalibrationInstructionsManager.Core.Models.Templates
        /// </summary>
        /// <returns></returns>

        public LinkedList<IChannelSettingTemplate> GetChannelSettingTemplates()
        {
            var channelSettingTemplateCatalog = new LinkedList<IChannelSettingTemplate>();
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
        public LinkedList<IChannelSettingParameters> GetChannelSettingParameters()
        {
            var channelSettingParameterCatalog = new LinkedList<IChannelSettingParameters>();
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
                                            channelSettingParameter.DefaultValue = null;
                                        }
                                        else
                                        {
                                            channelSettingParameter.DefaultValue = dataReader.GetDouble(indexDefaultValue);
                                        }

                                        if (dataReader.IsDBNull(indexUncertaintyValue))
                                        {
                                            channelSettingParameter.UncertaintyValue = null;
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

        


        public int CopyChannelSettingTemplate(IChannelSettingTemplate template)
        {
            string updateCommand = @"INSERT INTO kvorlage (name) VALUES (@fullName) RETURNING idkvorlage;";
            int generatedId = -1;

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

                            if (template.FullName != null)
                                command.Parameters.AddWithValue("@fullName", template.FullName);

                            generatedId = (int)command.ExecuteScalar();

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

            return generatedId;
        }

        // TODO: Make selectStatement more performantive with WHERE kv.idkvorlage = @selectedId
        public LinkedList<IChannelSettingParameters> GetSelectedChannelSettingParameters(int? selectedId)
        {
            var selectedChannelSettingParameters = new LinkedList<IChannelSettingParameters>();
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

                                        if (!dataReader.IsDBNull(indexDefaultValue))
                                        {
                                            channelSettingParameter.DefaultValue = dataReader.GetDouble(indexDefaultValue);
                                        }

                                        if (!dataReader.IsDBNull(indexUncertaintyValue))
                                        {
                                            channelSettingParameter.UncertaintyValue = dataReader.GetDouble(indexUncertaintyValue);
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

        public void CopyExistingChannelSettingParameters(IChannelSettingParameters parameter)
        {
            string updateCommand = @"INSERT INTO mpunkt (kvorlage_id, vorgabe, unsicherheit, index, anzahl, vorgabetyp_id) VALUES (@templateId, @defaultValue, @uncertaintyValue, @parameterIndex, @parameterQuantity, @typeId);";

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

                            if (parameter.TemplateId != null)
                                command.Parameters.AddWithValue("@templateId", parameter.TemplateId);

                            if (parameter.DefaultValue != null)
                                command.Parameters.AddWithValue("@defaultValue", parameter.DefaultValue);

                            if (parameter.UncertaintyValue != null)
                            {
                                command.Parameters.AddWithValue("@uncertaintyValue", parameter.UncertaintyValue);
                            }
                            else if (parameter.UncertaintyValue == null)
                            {
                                command.Parameters.AddWithValue("@uncertaintyValue", DBNull.Value);
                            }

                            if (parameter.ParameterIndex != null)
                                command.Parameters.AddWithValue("@parameterIndex", parameter.ParameterIndex);

                            if (parameter.ParameterQuantity != null)
                                command.Parameters.AddWithValue("@parameterQuantity", parameter.ParameterQuantity);

                            if (parameter.TypeId != null)
                                command.Parameters.AddWithValue("@typeId", parameter.TypeId);

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

    }
}
