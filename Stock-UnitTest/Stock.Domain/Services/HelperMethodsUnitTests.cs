using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using Stock.Domain.Services;
using System.Collections.Generic;
using Stock.Domain.Enums;

namespace Stock_UnitTest.Stock.Services
{


    [TestClass]
    public class HelperMethodsUnitTests
    {


        #region getEarliestDate
        [TestMethod]
        [TestCategory("getEarliestDate")]
        public void getEarliestDate_if_at_least_one_date_is_null_function_returns_null()
        {
            DateTime?[] dates = new DateTime?[] {   new DateTime(2016, 1, 1, 0, 0, 0), 
                                                    new DateTime(2015, 1, 1, 0, 0, 0), 
                                                    new DateTime(2016, 2, 2, 0, 0, 0), 
                                                    null,
                                                    new DateTime(2016, 3, 3, 0, 0, 0)};
            List<DateTime?> list = new List<DateTime?>(dates);
            var earliest = list.getEarliestDate();
            Assert.IsNull(earliest);

        }


        [TestMethod]
        [TestCategory("getEarliestDate")]
        public void getEarliestDate_if_no_item_is_null_function_returns_earliest_date()
        {
            DateTime?[] dates = new DateTime?[] {   new DateTime(2016, 1, 1, 0, 0, 0), 
                                                    new DateTime(2015, 1, 1, 0, 0, 0), 
                                                    new DateTime(2016, 2, 2, 0, 0, 0), 
                                                    new DateTime(2016, 3, 3, 0, 0, 0)};
            List<DateTime?> list = new List<DateTime?>(dates);
            var earliest = list.getEarliestDate();
            Assert.AreEqual(earliest, new DateTime(2015, 1, 1, 0, 0, 0));
        }
        #endregion getEarliestDate


    }
}
