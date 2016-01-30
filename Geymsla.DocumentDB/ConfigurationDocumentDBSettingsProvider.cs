using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geymsla.DocumentDB
{
    public class ConfigurationDocumentDBSettingsProvider : IDocumentDBSettingsProvider
    {
        public string AuthorizationKey
        {
            get
            {
                return ConfigurationManager.AppSettings["DocumentDBAuthorizationKey"];
            }
        }

        public string DatabaseIdentifier
        {
            get
            {
                return ConfigurationManager.AppSettings["DocumentDBDatabaseIdentifier"];
            }
        }

        public Uri EndpointUrl
        {
            get
            {
               return new Uri(ConfigurationManager.AppSettings["DocumentDBAuthorizationKey"]);
            }
        }
    }
}
