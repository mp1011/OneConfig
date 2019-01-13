using Dapper;
using OneConfig.Models;
using OneConfig.Models.Exceptions;
using OneConfig.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace OneConfig.Services.ConfigurationReaders
{
    public class DatabaseConfigurationReader : IMultiConfigurationReader
    {
        private string _connectionString;
        public DatabaseConfigurationReader(string connectionString) 
        {
            _connectionString = connectionString;
        }
     
        private IDbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
        
        public string GetSingleValue(string key)
        {
            try
            {
                using (var connection = CreateConnection(_connectionString))
                {
                    var result = connection.Query<NameValue>("SELECT TOP(1) * FROM ConfigurationSettings WHERE Name=@Key", new { Key = key }).FirstOrDefault();
                    if (result == null)
                        return null;
                    else
                        return result.Value;
                }
            }
            catch (Exception e)
            {
                throw new ConfigurationException($"Unable to read database configuration. Error: {e.Message}", e);
            }
        }

        public Dictionary<string, string> GetAllValues()
        {
            var configValues = new Dictionary<string, string>();
            try
            {
                using (var connection = CreateConnection(_connectionString))
                {
                    foreach (var nameValue in connection.Query<NameValue>("SELECT * FROM ConfigurationSettings"))
                    {
                        configValues[nameValue.Name] = nameValue.Value;
                    }
                }

                return configValues;
            }
            catch (Exception e)
            {
                throw new ConfigurationException($"Unable to read database configuration. Error: {e.Message}", e);
            }
        }
    }
}
