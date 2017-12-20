using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.DAL.Infrastructure;
using Stock.DAL.TransferObjects;
using Stock.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stock.DAL.Helpers;
using Stock.Utils;
using Stock.Core;

namespace Stock_UnitTest.Stock.DAL.Repositories.Data
{
    [TestClass]
    public class EFQuotationRepositoryUnitTest
    {

        private const string UNIT_TEST_DB_NAME = "fx_unittests";
        private const string QUOTATIONS_TABLE_NAME = "quotations";
        //public int Id { get; set; }
        //public DateTime PriceDate { get; set; }
        //public int AssetId { get; set; }
        //public int TimeframeId { get; set; }
        //public double OpenPrice { get; set; }
        //public double HighPrice { get; set; }
        //public double LowPrice { get; set; }
        //public double ClosePrice { get; set; }
        //public double Volume { get; set; }


        #region INFRASTRUCTURE

        [ClassInitialize()]
        public static void InitTestSuite(TestContext testContext)
        {
            DbContext context = new OriginalDbContext();
            context.Database.ExecuteSqlCommand("recreateDb");
        }

        [ClassCleanup()]
        public static void CleanupTestSuite()
        {
            DbContext context = new OriginalDbContext();
            context.Database.ExecuteSqlCommand("DROP DATABASE fx_unittests");
        }

        private void clearQuotationsTable()
        {
            DbContext context = new UnitTestsDbContext();
            context.ClearTable(UNIT_TEST_DB_NAME, QUOTATIONS_TABLE_NAME);
        }

        #endregion INFRASTRUCTURE


        #region UPDATE_QUOTATIONS

        [TestMethod]
        public void UpdateQuotations_WorksProperly_IfItemsAreOnlyAdded()
        {

            //Arrange
            EFQuotationRepository repository = new EFQuotationRepository();
            List<QuotationDto> quotations = new List<QuotationDto>();
            quotations.Add(new QuotationDto() { QuotationId = 2, PriceDate =  new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, OpenPrice =  1.09191, HighPrice =  1.09218, LowPrice =  1.09186, ClosePrice =  1.09194, Volume = 1411, IndexNumber = 2 });
            quotations.Add(new QuotationDto() { QuotationId = 3, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 });

            //Act
            clearQuotationsTable();
            repository.UpdateQuotations(quotations);
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            IEnumerable<QuotationDto> actualRecords = repository.GetQuotations(queryDef);

            //Assert
            bool areEqual = quotations.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void UpdateQuotations_WorksProperly_IfItemsAreOnlyUpdated()
        {

            //Arrange
            EFQuotationRepository repository = new EFQuotationRepository();
            List<QuotationDto> quotations = new List<QuotationDto>();
            QuotationDto dto1 = new QuotationDto() { QuotationId = 2, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09191, HighPrice = 1.09218, LowPrice = 1.09186, ClosePrice = 1.09194, Volume = 1411, IndexNumber = 2 };
            QuotationDto dto2 = new QuotationDto() { QuotationId = 3, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            quotations.AddRange(new QuotationDto[] { dto1, dto2 } );
            clearQuotationsTable();
            repository.UpdateQuotations(quotations);

            //Act
            dto1.OpenPrice = dto1.OpenPrice + 1;
            dto1.HighPrice = dto1.HighPrice + 1;
            dto1.LowPrice = dto1.LowPrice + 1;
            dto1.ClosePrice = dto1.ClosePrice + 1;
            dto2.OpenPrice = dto2.OpenPrice + 1;
            dto2.HighPrice = dto2.HighPrice + 1;
            dto2.LowPrice = dto2.LowPrice + 1;
            dto2.ClosePrice = dto2.ClosePrice + 1;
            repository.UpdateQuotations(new QuotationDto[] { dto1, dto2 });
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            IEnumerable<QuotationDto> actualRecords = repository.GetQuotations(queryDef);

            //Assert
            bool areEqual = quotations.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void UpdateQuotations_WorksProperly_IfItemsAreAddedAndUpdated()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFQuotationRepository repository = new EFQuotationRepository();
            List<QuotationDto> quotations = new List<QuotationDto>();
            QuotationDto dto1 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09191, HighPrice = 1.09218, LowPrice = 1.09186, ClosePrice = 1.09194, Volume = 1411, IndexNumber = 2 };
            QuotationDto dto2 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto3 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto4 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            quotations.AddRange(new QuotationDto[] { dto1, dto2 });
            clearQuotationsTable();
            repository.UpdateQuotations(quotations);

            //Act
            dto1.OpenPrice = dto1.OpenPrice + 1;
            dto1.HighPrice = dto1.HighPrice + 1;
            dto1.LowPrice = dto1.LowPrice + 1;
            dto1.ClosePrice = dto1.ClosePrice + 1;
            dto2.OpenPrice = dto2.OpenPrice + 1;
            dto2.HighPrice = dto2.HighPrice + 1;
            dto2.LowPrice = dto2.LowPrice + 1;
            dto2.ClosePrice = dto2.ClosePrice + 1;

            IEnumerable<QuotationDto> expectedRecords = new QuotationDto[] { dto1, dto2, dto3, dto4 };
            repository.UpdateQuotations(expectedRecords);
            IEnumerable<QuotationDto> actualRecords = repository.GetQuotations(queryDef);

            //Assert
            bool areEqual = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqual);

        }

