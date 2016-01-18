using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
// ReSharper disable InconsistentNaming

namespace Geymsla.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class IRepositoryExtensionsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenThatRepositoryIsNull_WhenTryingToGetAll_ThenArgumentNullExceptionIsThrown()
        {
            IRepository<Data> repo = null;
            repo.GetAll();
        }

        [TestMethod]
        public void GivenThatZeroItemsExists_WhenTryingToGetAll_ThenReturnedListIsEmpty()
        {
            var repo = MockRepository.GenerateMock<IRepository<Data>>();
            repo.Stub(x => x.Get(Arg<Func<IQueryable<Data>, IQueryable<Data>>>.Is.Anything)).Return(Enumerable.Empty<Data>());

            var items = repo.GetAll();

            Assert.AreEqual(0, items.Count());
        }

        [TestMethod]
        public void GivenThatOneItemExists_WhenTryingToGetAll_ThenReturnedListContainsOneItem()
        {
            var repo = MockRepository.GenerateMock<IRepository<Data>>();
            repo.Stub(x => x.Get(Arg<Func<IQueryable<Data>, IQueryable<Data>>>.Is.Anything)).Return(CreateNumbers(1));

            var items = repo.GetAll();

            Assert.AreEqual(1, items.Count());
        }

        [TestMethod]
        public void GivenThatTenItemsExists_WhenTryingToGetAll_ThenReturnedListContainsTenItems()
        {
            var repo = MockRepository.GenerateMock<IRepository<Data>>();
            repo.Stub(x => x.Get(Arg<Func<IQueryable<Data>, IQueryable<Data>>>.Is.Anything)).Return(CreateNumbers(10));

            var items = repo.GetAll();

            Assert.AreEqual(10, items.Count());
        }

        private IList<Data> CreateNumbers(int numberOfItems)
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
    public class Data
    {
        public int Number { get; set; }

        public Data(int number)
        {
            Number = number;
        }
    }
}