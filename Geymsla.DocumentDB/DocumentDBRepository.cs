﻿using Microsoft.Azure.Documents;
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
                    var db = Client.CreateDatabaseQuery().FirstOrDefault(d => d.Id == Settings.DatabaseIdentifier);

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
                        .FirstOrDefault(c => c.Id == _collectionIdentifier);

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

        public IQueryable<T> GetAllAsQueryable(params Expression<Func<T, object>>[] includeProperties)
        {
            return DocumentDBRepository.Client.CreateDocumentQuery<T>(Collection.DocumentsLink);
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