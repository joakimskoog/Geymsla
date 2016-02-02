using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private string _continuationToken;
        private readonly string _collectionIdentifier;
        private readonly IDocumentDBCache<T> _documentCache;

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

        public DocumentDBRepository(string collectionIdentifier, IDocumentDBCache<T> documentCache)
        {
            if (collectionIdentifier == null) throw new ArgumentNullException(nameof(collectionIdentifier));
            _collectionIdentifier = collectionIdentifier;
            _documentCache = documentCache;
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

        private int GetCount()
        {
            return DocumentDBRepository.Client.CreateDocumentQuery<T>(Collection.DocumentsLink, $"SELECT * FROM {_collectionIdentifier}").AsEnumerable().Count();
        }

        public async Task<IPagedList<T>> GetPaginatedListAsync(Func<IQueryable<T>, IQueryable<T>> queryFilter, int pageNumber, int pageSize, CancellationToken cancellationToken,
            params Expression<Func<T, object>>[] includeProperties)
        {
            if (_documentCache.IsExpired)
            {
                _documentCache.Clear();
                _continuationToken = string.Empty;
            }
           
            int count = GetCount(); //Maybe cache the count
            var paginationData = new PaginationData(count, pageNumber, pageSize);
            var numberOfItemsWanted = pageNumber * pageSize;
            var numberOfItemsToRetrieveFromDatabase = Math.Max(0, numberOfItemsWanted - _documentCache.Count);


            //If we already have the whole collection loaded into the cache, we should not try to hit the database
            if (count == _documentCache.Count)
            {
                numberOfItemsToRetrieveFromDatabase = 0;
            }

            //We need to retrieve items from the database and cache them
            if (numberOfItemsToRetrieveFromDatabase > 0)
            {
                var queryable = GetAllAsQueryable(new FeedOptions()
                {
                    MaxItemCount = numberOfItemsToRetrieveFromDatabase,
                    RequestContinuation = _continuationToken
                });
                var query = queryFilter(queryable).AsDocumentQuery();

                var itemsToAdd = await query.ExecuteNextAsync<T>();
                foreach (var itemToAdd in itemsToAdd)
                {
                    _documentCache.Add(itemToAdd);
                }

                _continuationToken = itemsToAdd.ResponseContinuation;
            }

            return new PagedList<T>(paginationData, _documentCache.GetContents().Skip((paginationData.PageNumber - 1) * paginationData.PageSize).Take(paginationData.PageSize));
        }

        private IQueryable<T> GetAllAsQueryable(FeedOptions feedOptions)
        {
            return DocumentDBRepository.Client.CreateDocumentQuery<T>(Collection.DocumentsLink, feedOptions: feedOptions);
        }

        public IQueryable<T> GetAllAsQueryable()
        {
            return DocumentDBRepository.Client.CreateDocumentQuery<T>(Collection.DocumentsLink);
        }
    }
}