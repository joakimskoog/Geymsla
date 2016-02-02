using System;
using System.Collections.Generic;
using System.Linq;

namespace Geymsla.Collections
{
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        private readonly PaginationData _paginationData;

        public int PageNumber => _paginationData.PageNumber;
        public int? PreviousPageNumber => _paginationData.PreviousPageNumber;
        public int? NextPageNumber => _paginationData.NextPageNumber;
        public int FirstPageNumber => _paginationData.FirstPageNumber;
        public int LastPageNumber => _paginationData.LastPageNumber;
        public int PageSize => _paginationData.PageSize;
        public int PageCount => _paginationData.PageCount;

        public PagedList(PaginationData paginationData, IEnumerable<T> paginatedSet)
        {
            if (paginationData == null) throw new ArgumentNullException(nameof(paginationData));
            if (paginatedSet == null) throw new ArgumentNullException(nameof(paginatedSet));
            _paginationData = paginationData;

            AddRange(paginatedSet);
        }

        public PagedList(IQueryable<T> superset, int pageNumber, int pageSize)
        {
            if (superset == null) throw new ArgumentNullException(nameof(superset));
            _paginationData = new PaginationData(superset.Count(), pageNumber, pageSize);

            var subset = superset.Skip((PageNumber - 1) * PageSize).Take(PageSize);
            AddRange(subset);
        }


        public IPagedList<TOutput> ConvertTo<TOutput>(Converter<T, TOutput> converter)
        {
            var convertedList = this.Select(x => converter(x));

            return new PagedList<TOutput>(_paginationData, convertedList);
        }
    }
}