        #endregion UPDATE_QUOTATIONS


        #region GET_QUOTATIONS

        [TestMethod]
        public void GetAllQuotations_ReturnsCollectionOnlyForGivenAssetAndTimeframe()
        {

            //Arrange
            EFQuotationRepository repository = new EFQuotationRepository();
            List<QuotationDto> quotations = new List<QuotationDto>();
            QuotationDto dto1 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09191, HighPrice = 1.09218, LowPrice = 1.09186, ClosePrice = 1.09194, Volume = 1411, IndexNumber = 2 };
            QuotationDto dto2 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto3 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto4 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto5 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto6 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto7 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto8 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 45, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            quotations.AddRange(new QuotationDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearQuotationsTable();
            repository.UpdateQuotations(quotations);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            IEnumerable<QuotationDto> actualRecords = repository.GetQuotations(queryDef);
            IEnumerable<QuotationDto> expectedRecords = new QuotationDto[] { dto1, dto2, dto3, dto4 };

            //Assert
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetAllQuotations_ReturnsEmptyCollection_IfThereIsNoItemsForGivenAssetAndTimeframe()
        {

            //Arrange
            EFQuotationRepository repository = new EFQuotationRepository();
            List<QuotationDto> quotations = new List<QuotationDto>();
            QuotationDto dto1 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09191, HighPrice = 1.09218, LowPrice = 1.09186, ClosePrice = 1.09194, Volume = 1411, IndexNumber = 2 };
            QuotationDto dto2 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto3 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto4 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto5 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto6 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto7 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto8 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 45, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            quotations.AddRange(new QuotationDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearQuotationsTable();
            repository.UpdateQuotations(quotations);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(2, 2);
            IEnumerable<QuotationDto> actualRecords = repository.GetQuotations(queryDef);

            //Assert
            bool isEmptyCollection = (actualRecords.Count() == 0);
            Assert.IsTrue(isEmptyCollection);

        }

        #endregion GET_QUOTATIONS



        #region GET_QUOTATIONS_WITH_QUERY_DEF


        [TestMethod]
        public void GetQuotationsWithQueryDef_ReturnsAllDataAvailableForGivenCombinationOfAssetAndTimeframe()
        {

            //Arrange
            EFQuotationRepository repository = new EFQuotationRepository();
            List<QuotationDto> quotations = new List<QuotationDto>();
            QuotationDto dto1 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09191, HighPrice = 1.09218, LowPrice = 1.09186, ClosePrice = 1.09194, Volume = 1411, IndexNumber = 2 };
            QuotationDto dto2 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto3 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto4 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto5 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto6 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto7 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto8 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 45, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            quotations.AddRange(new QuotationDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearQuotationsTable();
            repository.UpdateQuotations(quotations);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            IEnumerable<QuotationDto> actualRecords = repository.GetQuotations(queryDef);
            IEnumerable<QuotationDto> expectedRecords = new QuotationDto[] { dto1, dto2, dto3, dto4 };

            //Assert
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetQuotationsWithQueryDef_ReturnsEmptyContainer_IfThereIsNoDataForGivenCombinationOfAssetAndTimeframe()
        {

            //Arrange
            EFQuotationRepository repository = new EFQuotationRepository();
            List<QuotationDto> quotations = new List<QuotationDto>();
            QuotationDto dto1 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09191, HighPrice = 1.09218, LowPrice = 1.09186, ClosePrice = 1.09194, Volume = 1411, IndexNumber = 2 };
            QuotationDto dto2 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto3 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto4 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto5 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto6 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto7 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto8 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 45, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            quotations.AddRange(new QuotationDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearQuotationsTable();
            repository.UpdateQuotations(quotations);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(2, 2);
            IEnumerable<QuotationDto> actualRecords = repository.GetQuotations(queryDef);

            //Assert
            bool isEmptyCollection = (actualRecords.Count() == 0);
            Assert.IsTrue(isEmptyCollection);

        }

        [TestMethod]
        public void GetQuotationsWithQueryDef_ReturnsDataFromGivenStartDateOnly()
        {

            //Arrange
            EFQuotationRepository repository = new EFQuotationRepository();
            List<QuotationDto> quotations = new List<QuotationDto>();
            QuotationDto dto1 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09191, HighPrice = 1.09218, LowPrice = 1.09186, ClosePrice = 1.09194, Volume = 1411, IndexNumber = 2 };
            QuotationDto dto2 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto3 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto4 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto5 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto6 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto7 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto8 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 45, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            quotations.AddRange(new QuotationDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearQuotationsTable();
            repository.UpdateQuotations(quotations);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1) { StartDate = new DateTime(2016, 1, 15, 22, 35, 0) };
            IEnumerable<QuotationDto> actualRecords = repository.GetQuotations(queryDef);

            //Assert
            IEnumerable<QuotationDto> expectedRecords = new QuotationDto[] { dto3, dto4 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetQuotationsWithQueryDef_ReturnsDataToGivenEndDateOnly()
        {

            //Arrange
            EFQuotationRepository repository = new EFQuotationRepository();
            List<QuotationDto> quotations = new List<QuotationDto>();
            QuotationDto dto1 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09191, HighPrice = 1.09218, LowPrice = 1.09186, ClosePrice = 1.09194, Volume = 1411, IndexNumber = 2 };
            QuotationDto dto2 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto3 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto4 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto5 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto6 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto7 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto8 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 45, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            quotations.AddRange(new QuotationDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearQuotationsTable();
            repository.UpdateQuotations(quotations);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1) { EndDate = new DateTime(2016, 1, 15, 22, 35, 0) };
            IEnumerable<QuotationDto> actualRecords = repository.GetQuotations(queryDef);

            //Assert
            IEnumerable<QuotationDto> expectedRecords = new QuotationDto[] { dto1, dto2, dto3 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }


        [TestMethod]
        public void GetQuotationsWithQueryDef_ReturnsDataFromGivenStartIndexOnly()
        {

            //Arrange
            EFQuotationRepository repository = new EFQuotationRepository();
            List<QuotationDto> quotations = new List<QuotationDto>();
            QuotationDto dto1 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09191, HighPrice = 1.09218, LowPrice = 1.09186, ClosePrice = 1.09194, Volume = 1411, IndexNumber = 2 };
            QuotationDto dto2 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto3 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 4 };
            QuotationDto dto4 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 5 };
            QuotationDto dto5 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 2 };
            QuotationDto dto6 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto7 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 1 };
            QuotationDto dto8 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 45, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 2 };
            quotations.AddRange(new QuotationDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearQuotationsTable();
            repository.UpdateQuotations(quotations);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1) { StartIndex = 3 };
            IEnumerable<QuotationDto> actualRecords = repository.GetQuotations(queryDef);

            //Assert
            IEnumerable<QuotationDto> expectedRecords = new QuotationDto[] { dto2, dto3, dto4 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetQuotationsWithQueryDef_ReturnsDataToGivenEndIndexOnly()
        {

            //Arrange
            EFQuotationRepository repository = new EFQuotationRepository();
            List<QuotationDto> quotations = new List<QuotationDto>();
            QuotationDto dto1 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09191, HighPrice = 1.09218, LowPrice = 1.09186, ClosePrice = 1.09194, Volume = 1411, IndexNumber = 2 };
            QuotationDto dto2 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto3 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 4 };
            QuotationDto dto4 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 5 };
            QuotationDto dto5 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 1 };
            QuotationDto dto6 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 2 };
            QuotationDto dto7 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 1 };
            QuotationDto dto8 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 45, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 2 };
            quotations.AddRange(new QuotationDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearQuotationsTable();
            repository.UpdateQuotations(quotations);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1) { EndIndex = 3 };
            IEnumerable<QuotationDto> actualRecords = repository.GetQuotations(queryDef);

            //Assert
            IEnumerable<QuotationDto> expectedRecords = new QuotationDto[] { dto1, dto2 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }



        [TestMethod]
        public void GetQuotationsWithQueryDef_ReturnsDataFromGivenStartDateAndToGivenEndDateOnly()
        {

            //Arrange
            EFQuotationRepository repository = new EFQuotationRepository();
            List<QuotationDto> quotations = new List<QuotationDto>();
            QuotationDto dto1 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09191, HighPrice = 1.09218, LowPrice = 1.09186, ClosePrice = 1.09194, Volume = 1411, IndexNumber = 2 };
            QuotationDto dto2 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto3 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto4 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto5 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto6 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto7 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto8 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 45, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            quotations.AddRange(new QuotationDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearQuotationsTable();
            repository.UpdateQuotations(quotations);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1) { 
                                                            StartDate = new DateTime(2016, 1, 15, 22, 30, 0), 
                                                            EndDate = new DateTime(2016, 1, 15, 22, 35, 0) 
                                                        };
            IEnumerable<QuotationDto> actualRecords = repository.GetQuotations(queryDef);

            //Assert
            IEnumerable<QuotationDto> expectedRecords = new QuotationDto[] { dto2, dto3 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }


        [TestMethod]
        public void GetQuotationsWithQueryDef_ReturnsDataFromGivenStartIndexAndToGivenEndIndexOnly()
        {

            //Arrange
            EFQuotationRepository repository = new EFQuotationRepository();
            List<QuotationDto> quotations = new List<QuotationDto>();
            QuotationDto dto1 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09191, HighPrice = 1.09218, LowPrice = 1.09186, ClosePrice = 1.09194, Volume = 1411, IndexNumber = 2 };
            QuotationDto dto2 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto3 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 4 };
            QuotationDto dto4 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 5 };
            QuotationDto dto5 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 1 };
            QuotationDto dto6 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 2 };
            QuotationDto dto7 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 6 };
            QuotationDto dto8 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 45, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 7 };
            quotations.AddRange(new QuotationDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearQuotationsTable();
            repository.UpdateQuotations(quotations);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1)
            {
                StartIndex = 3,
                EndIndex = 4
            };
            IEnumerable<QuotationDto> actualRecords = repository.GetQuotations(queryDef);

            //Assert
            IEnumerable<QuotationDto> expectedRecords = new QuotationDto[] { dto2, dto3 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetQuotationsWithQueryDef_ReturnsEmptyContainer_IfThereIsNoDataInGivenDateRange()
        {

            //Arrange
            EFQuotationRepository repository = new EFQuotationRepository();
            List<QuotationDto> quotations = new List<QuotationDto>();
            QuotationDto dto1 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09191, HighPrice = 1.09218, LowPrice = 1.09186, ClosePrice = 1.09194, Volume = 1411, IndexNumber = 2 };
            QuotationDto dto2 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto3 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto4 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto5 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto6 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto7 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto8 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 45, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            quotations.AddRange(new QuotationDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearQuotationsTable();
            repository.UpdateQuotations(quotations);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1)
            {
                StartDate = new DateTime(2016, 1, 15, 23, 30, 0),
                EndDate = new DateTime(2016, 1, 15, 22, 35, 0)
            };
            IEnumerable<QuotationDto> actualRecords = repository.GetQuotations(queryDef);

            //Assert
            bool isEmpty = actualRecords.Count() == 0;
            Assert.IsTrue(isEmpty);

        }


        [TestMethod]
        public void GetQuotationsWithQueryDef_ReturnsEmptyContainer_IfThereIsNoDataInGivenIndexRange()
        {

            //Arrange
            EFQuotationRepository repository = new EFQuotationRepository();
            List<QuotationDto> quotations = new List<QuotationDto>();
            QuotationDto dto1 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09191, HighPrice = 1.09218, LowPrice = 1.09186, ClosePrice = 1.09194, Volume = 1411, IndexNumber = 2 };
            QuotationDto dto2 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto3 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 4 };
            QuotationDto dto4 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 5 };
            QuotationDto dto5 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 1 };
            QuotationDto dto6 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 2 };
            QuotationDto dto7 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto8 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 45, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 4 };
            quotations.AddRange(new QuotationDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearQuotationsTable();
            repository.UpdateQuotations(quotations);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1)
            {
                StartIndex = 10,
                EndIndex = 15
            };
            IEnumerable<QuotationDto> actualRecords = repository.GetQuotations(queryDef);

            //Assert
            bool isEmpty = actualRecords.Count() == 0;
            Assert.IsTrue(isEmpty);

        }

        [TestMethod]
        public void GetQuotationsWithQueryDef_ReturnsOnlyLimitedNumberOfQuotationsFromLeft_IfStartDateIsGiven()
        {

            //Arrange
            EFQuotationRepository repository = new EFQuotationRepository();
            List<QuotationDto> quotations = new List<QuotationDto>();
            QuotationDto dto1 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09191, HighPrice = 1.09218, LowPrice = 1.09186, ClosePrice = 1.09194, Volume = 1411, IndexNumber = 2 };
            QuotationDto dto2 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto3 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto4 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto5 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto6 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto7 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto8 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 45, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            quotations.AddRange(new QuotationDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearQuotationsTable();
            repository.UpdateQuotations(quotations);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1)
            {
                StartDate = new DateTime(2016, 1, 15, 22, 25, 0),
                EndDate = new DateTime(2016, 1, 15, 22, 35, 0),
                Limit = 2
            };
            IEnumerable<QuotationDto> actualRecords = repository.GetQuotations(queryDef);

            //Assert
            IEnumerable<QuotationDto> expectedRecords = new QuotationDto[] { dto1, dto2 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetQuotationsWithQueryDef_ReturnsOnlyLimitedNumberOfQuotationsFromRight_IfOnlyEndDateIsGiven()
        {

            //Arrange
            EFQuotationRepository repository = new EFQuotationRepository();
            List<QuotationDto> quotations = new List<QuotationDto>();
            QuotationDto dto1 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09191, HighPrice = 1.09218, LowPrice = 1.09186, ClosePrice = 1.09194, Volume = 1411, IndexNumber = 2 };
            QuotationDto dto2 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto3 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto4 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto5 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto6 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto7 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto8 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 45, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            quotations.AddRange(new QuotationDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearQuotationsTable();
            repository.UpdateQuotations(quotations);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1)
            {
                EndDate = new DateTime(2016, 1, 15, 22, 35, 0),
                Limit = 2
            };
            IEnumerable<QuotationDto> actualRecords = repository.GetQuotations(queryDef);

            //Assert
            IEnumerable<QuotationDto> expectedRecords = new QuotationDto[] { dto2, dto3 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetQuotationsWithQueryDef_ReturnsOnlyLimitedNumberOfQuotationsFromLeft_IfNoDateIsGiven()
        {

            //Arrange
            EFQuotationRepository repository = new EFQuotationRepository();
            List<QuotationDto> quotations = new List<QuotationDto>();
            QuotationDto dto1 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09191, HighPrice = 1.09218, LowPrice = 1.09186, ClosePrice = 1.09194, Volume = 1411, IndexNumber = 2 };
            QuotationDto dto2 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto3 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto4 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto5 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto6 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto7 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto8 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 45, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            quotations.AddRange(new QuotationDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearQuotationsTable();
            repository.UpdateQuotations(quotations);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1)
            {
                Limit = 2
            };
            IEnumerable<QuotationDto> actualRecords = repository.GetQuotations(queryDef);

            //Assert
            IEnumerable<QuotationDto> expectedRecords = new QuotationDto[] { dto1, dto2 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetQuotationsWithQueryDef_ReturnsQuotations_EvenIfQuotationIsNotIncludedInAnalysisTypeParamOfQueryDef()
        {

            //Arrange
            EFQuotationRepository repository = new EFQuotationRepository();
            List<QuotationDto> quotations = new List<QuotationDto>();
            QuotationDto dto1 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09191, HighPrice = 1.09218, LowPrice = 1.09186, ClosePrice = 1.09194, Volume = 1411, IndexNumber = 2 };
            QuotationDto dto2 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto3 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto4 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 1, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto5 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto6 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), AssetId = 2, TimeframeId = 1, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto7 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            QuotationDto dto8 = new QuotationDto() { PriceDate = new DateTime(2016, 1, 15, 22, 45, 0), AssetId = 1, TimeframeId = 2, OpenPrice = 1.09193, HighPrice = 1.09256, LowPrice = 1.09165, ClosePrice = 1.09177, Volume = 1819, IndexNumber = 3 };
            quotations.AddRange(new QuotationDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearQuotationsTable();
            repository.UpdateQuotations(quotations);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1)
            {
                AnalysisTypes = new AnalysisType[] { AnalysisType.Macd, AnalysisType.Adx }
            };
            IEnumerable<QuotationDto> actualRecords = repository.GetQuotations(queryDef);

            //Assert
            IEnumerable<QuotationDto> expectedRecords = new QuotationDto[] { dto1, dto2, dto3, dto4 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        #endregion GET_QUOTATIONS_WITH_QUERY_DEF


    }
}
