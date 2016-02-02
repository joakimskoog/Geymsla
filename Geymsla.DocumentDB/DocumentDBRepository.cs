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
using Geymsla.Collections;

namespace Geymsla.DocumentDB
{
    public static class DocumentDBRepository
    {
        private static readonly object _lock = new object();

        private static IDocumentDBSettingsProvider _settings;
        public static IDocumentDBSettingsProvider Settings
        {
            get
            {
                lock (_lock)
                {
                    return _settings ?? (_settings = new ConfigurationDocumentDBSettingsProvider());
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
        public static Database Database
        {
            get
            {
                if (_database == null)
                {
                    var db = Client.CreateDatabaseQuery().Where(d => d.Id == Settings.DatabaseIdentifier).AsEnumerable().FirstOrDefault();

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
        public static DocumentClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new DocumentClient(Settings.EndpointUrl, Settings.AuthorizationKey)
                    {
                        ConnectionPolicy =
                        {
                            ConnectionMode = ConnectionMode.Direct,
                            ConnectionProtocol = Protocol.Tcp
                        }
                    };
                }

                return _client;
            }
        }
    }

    public class DocumentDBRepository<T, TId> : IRepository<T, TId> where T : class
    {
        private readonly string _collectionIdentifier;

        private DocumentCollection _collection;
        private DocumentCollection Collection
        {
            get
            {
                if (_collection == null)
                {
                    var col = DocumentDBRepository.Client
                        .CreateDocumentCollectionQuery(DocumentDBRepository.Database.SelfLink)
                        .Where(c => c.Id == _collectionIdentifier).AsEnumerable().FirstOrDefault();

                    if (col == null)
                    {
                        col = DocumentDBRepository.Client.CreateDocumentCollectionAsync(DocumentDBRepository.Database.SelfLink,
                            new DocumentCollection { Id = _collectionIdentifier }).Result;
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

        public async Task<IEnumerable<T>> GetAsync(Func<IQueryable<T>, IQueryable<T>> queryFilter, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties)
        {
            var queryable = GetAllAsQueryable();
            var filtered = queryFilter(queryable).AsDocumentQuery();

            var items = new List<T>();

            while (filtered.HasMoreResults)
            {
                var itemsToAdd = await filtered.ExecuteNextAsync<T>();
                items.AddRange(itemsToAdd);
            }

            return items;
        }

        public Task<IPagedList<T>> GetPaginatedListAsync(Func<IQueryable<T>, IQueryable<T>> queryFilter, int pageNumber, int pageSize, CancellationToken cancellationToken,
            params Expression<Func<T, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAllAsQueryable()
        {
            return DocumentDBRepository.Client.CreateDocumentQuery<T>(Collection.DocumentsLink, feedOptions: new FeedOptions()
            {
                MaxItemCount = DocumentDBRepository.Settings.MaxItemsInResponse
            });
        }
    }
}