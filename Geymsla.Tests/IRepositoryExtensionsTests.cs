using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Geymsla.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
// ReSharper disable InconsistentNaming

namespace Geymsla.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class IRepositoryExtensionsTests
    {
        #region GetAllAsync

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GivenThatRepositoryIsNull_WhemGetAllAsync_ThenArgumentNullExceptionIsThrown()
        {
            IRepository<Data, int> repo = null;
            await repo.GetAllAsync();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GivenThatRepositoryIsNull_WhemGetAllAsyncWithCancellationToken_ThenArgumentNullExceptionIsThrown()
        {
            IRepository<Data, int> repo = null;
            await repo.GetAllAsync(new CancellationToken(false));
        }

        [TestMethod]
        public async Task GivenThatNoItemsExists_WhenGetAllAsync_ThenReturnedListIsEmpty()
        {
            var repo = MockRepository.GenerateMock<IRepository<Data, int>>();
            repo.Stub(x => x.GetAsync(Arg<Func<IQueryable<Data>, IQueryable<Data>>>.Is.Anything, Arg<CancellationToken>.Is.Anything, Arg<Expression<Func<Data, object>>[]>.Is.Anything))
                .Return(Task.FromResult(Enumerable.Empty<Data>()));

            var items = await repo.GetAllAsync();

            Assert.AreEqual(0, items.Count());
        }

        [TestMethod]
        public async Task GivenThatNoItemsExists_WhenGetAllAsyncWithCancellationToken_ThenReturnedListIsEmpty()
        {
            var repo = MockRepository.GenerateMock<IRepository<Data, int>>();
            repo.Stub(x => x.GetAsync(Arg<Func<IQueryable<Data>, IQueryable<Data>>>.Is.Anything, Arg<CancellationToken>.Is.Anything, Arg<Expression <Func <Data, object >>[]>.Is.Anything))
                .Return(Task.FromResult(Enumerable.Empty<Data>()));

            var items = await repo.GetAllAsync(new CancellationToken(false));

            Assert.AreEqual(0, items.Count());
        }

        [TestMethod]
        public async Task GivenThatOneItemExists_WhenGetAllAsync_ThenReturnedListContainsOne()
        {
            var repo = MockRepository.GenerateMock<IRepository<Data, int>>();
            repo.Stub(x => x.GetAsync(Arg<Func<IQueryable<Data>, IQueryable<Data>>>.Is.Anything, Arg<CancellationToken>.Is.Anything, Arg<Expression<Func<Data, object>>[]>.Is.Anything))
                .Return(Task.FromResult(CreateNumbers(1)));

            var items = await repo.GetAllAsync();

            Assert.AreEqual(1, items.Count());
        }

        [TestMethod]
        public async Task GivenThatTenItemsExists_WhenGetAllAsync_ThenReturnedListContainsTen()
        {
            var repo = MockRepository.GenerateMock<IRepository<Data, int>>();
            repo.Stub(x => x.GetAsync(Arg<Func<IQueryable<Data>, IQueryable<Data>>>.Is.Anything, Arg<CancellationToken>.Is.Anything, Arg<Expression<Func<Data, object>>[]>.Is.Anything))
                .Return(Task.FromResult(CreateNumbers(10)));

            var items = await repo.GetAllAsync();

            Assert.AreEqual(10, items.Count());
        }

        #endregion

        #region FindByAsync

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GivenThatRepositoryIsNull_WhenFindByAsync_ThenArgumentNullExceptionIsThrown()
        {
            IRepository<Data, int> repo = null;
            var items = await repo.FindByAsync(x => x.Number == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GivenThatRepositoryIsNull_WhenFindByAsyncWithCancellationToken_ThenArgumentNullExceptionIsThrown()
        {
            IRepository<Data, int> repo = null;
            var items = await repo.FindByAsync(x => x.Number == 0, new CancellationToken(false));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GivenThatPredicateIsNull_WhenFindByAsync_ThenArgumentNullExceptionIsThrown()
        {
            IRepository<Data, int> repo = MockRepository.GenerateMock<IRepository<Data, int>>();
            var items = await repo.FindByAsync(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GivenThatPredicateIsNull_WhenFindByAsyncWithCancellationToken_ThenArgumentNullExceptionIsThrown()
        {
            IRepository<Data, int> repo = MockRepository.GenerateMock<IRepository<Data, int>>();
            var items = await repo.FindByAsync(null, new CancellationToken(false));
        }

        [TestMethod]
        public async Task GivenThatFiveItemsExistsWithOneItemWithNumberOne_WhenFindByAsyncWhereItemsHaveNumberOne_ThenReturnedListContainsOne()
        {
            var repo = new FakeRepository<Data>(CreateNumbers(5));

            var items = await repo.FindByAsync(x => x.Number == 1);

            Assert.AreEqual(1, items.Count());
        }

        [TestMethod]
        public async Task GivenThatFiveItemsExistsWithOneItemWithNumberOne_WhenFindByAsyncWhereItemsHaveNumberOneWithCancellationToken_ThenReturnedListContainsOne()
        {
            var repo = new FakeRepository<Data>(CreateNumbers(5));

            var items = await repo.FindByAsync(x => x.Number == 1, new CancellationToken(false));

            Assert.AreEqual(1, items.Count());
        }


        #endregion

        #region GetFirstOrDefaultAsync

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GivenThatRepositoryIsNull_WhenGetFirstOrDefaultAsync_ThenArgumentNullExceptionIsThrown()
        {
            IRepository<Data, int> repo = null;
            await repo.GetFirstOrDefaultAsync();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GivenThatRepositoryIsNull_WhenGetFirstOrDefaultAsyncWithCancellationToken_ThenArgumentNullExceptionIsThrown()
        {
            IRepository<Data, int> repo = null;
            await repo.GetFirstOrDefaultAsync(new CancellationToken(false));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GivenThatRepositoryIsNull_WhenGetFirstOrDefaultAsyncWithPredicate_ThenArgumentNullExceptionIsThrown()
        {
            IRepository<Data, int> repo = null;
            Expression<Func<Data, bool>> predicate = x => true;
            await repo.GetFirstOrDefaultAsync(predicate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GivenThatRepositoryIsNull_WhenGetFirstOrDefaultAsyncWithPredicateAndCancellationToken_ThenArgumentNullExceptionIsThrown()
        {
            IRepository<Data, int> repo = null;
            Expression<Func<Data, bool>> predicate = x => true;
            await repo.GetFirstOrDefaultAsync(predicate, new CancellationToken(false));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GivenThatPredicateIsNull_WhenGetFirstOrDefaultAsyncWithPredicate_ThenArgumentNullExceptionIsThrown()
        {
            IRepository<Data, int> repo = MockRepository.GenerateMock<IRepository<Data, int>>();
            Expression<Func<Data, bool>> predicate = null;
            await repo.GetFirstOrDefaultAsync(predicate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GivenThatPredicateIsNull_WhenGetFirstOrDefaultAsyncWithPredicateAndCancellationToken_ThenArgumentNullExceptionIsThrown()
        {
            IRepository<Data, int> repo = MockRepository.GenerateMock<IRepository<Data, int>>();
            Expression<Func<Data, bool>> predicate = null;
            await repo.GetFirstOrDefaultAsync(predicate, new CancellationToken(false));
        }

        [TestMethod]
        public async Task GivenThatNoItemsExists_WhenGetFirstOrDefaultAsync_ThenReturnedItemIsNull()
        {
            var repo = MockRepository.GenerateMock<IRepository<Data, int>>();
            Func<IQueryable<Data>, IQueryable<Data>> queryFilter = null;
            repo.Stub(x => x.GetAsync(queryFilter, CancellationToken.None)).IgnoreArguments().Return(Task.FromResult(Enumerable.Empty<Data>()));

            var item = await repo.GetFirstOrDefaultAsync();

            Assert.IsNull(item);
        }

        [TestMethod]
        public async Task GivenThatOneItemExists_WhenGetFirstOrDefaultAsyncWithNoPredicate_ThenReturnedItemIsNotNull()
        {
            var repo = new FakeRepository<Data>(new List<Data> { new Data(1) });

            var item = await repo.GetFirstOrDefaultAsync();

            Assert.IsNotNull(item);
        }

        [TestMethod]
        public async Task GivenThatFiveItemsExists_WhenGetFirstOrDefaultAsyncWithPredicateThatReturnsOne_ThenReturnedItemIsNotNull()
        {
            var repo = new FakeRepository<Data>(CreateNumbers(5));

            var item = await repo.GetFirstOrDefaultAsync(x => x.Number == 1);

            Assert.IsNotNull(item);
        }

        [TestMethod]
        public async Task GivenThatTwoItemsExistsWithSameNumber_WhenGetFirstOrDefaultAsyncWithPredicateOnNumber_ThenFirstItemIsReturned()
        {
            var repo = new FakeRepository<Data>(new List<Data> { new Data(1) { Name = "first" }, new Data(1) { Name = "second" } });

            var item = await repo.GetFirstOrDefaultAsync(x => x.Number == 1);

            Assert.IsNotNull(item);
            Assert.AreEqual("first", item.Name);
        }

        #endregion

        #region GetSingleOrDefaultAsync

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GivenThatRepositoryIsNull_WhenGetSingleOrDefaultAsync_ThenArgumentNullExceptionIsThrown()
        {
            IRepository<Data, int> repo = null;
            await repo.GetSingleOrDefaultAsync();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GivenThatRepositoryIsNull_WhenGetSingleOrDefaultAsyncWithCancellationToken_ThenArgumentNullExceptionIsThrown()
        {
            IRepository<Data, int> repo = null;
            await repo.GetSingleOrDefaultAsync(new CancellationToken(false));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GivenThatRepositoryIsNull_WhenGetSingleOrDefaultAsyncWithPredicate_ThenArgumentNullExceptionIsThrown()
        {
            IRepository<Data, int> repo = null;
            await repo.GetSingleOrDefaultAsync(x => true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GivenThatRepositoryIsNull_WhenGetSingleOrDefaultAsyncWithPredicateAndCancellationToken_ThenArgumentNullExceptionIsThrown()
        {
            IRepository<Data, int> repo = null;
            await repo.GetSingleOrDefaultAsync(x => true, new CancellationToken(false));
        }

        [TestMethod]
        public async Task GivenThatNoItemsExists_WhenGetSingleOrDefaultAsync_ThenReturnedItemIsNull()
        {
            var repo = MockRepository.GenerateMock<IRepository<Data, int>>();
            repo.Stub(x => x.GetAsync(Arg<Func<IQueryable<Data>, IQueryable<Data>>>.Is.Anything, Arg<CancellationToken>.Is.Anything, Arg<Expression<Func<Data, object>>[]>.Is.Anything))
                .IgnoreArguments()
                .Return(Task.FromResult(Enumerable.Empty<Data>()));

            var item = await repo.GetSingleOrDefaultAsync();

            Assert.IsNull(item);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GivenThatTwoItemsExists_WhenGetSingleOrDefaultAsyncWithNoPredicate_ThenInvalidOperationExceptionIsThrown()
        {
            var repo = MockRepository.GenerateMock<IRepository<Data, int>>();
            repo.Stub(x => x.GetAsync(Arg<Func<IQueryable<Data>, IQueryable<Data>>>.Is.Anything, Arg<CancellationToken>.Is.Anything))
                .IgnoreArguments()
                .Return(Task.FromResult(CreateNumbers(2)));

            var item = await repo.GetSingleOrDefaultAsync();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GivenThatSeveralItemsMatchesTheCondition_WhenGetSingleOrDefaultAsync_ThenInvalidOperationExceptionIsThrown()
        {
            var repo = new FakeRepository<Data>(CreateNumbers(10));

            var item = await repo.GetSingleOrDefaultAsync(x => x.Number > 5);
        }

        [TestMethod]
        public async Task GivenThatOneItemMatchesTheCondition_WhenGetSingleOrDefaultAsync_ThenReturnedItemIsCorrect()
        {
            var repo = new FakeRepository<Data>(new List<Data> { new Data(1) { Name = "first" }, new Data(2) { Name = "second" } });

            var item = await repo.GetSingleOrDefaultAsync(x => x.Number == 2);

            Assert.IsNotNull(item);
            Assert.AreEqual("second", item.Name);
        }

        #endregion

        private IEnumerable<Data> CreateNumbers(int numberOfItems)
        {
            var numbers = new List<Data>();
            for (int i = 0; i < numberOfItems; i++)
            {
                numbers.Add(new Data(i));
            }

            return numbers;
        }
    }

    [ExcludeFromCodeCoverage]
    public class FakeRepository<T> : IRepository<T, int> where T : class
    {
        private readonly IEnumerable<T> _data;

        public FakeRepository(IEnumerable<T> data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            _data = data;
        }

        public T Get(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(int id, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Get(Func<IQueryable<T>, IQueryable<T>> queryFilter, params Expression<Func<T, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAsync(Func<IQueryable<T>, IQueryable<T>> queryFilter, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = queryFilter(_data.AsQueryable());
            return Task.FromResult(query.AsEnumerable());
        }

        public Task<IPagedList<T>> GetPaginatedListAsync(Func<IQueryable<T>, IQueryable<T>> queryFilter, int pageNumber, int pageSize, CancellationToken cancellationToken,
            params Expression<Func<T, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAllAsQueryable()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return _data.Count();
        }

        public IQueryable<T> GetAllAsQueryable(params Expression<Func<T, object>>[] includeProperties)
        {
            return _data.AsQueryable();
        }
    }

    [ExcludeFromCodeCoverage]
    public class Data
    {
        public int Number { get; set; }
        public string Name { get; set; }

        public Data(int number)
        {
            Number = number;
        }
    }
}