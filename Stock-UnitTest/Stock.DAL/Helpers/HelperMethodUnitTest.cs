using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.DAL.Helpers;
using System.Data.Entity;
using Stock.DAL.Infrastructure;
using System.Linq;
using MySql.Data.MySqlClient;

namespace Stock_UnitTest
{
    [TestClass]
    public class HelperMethod
    {
        private const string UNIT_TEST_DB_NAME = "fx_unittests";
        private const string UNIT_TEST_TABLE_NAME = "currencies";


        #region TEST_CLASS_INITIALIZATION

        [ClassInitialize()]
        public static void InitTestSuite(TestContext testContext)
        {
            DbContext context = new OriginalDbContext();
            context.Database.ExecuteSqlCommand("recreateDb");
        }

        #endregion TEST_CLASS_INITIALIZATION


        #region TO_DB_STRING

        [TestMethod]
        public void toDbString_ReturnsStringNull_ForNullDoubleValue()
        {
            double? value = null;
            Assert.AreEqual("NULL", value.ToDbString());
        }

        [TestMethod]
        public void toDbString_ReturnsStringRepresentationOfNumberWithDotAsDecimalSeparator_ForNumericValue()
        {
            double? value = 5.342137;
            Assert.AreEqual("5.34214", value.ToDbString());
        }

        [TestMethod]
        public void toDbString_ReturnsStringNull_ForNullIntValue()
        {
            int? value = null;
            Assert.AreEqual("NULL", value.ToDbString());
        }

        [TestMethod]
        public void toDbString_ReturnsStringRepresentationOfNumber_ForInt()
        {
            int? value = 5;
            Assert.AreEqual("5", value.ToDbString());
        }

        [TestMethod]
        public void toDbString_Returns1_ForTrue()
        {
            bool value = true;
            Assert.AreEqual("1", value.ToDbString());
        }

        [TestMethod]
        public void toDbString_Returns0_ForFalse()
        {
            bool value = false;
            Assert.AreEqual("0", value.ToDbString());
        }

        [TestMethod]
        public void toDbString_ReturnsNullString_ForNullBoolean()
        {
            bool? value = null;
            Assert.AreEqual("NULL", value.ToDbString());
        }

        [TestMethod]
        public void toDbString_ReturnsNullString_ForNullDate()
        {
            DateTime? date = null;
            Assert.AreEqual("NULL", date.ToDbString());
        }

        [TestMethod]
        public void toDbString_ReturnsProperString_ForOnlyDate()
        {
            DateTime date = new DateTime(2017, 12, 4);
            Assert.AreEqual("'2017-12-04 00:00:00'", date.ToDbString());
        }

        [TestMethod]
        public void toDbString_ReturnsProperString_ForDateWithTime()
        {
            DateTime date = new DateTime(2017, 12, 4, 11, 31, 14);
            Assert.AreEqual("'2017-12-04 11:31:14'", date.ToDbString());
        }

        #endregion TO_DB_STRING


        #region CLEAR_TABLE

        [TestMethod]
        public void clearTable_DeletesAllRecords_IfTableExists()
        {

            //Arrange.
            const string SQL_COUNT_QUERY_RECORDS = "SELECT COUNT(*) FROM {0}.{1}";
            const string INSERT_SQL_PATTERN = "INSERT INTO {0}.{1}(Id, CurrencySymbol, CurrencyFullName) VALUES({2}, '{3}', '{4}');";
            string insertSql = string.Format(INSERT_SQL_PATTERN, UNIT_TEST_DB_NAME, UNIT_TEST_TABLE_NAME, 1, "USD", "US Dollar");

            DbContext context = new UnitTestsDbContext();
            context.Database.ExecuteSqlCommand(insertSql);

            //Act.
            context.ClearTable(UNIT_TEST_DB_NAME, UNIT_TEST_TABLE_NAME);
            var sql = string.Format(SQL_COUNT_QUERY_RECORDS, UNIT_TEST_DB_NAME, UNIT_TEST_TABLE_NAME);
            var actualRecordsCounter = context.Database.SqlQuery<int>(sql).Single();

            //Assert.
            int expectedRecordsCounter = 0;
            Assert.AreEqual(expectedRecordsCounter, actualRecordsCounter);

        }

        [TestMethod]
        [ExpectedException(typeof(MySqlException), "Table fx_unittests.nonExistingTable doesn't exist")]
        public void clearTable_ThrowException_IfGivenTableNameDoestnExists()
        {
            //Arrange.
            DbContext context = new UnitTestsDbContext();

            //Act.
            context.ClearTable(UNIT_TEST_DB_NAME, "nonExistingTable");

        }

        #endregion CLEAR_TABLE


        #region TEST_CLASS_TERMINATION

        [ClassCleanup()]
        public static void CleanupTestSuite()
        {
            DbContext context = new OriginalDbContext();
            context.Database.ExecuteSqlCommand("DROP DATABASE fx_unittests");
        }

        #endregion TEST_CLASS_TERMINATION

    }
}
