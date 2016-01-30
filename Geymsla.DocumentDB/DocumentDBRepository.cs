using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Geymsla.DocumentDB
{
    public class DocumentDBRepository<T, TId> : IRepository<T, TId> where T : class
    {
        private static object _lock = new object();

        private static IDocumentDBSettingsProvider _settings;
        private static IDocumentDBSettingsProvider Settings
        {
            get
            {
                lock (_lock)
                {
                    if (_settings == null)
                    {
                        _settings = new ConfigurationDocumentDBSettingsProvider();
                    }

                    return _settings;
                }
            }
            set
            {
                lock (_lock)
                {
                    _settings = value;
                }
            }
        }

        private static Database _database;
        private static Database Database
        {
            get
            {
                if (_database == null)
                {
                    var db = Client.CreateDatabaseQuery().Where(d => d.Id == Settings.DatabaseIdentifier).FirstOrDefault();

                    if (db == null)
                    {
                        db = Client.CreateDatabaseAsync(new Database { Id = Settings.DatabaseIdentifier }).Result;
                    }

                    _database = db;
                }

                return _database;
            }
        }

        private static DocumentClient _client;
        private static DocumentClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new DocumentClient(Settings.EndpointUrl, Settings.AuthorizationKey);
                }

                return _client;
            }
        }

        private string _collectionIdentifier;
        private DocumentCollection _collection;
        private DocumentCollection Collection
        {
            get
            {
                if (_collection == null)
                {
                    var col = Client.CreateDocumentCollectionQuery(Database.SelfLink)
                        .Where(c => c.Id == _collectionIdentifier)
                        .FirstOrDefault();

                    if (col == null)
                    {
                        col = Client.CreateDocumentCollectionAsync(Database.SelfLink, new DocumentCollection { Id = _collectionIdentifier }).Result;
                    }

                    _collection = col;
                }

                return _collection;
            }
        }

        public DocumentDBRepository(string collectionIdentifier)
        {
            if (collectionIdentifier == null) throw new ArgumentNullException(nameof(collectionIdentifier));
            _collectionIdentifier = collectionIdentifier;
        }

        public IQueryable<T> GetAllAsQueryable(params Expression<Func<T, object>>[] includeProperties)
        {
            return Client.CreateDocumentQuery<T>(Collection.DocumentsLink);
        }

        public async Task<IEnumerable<T>> GetAsync(Func<IQueryable<T>, IQueryable<T>> queryFilter, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties)
        {
            var queryable = GetAllAsQueryable(includeProperties);
            var filtered = queryFilter(queryable).AsDocumentQuery();

            var items = new List<T>();

            while (filtered.HasMoreResults)
            {
                var itemsToAdd = await filtered.ExecuteNextAsync<T>();
                items.AddRange(itemsToAdd);
            }

            return items;
        }
    }
}