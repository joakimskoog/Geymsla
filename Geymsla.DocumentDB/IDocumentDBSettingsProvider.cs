using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geymsla.DocumentDB
{
    public interface IDocumentDBSettingsProvider
    {
        string AuthorizationKey { get; }
        Uri EndpointUrl { get; }
        string DatabaseIdentifier { get; }
    }
}
