using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Geymsla.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Geymsla.Tests.Collections
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PagedListTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenThatSupersetIsNull_WhenCreatingPagedList_ThenArgumentNullExceptionIsThrown()
        {
            var pagedList = new PagedList<int>(null, 1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GivenThatPageNumberIsSmallerThanOne_WhenCreatingPagedList_ThenArgumentOutOfRangeExceptionIsThrown()
        {
            var superset = new List<int> { 1, 2, 3 };
            var pagedList = new PagedList<int>(superset.AsQueryable(), 0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GivenThatPageSizeIsSmallerThanOne_WhenCreatingPagedList_ThenArgumentOutOfRangeExceptionIsThrown()
        {
            var superset = new List<int> { 1, 2, 3 };
            var pagedList = new PagedList<int>(superset.AsQueryable(), 1, 0);
        }

        [TestMethod]
        public void GivenThatQueryableIsEmpty_WhenCreatingPagedList_ThenFirstPageIsOneLastPageIsOnePageCountIsOneAndPageNummberIsOne()
        {
            var superset = new List<int>();
            var pagedList = new PagedList<int>(superset.AsQueryable(), 1, 10);

            Assert.AreEqual(1, pagedList.FirstPageNumber);
            Assert.AreEqual(1, pagedList.LastPageNumber);
            Assert.AreEqual(1, pagedList.PageCount);
            Assert.AreEqual(1, pagedList.PageNumber);
        }

        [TestMethod]
        public void GivenThatSupersetAndDataArgumentsAreCorrect_WhenCreatingPagedList_ThenPagedListPageParametersAreCorrect()
        {
            var superset = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var pagedList = new PagedList<int>(superset.AsQueryable(), 1, 10);

            Assert.AreEqual(1, pagedList.PageCount);
            Assert.AreEqual(1, pagedList.PageNumber);
            Assert.AreEqual(10, pagedList.PageSize);
        }

        [TestMethod]
        public void GivenPageNumberOneAndAPageSizeOfOne_WhenCreatingPagedList_ThenPagedListElementEqualsFirstElementInSuperset()
        {
            var superset = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var pagedList = new PagedList<int>(superset.AsQueryable(), 1, 1);

            Assert.AreEqual(1, pagedList.PageSize);
            Assert.AreEqual(superset[0], pagedList[0]);
        }

        [TestMethod]
        public void GivenThatPageIsFirstOne_WhenCreatingPagedList_ThenPreviousPageNumberIsNull()
        {
            var superset = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var pagedList = new PagedList<int>(superset.AsQueryable(), 1, 1);

            Assert.IsNull(pagedList.PreviousPageNumber);
        }

        [TestMethod]
        public void GivenThatPageIsSecondOne_WhenCreatingPagedList_ThenPreviousPageNumberIsOne()
        {
            var superset = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var pagedList = new PagedList<int>(superset.AsQueryable(), 2, 1);

            Assert.AreEqual(1, pagedList.PreviousPageNumber.Value);
        }

        [TestMethod]
        public void GivenThatPageIsLastOne_WhenCreatingPagedList_ThenNextPageNumberIsNull()
        {
            var superset = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var pagedList = new PagedList<int>(superset.AsQueryable(), 10, 1);

            Assert.IsNull(pagedList.NextPageNumber);
        }

        [TestMethod]
        public void GivenThatPageIsSecondOne_WhenCreatingPagedList_ThenNextPageNumberIsThree()
        {
            var superset = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var pagedList = new PagedList<int>(superset.AsQueryable(), 2, 1);

            Assert.AreEqual(3, pagedList.NextPageNumber.Value);
        }

        [TestMethod]
        public void GivenThatPageNumberIsOne_WhenCreatingPagedList_ThenFirstPageNumberIsOne()
        {
            var superset = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var pagedList = new PagedList<int>(superset.AsQueryable(), 1, 1);

            Assert.AreEqual(1, pagedList.FirstPageNumber);
        }

        [TestMethod]
        public void GivenThatPageNumberisFive_WhenCreatingPagedList_ThenFirstPageNumberIsOne()
        {
            var superset = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var pagedList = new PagedList<int>(superset.AsQueryable(), 5, 1);

            Assert.AreEqual(1, pagedList.FirstPageNumber);
        }

        [TestMethod]
        public void GivenThatPageNumberIsOneWithASupersetCountOfTenWithPageSizeOfOne_WhenCreatingPagedList_ThenLastPageNumberIsTen()
        {
            var superset = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var pagedList = new PagedList<int>(superset.AsQueryable(), 5, 1);

            Assert.AreEqual(superset.Count, pagedList.LastPageNumber);
        }

        [TestMethod]
        public void GivenPagedListOfIntegers_WhenConvertToDouble_ThenConvertedPagedListContainsDoubles()
        {
            var superset = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var pagedList = new PagedList<int>(superset.AsQueryable(), 5, 1);

            var convertedList = pagedList.ConvertTo(input => (double) input);

            Assert.AreEqual(pagedList.Count, convertedList.Count);
        }
    }
}