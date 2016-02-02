using System;

namespace Geymsla.DocumentDB
{
    public interface IDocumentDBSettingsProvider
    {
        string AuthorizationKey { get; }
        Uri EndpointUrl { get; }
        string DatabaseIdentifier { get; }
    }
}
