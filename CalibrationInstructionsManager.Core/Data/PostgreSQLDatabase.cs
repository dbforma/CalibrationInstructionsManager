// Note: System.Threading.Tasks.Extensions must be <= 4.5.2

using System;
using System.Collections.Generic;
using System.Data;
using CalibrationInstructionsManager.Core.Models.ValueTypes;
using Npgsql;

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
        /// Casts datarecordInternal to POCO object defined in CalibrationInstructionsManager.Core.Models.DataTypes
        /// </summary>
        public LinkedList<DefaultConfigurationValueType> GetDefaultConfigurationValueTypeParameters()
        {
            var defaultConfigurationValueTypeCatalog = new LinkedList<DefaultConfigurationValueType>();
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
                                        var defaultConfigurationValueType = new DefaultConfigurationValueType();

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
    }
}
