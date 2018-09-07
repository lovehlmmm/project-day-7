using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class ConfigHelper
    {
        private Configuration _config;

        public ConfigHelper()
        {
            _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public string GetConnectionString(string key)
        {
            return _config.ConnectionStrings.ConnectionStrings[key].ConnectionString;
        }

        public void SaveConnectionString(string key, string value)
        {
            _config.ConnectionStrings.ConnectionStrings[key].ConnectionString = value;
            _config.ConnectionStrings.ConnectionStrings[key].ProviderName = "System.Data.SqlClient";
            _config.Save(ConfigurationSaveMode.Modified);
        }

        public string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
        public void SaveAppSetting(string key,string value)
        {
            _config.AppSettings.Settings[key].Value = value;
            _config.Save(ConfigurationSaveMode.Modified);

        }
    }
}
