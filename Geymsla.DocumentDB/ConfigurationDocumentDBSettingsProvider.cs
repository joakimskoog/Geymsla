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
        public string AuthorizationKey => ConfigurationManager.AppSettings["DocumentDBAuthorizationKey"];
        public string DatabaseIdentifier => ConfigurationManager.AppSettings["DocumentDBDatabaseIdentifier"];
        public Uri EndpointUrl => new Uri(ConfigurationManager.AppSettings["DocumentDBEndpointUrl"]);
    }
}
