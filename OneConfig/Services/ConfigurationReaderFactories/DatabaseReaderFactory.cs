using OneConfig.Services.ConfigurationReaders;
using OneConfig.Services.Interfaces;
using System;
#if NETSTD
using Microsoft.Data.SqlClient;
#else
using System.Data.SqlClient;
#endif

namespace OneConfig.Services.ConfigurationReaderFactories
{
    public class DatabaseReaderFactory : IConfigurationReaderFactory
    {  
        public IConfigurationReader TryParseReader(string text)
        {
            try
            {
                var builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = text;
                if (!String.IsNullOrEmpty(builder.InitialCatalog))
                    return new DatabaseConfigurationReader(text);
                else
                    return null;
            }
            catch(Exception e)
            {
                return null;
            }
        }        
    }
}
