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
    public class EFPriceRepositoryUnitTest
    {

        private const string UNIT_TEST_DB_NAME = "fx_unittests";
        private const string PRICES_TABLE_NAME = "prices";
        private const string EXTREMA_TABLE_NAME = "extrema";


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

        private void clearPricesTable()
        {
            DbContext context = new UnitTestsDbContext();
            context.ClearTable(UNIT_TEST_DB_NAME, PRICES_TABLE_NAME);
            context.ClearTable(UNIT_TEST_DB_NAME, EXTREMA_TABLE_NAME);
        }

        #endregion INFRASTRUCTURE


        #region UPDATE_PRICES

        [TestMethod]
        public void UpdatePrices_WorksProperly_IfItemsAreOnlyAdded()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFPriceRepository repository = new EFPriceRepository();
            List<PriceDto> prices = new List<PriceDto>();
            PriceDto dto1 = new PriceDto() { Id = 1, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto2 = new PriceDto() { Id = 2, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto3 = new PriceDto() { Id = 3, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto4 = new PriceDto() { Id = 4, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            prices.AddRange(new PriceDto[] { dto1, dto2, dto3, dto4 });

            //Act
            clearPricesTable();
            repository.UpdatePrices(prices);
            IEnumerable<PriceDto> actualRecords = repository.GetPrices(queryDef);

            //Assert
            bool areEqual = prices.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void UpdatePrices_WorksProperly_IfItemsAreOnlyUpdated()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFPriceRepository repository = new EFPriceRepository();
            List<PriceDto> prices = new List<PriceDto>();
            PriceDto dto1 = new PriceDto() { Id = 1, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto2 = new PriceDto() { Id = 2, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto3 = new PriceDto() { Id = 3, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto4 = new PriceDto() { Id = 4, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            prices.AddRange(new PriceDto[] { dto1, dto2, dto3, dto4 });
            clearPricesTable();
            repository.UpdatePrices(prices);

            //Act
            dto1.DeltaClosePrice += 0.07;
            dto1.PriceDirection2D *= -1;
            dto2.CloseRatio += 1.23;
            dto3.DeltaClosePrice += 0.23;
            repository.UpdatePrices(new PriceDto[] { dto1, dto2, dto3 });
            IEnumerable<PriceDto> actualRecords = repository.GetPrices(queryDef);

            //Assert
            bool areEqual = prices.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void UpdatePrices_WorksProperly_IfItemsAreAddedAndUpdated()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFPriceRepository repository = new EFPriceRepository();
            List<PriceDto> prices = new List<PriceDto>();
            PriceDto dto1 = new PriceDto() { Id = 1, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto2 = new PriceDto() { Id = 2, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto3 = new PriceDto() { Id = 3, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto4 = new PriceDto() { Id = 4, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            prices.AddRange(new PriceDto[] { dto1, dto2, dto3 });
            clearPricesTable();
            repository.UpdatePrices(prices);

            //Act
            dto1.DeltaClosePrice += 0.07;
            dto1.PriceDirection2D *= -1;
            dto2.CloseRatio += 0.22;
            dto3.DeltaClosePrice += 0.23;

            IEnumerable<PriceDto> expectedRecords = new PriceDto[] { dto1, dto2, dto3, dto4 };
            repository.UpdatePrices(expectedRecords);
            IEnumerable<PriceDto> actualRecords = repository.GetPrices(queryDef);

            //Assert
            bool areEqual = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqual);

        }

        #endregion UPDATE_PRICES


        #region UPDATE_EXTREMA

        [TestMethod]
        public void UpdateExtrema_WorksProperly_IfItemsAreOnlyAdded()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFPriceRepository repository = new EFPriceRepository();
            List<ExtremumDto> extrema = new List<ExtremumDto>();
            ExtremumDto dto1 = new ExtremumDto() { Id = 1, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto2 = new ExtremumDto() { Id = 2, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto3 = new ExtremumDto() { Id = 3, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto4 = new ExtremumDto() { Id = 4, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            extrema.AddRange(new ExtremumDto[] { dto1, dto2, dto3, dto4 });

            //Act
            clearPricesTable();
            repository.UpdateExtrema(extrema);
            IEnumerable<ExtremumDto> actualRecords = repository.GetExtrema(queryDef);

            //Assert
            bool areEqual = extrema.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void UpdateExtrema_WorksProperly_IfItemsAreOnlyUpdated()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFPriceRepository repository = new EFPriceRepository();
            List<ExtremumDto> extrema = new List<ExtremumDto>();
            ExtremumDto dto1 = new ExtremumDto() { Id = 1, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto2 = new ExtremumDto() { Id = 2, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto3 = new ExtremumDto() { Id = 3, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto4 = new ExtremumDto() { Id = 4, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            extrema.AddRange(new ExtremumDto[] { dto1, dto2, dto3, dto4 });

            //Act
            clearPricesTable();
            repository.UpdateExtrema(extrema);
            dto1.EarlierChange1 += 0.07;
            dto1.EarlierAmplitude += -0.15;
            dto2.Volatility += 0.05;
            dto2.LaterChange1 += 0.23;
            dto3.LaterCounter++;
            repository.UpdateExtrema(new ExtremumDto[] { dto1, dto2, dto3 });
            IEnumerable<ExtremumDto> actualRecords = repository.GetExtrema(queryDef);

            //Assert
            bool areEqual = extrema.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void UpdateExtrema_WorksProperly_IfItemsAreAddedAndUpdated()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFPriceRepository repository = new EFPriceRepository();
            List<ExtremumDto> extrema = new List<ExtremumDto>();
            ExtremumDto dto1 = new ExtremumDto() { Id = 1, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto2 = new ExtremumDto() { Id = 2, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto3 = new ExtremumDto() { Id = 3, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto4 = new ExtremumDto() { Id = 4, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            extrema.AddRange(new ExtremumDto[] { dto1, dto2, dto3, dto4 });
            clearPricesTable();
            repository.UpdateExtrema(extrema);

            //Act
            dto1.EarlierChange10 += 0.07;
            dto1.EarlierCounter++;
            dto2.Volatility += 0.05;
            dto2.LaterChange2 += 0.23;
            dto3.Value += 0.23;
            dto3.IsOpen = !dto3.IsOpen;

            IEnumerable<ExtremumDto> expectedRecords = new ExtremumDto[] { dto1, dto2, dto3, dto4 };
            repository.UpdateExtrema(expectedRecords);
            IEnumerable<ExtremumDto> actualRecords = repository.GetExtrema(queryDef);

            //Assert
            bool areEqual = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqual);

        }

        #endregion UPDATE_EXTREMA


        #region GET_PRICES

        [TestMethod]
        public void GetAllPrices_ReturnsCollectionOnlyForGivenAssetTimeframeSimulation()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1) { SimulationId = 1 };
            EFPriceRepository repository = new EFPriceRepository();
            List<PriceDto> prices = new List<PriceDto>();
            PriceDto dto1 = new PriceDto() { Id = 1, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            PriceDto dto2 = new PriceDto() { Id = 2, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            PriceDto dto3 = new PriceDto() { Id = 3, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            PriceDto dto4 = new PriceDto() { Id = 4, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 2 };
            PriceDto dto5 = new PriceDto() { Id = 5, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            PriceDto dto6 = new PriceDto() { Id = 6, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            PriceDto dto7 = new PriceDto() { Id = 7, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            PriceDto dto8 = new PriceDto() { Id = 8, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            prices.AddRange(new PriceDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdatePrices(prices);

            //Act
            IEnumerable<PriceDto> actualRecords = repository.GetPrices(queryDef);
            IEnumerable<PriceDto> expectedRecords = new PriceDto[] { dto1, dto2, dto3 };

            //Assert
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetAllPrices_ReturnsCollectionOnlyForGivenAssetTimeframeIfSimulationNotSpecified()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            EFPriceRepository repository = new EFPriceRepository();
            List<PriceDto> prices = new List<PriceDto>();
            PriceDto dto1 = new PriceDto() { Id = 1, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            PriceDto dto2 = new PriceDto() { Id = 2, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            PriceDto dto3 = new PriceDto() { Id = 3, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            PriceDto dto4 = new PriceDto() { Id = 4, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 2 };
            PriceDto dto5 = new PriceDto() { Id = 5, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            PriceDto dto6 = new PriceDto() { Id = 6, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            PriceDto dto7 = new PriceDto() { Id = 7, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            PriceDto dto8 = new PriceDto() { Id = 8, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            prices.AddRange(new PriceDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdatePrices(prices);

            //Act
            IEnumerable<PriceDto> actualRecords = repository.GetPrices(queryDef);
            IEnumerable<PriceDto> expectedRecords = new PriceDto[] { dto1, dto2, dto3, dto4 };

            //Assert
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }


        [TestMethod]
        public void GetAllPrices_ReturnsEmptyCollection_IfThereIsNoItemsForGivenAssetAndTimeframe()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(2, 2);
            EFPriceRepository repository = new EFPriceRepository();
            List<PriceDto> prices = new List<PriceDto>();
            PriceDto dto1 = new PriceDto() { Id = 1, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto2 = new PriceDto() { Id = 2, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto3 = new PriceDto() { Id = 3, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto4 = new PriceDto() { Id = 4, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto5 = new PriceDto() { Id = 5, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto6 = new PriceDto() { Id = 6, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto7 = new PriceDto() { Id = 7, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto8 = new PriceDto() { Id = 8, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            prices.AddRange(new PriceDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdatePrices(prices);

            //Act
            IEnumerable<PriceDto> actualRecords = repository.GetPrices(queryDef);

            //Assert
            bool isEmptyCollection = (actualRecords.Count() == 0);
            Assert.IsTrue(isEmptyCollection);

        }

        [TestMethod]
        public void GetAllPrices_ReturnsEmptyCollection_IfThereIsNoItemsForGivenSimulation()
        {

            //Arrange
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1) { SimulationId = 2 };
            EFPriceRepository repository = new EFPriceRepository();
            List<PriceDto> prices = new List<PriceDto>();
            PriceDto dto1 = new PriceDto() { Id = 1, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            PriceDto dto2 = new PriceDto() { Id = 2, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            PriceDto dto3 = new PriceDto() { Id = 3, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            PriceDto dto4 = new PriceDto() { Id = 4, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            PriceDto dto5 = new PriceDto() { Id = 5, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            PriceDto dto6 = new PriceDto() { Id = 6, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            PriceDto dto7 = new PriceDto() { Id = 7, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            PriceDto dto8 = new PriceDto() { Id = 8, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34, SimulationId = 1 };
            prices.AddRange(new PriceDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdatePrices(prices);

            //Act
            IEnumerable<PriceDto> actualRecords = repository.GetPrices(queryDef);

            //Assert
            bool isEmptyCollection = (actualRecords.Count() == 0);
            Assert.IsTrue(isEmptyCollection);

        }

        #endregion GET_PRICES


        #region GET_PRICES_WITH_QUERY_DEF


        [TestMethod]
        public void GetPricesWithQueryDef_ReturnsAllDataAvailableForGivenCombinationOfAssetAndTimeframe()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<PriceDto> prices = new List<PriceDto>();
            PriceDto dto1 = new PriceDto() { Id = 1, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto2 = new PriceDto() { Id = 2, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto3 = new PriceDto() { Id = 3, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto4 = new PriceDto() { Id = 4, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto5 = new PriceDto() { Id = 5, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto6 = new PriceDto() { Id = 6, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto7 = new PriceDto() { Id = 7, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto8 = new PriceDto() { Id = 8, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            prices.AddRange(new PriceDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdatePrices(prices);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            IEnumerable<PriceDto> actualRecords = repository.GetPrices(queryDef);
            IEnumerable<PriceDto> expectedRecords = new PriceDto[] { dto1, dto2, dto3, dto4 };

            //Assert
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetPricesWithQueryDef_ReturnsEmptyContainer_IfThereIsNoDataForGivenCombinationOfAssetAndTimeframe()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<PriceDto> prices = new List<PriceDto>();
            PriceDto dto1 = new PriceDto() { Id = 1, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto2 = new PriceDto() { Id = 2, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto3 = new PriceDto() { Id = 3, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto4 = new PriceDto() { Id = 4, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto5 = new PriceDto() { Id = 5, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto6 = new PriceDto() { Id = 6, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto7 = new PriceDto() { Id = 7, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto8 = new PriceDto() { Id = 8, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            prices.AddRange(new PriceDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdatePrices(prices);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(2, 2);
            IEnumerable<PriceDto> actualRecords = repository.GetPrices(queryDef);

            //Assert
            bool isEmptyCollection = (actualRecords.Count() == 0);
            Assert.IsTrue(isEmptyCollection);

        }

        [TestMethod]
        public void GetPricesWithQueryDef_ReturnsDataFromGivenStartDateOnly()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<PriceDto> prices = new List<PriceDto>();
            PriceDto dto1 = new PriceDto() { Id = 1, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto2 = new PriceDto() { Id = 2, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto3 = new PriceDto() { Id = 3, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto4 = new PriceDto() { Id = 4, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto5 = new PriceDto() { Id = 5, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto6 = new PriceDto() { Id = 6, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto7 = new PriceDto() { Id = 7, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto8 = new PriceDto() { Id = 8, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            prices.AddRange(new PriceDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdatePrices(prices);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1) { StartDate = new DateTime(2016, 1, 15, 22, 35, 0) };
            IEnumerable<PriceDto> actualRecords = repository.GetPrices(queryDef);

            //Assert
            IEnumerable<PriceDto> expectedRecords = new PriceDto[] { dto3, dto4 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetPricesWithQueryDef_ReturnsDataToGivenEndDateOnly()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<PriceDto> prices = new List<PriceDto>();
            PriceDto dto1 = new PriceDto() { Id = 1, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto2 = new PriceDto() { Id = 2, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto3 = new PriceDto() { Id = 3, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto4 = new PriceDto() { Id = 4, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto5 = new PriceDto() { Id = 5, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto6 = new PriceDto() { Id = 6, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto7 = new PriceDto() { Id = 7, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto8 = new PriceDto() { Id = 8, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            prices.AddRange(new PriceDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdatePrices(prices);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1) { EndDate = new DateTime(2016, 1, 15, 22, 35, 0) };
            IEnumerable<PriceDto> actualRecords = repository.GetPrices(queryDef);

            //Assert
            IEnumerable<PriceDto> expectedRecords = new PriceDto[] { dto1, dto2, dto3 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }


        [TestMethod]
        public void GetPricesWithQueryDef_ReturnsDataFromGivenStartDateAndToGivenEndDateOnly()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<PriceDto> prices = new List<PriceDto>();
            PriceDto dto1 = new PriceDto() { Id = 1, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto2 = new PriceDto() { Id = 2, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto3 = new PriceDto() { Id = 3, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto4 = new PriceDto() { Id = 4, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto5 = new PriceDto() { Id = 5, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto6 = new PriceDto() { Id = 6, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto7 = new PriceDto() { Id = 7, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto8 = new PriceDto() { Id = 8, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            prices.AddRange(new PriceDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdatePrices(prices);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1)
            {
                StartDate = new DateTime(2016, 1, 15, 22, 30, 0),
                EndDate = new DateTime(2016, 1, 15, 22, 35, 0)
            };
            IEnumerable<PriceDto> actualRecords = repository.GetPrices(queryDef);

            //Assert
            IEnumerable<PriceDto> expectedRecords = new PriceDto[] { dto2, dto3 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetPricesWithQueryDef_ReturnsEmptyContainer_IfThereIsNoDataInGivenDateRange()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<PriceDto> prices = new List<PriceDto>();
            PriceDto dto1 = new PriceDto() { Id = 1, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto2 = new PriceDto() { Id = 2, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto3 = new PriceDto() { Id = 3, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto4 = new PriceDto() { Id = 4, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto5 = new PriceDto() { Id = 5, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto6 = new PriceDto() { Id = 6, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto7 = new PriceDto() { Id = 7, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto8 = new PriceDto() { Id = 8, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            prices.AddRange(new PriceDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdatePrices(prices);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1)
            {
                StartDate = new DateTime(2016, 1, 15, 23, 30, 0),
                EndDate = new DateTime(2016, 1, 15, 22, 35, 0)
            };
            IEnumerable<PriceDto> actualRecords = repository.GetPrices(queryDef);

            //Assert
            bool isEmpty = actualRecords.Count() == 0;
            Assert.IsTrue(isEmpty);

        }

        [TestMethod]
        public void GetPricesWithQueryDef_ReturnsOnlyLimitedNumberOfPricesFromLeft_IfStartDateIsGiven()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<PriceDto> prices = new List<PriceDto>();
            PriceDto dto1 = new PriceDto() { Id = 1, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto2 = new PriceDto() { Id = 2, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto3 = new PriceDto() { Id = 3, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto4 = new PriceDto() { Id = 4, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto5 = new PriceDto() { Id = 5, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto6 = new PriceDto() { Id = 6, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto7 = new PriceDto() { Id = 7, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto8 = new PriceDto() { Id = 8, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            prices.AddRange(new PriceDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdatePrices(prices);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1)
            {
                StartDate = new DateTime(2016, 1, 15, 22, 25, 0),
                EndDate = new DateTime(2016, 1, 15, 22, 35, 0),
                Limit = 2
            };
            IEnumerable<PriceDto> actualRecords = repository.GetPrices(queryDef);

            //Assert
            IEnumerable<PriceDto> expectedRecords = new PriceDto[] { dto1, dto2 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetPricesWithQueryDef_ReturnsOnlyLimitedNumberOfPricesFromRight_IfOnlyEndDateIsGiven()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<PriceDto> prices = new List<PriceDto>();
            PriceDto dto1 = new PriceDto() { Id = 1, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto2 = new PriceDto() { Id = 2, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto3 = new PriceDto() { Id = 3, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto4 = new PriceDto() { Id = 4, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto5 = new PriceDto() { Id = 5, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto6 = new PriceDto() { Id = 6, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto7 = new PriceDto() { Id = 7, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto8 = new PriceDto() { Id = 8, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            prices.AddRange(new PriceDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdatePrices(prices);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1)
            {
                EndDate = new DateTime(2016, 1, 15, 22, 35, 0),
                Limit = 2
            };
            IEnumerable<PriceDto> actualRecords = repository.GetPrices(queryDef);

            //Assert
            IEnumerable<PriceDto> expectedRecords = new PriceDto[] { dto2, dto3 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetPricesWithQueryDef_ReturnsOnlyLimitedNumberOfPricesFromLeft_IfNoDateIsGiven()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<PriceDto> prices = new List<PriceDto>();
            PriceDto dto1 = new PriceDto() { Id = 1, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto2 = new PriceDto() { Id = 2, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto3 = new PriceDto() { Id = 3, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto4 = new PriceDto() { Id = 4, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto5 = new PriceDto() { Id = 5, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto6 = new PriceDto() { Id = 6, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto7 = new PriceDto() { Id = 7, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto8 = new PriceDto() { Id = 8, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            prices.AddRange(new PriceDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdatePrices(prices);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1)
            {
                Limit = 2
            };
            IEnumerable<PriceDto> actualRecords = repository.GetPrices(queryDef);

            //Assert
            IEnumerable<PriceDto> expectedRecords = new PriceDto[] { dto1, dto2 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetPricesWithQueryDef_ReturnsPrices_EvenIfPriceIsNotIncludedInAnalysisTypeParamOfQueryDef()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<PriceDto> prices = new List<PriceDto>();
            PriceDto dto1 = new PriceDto() { Id = 1, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto2 = new PriceDto() { Id = 2, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto3 = new PriceDto() { Id = 3, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto4 = new PriceDto() { Id = 4, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto5 = new PriceDto() { Id = 5, PriceDate = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.05, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto6 = new PriceDto() { Id = 6, PriceDate = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.06, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto7 = new PriceDto() { Id = 7, PriceDate = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.07, PriceDirection2D = 1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            PriceDto dto8 = new PriceDto() { Id = 8, PriceDate = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, DeltaClosePrice = 1.08, PriceDirection2D = -1, PriceDirection3D = 0, PriceGap = 1.23, CloseRatio = 1.23, ExtremumRatio = 2.34 };
            prices.AddRange(new PriceDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdatePrices(prices);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1)
            {
                AnalysisTypes = new AnalysisType[] { AnalysisType.Macd, AnalysisType.Adx }
            };
            IEnumerable<PriceDto> actualRecords = repository.GetPrices(queryDef);

            //Assert
            IEnumerable<PriceDto> expectedRecords = new PriceDto[] { dto1, dto2, dto3, dto4 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        #endregion GET_PRICES_WITH_QUERY_DEF


        #region GET_EXTREMA_WITH_QUERY_DEF


        [TestMethod]
        public void GetExtrema_ReturnsAllExtremaAvailableForGivenCombinationOfAssetAndTimeframe()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<ExtremumDto> extrema = new List<ExtremumDto>();
            ExtremumDto dto1 = new ExtremumDto() { Id = 1, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto2 = new ExtremumDto() { Id = 2, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto3 = new ExtremumDto() { Id = 3, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto4 = new ExtremumDto() { Id = 4, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto5 = new ExtremumDto() { Id = 5, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto6 = new ExtremumDto() { Id = 6, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto7 = new ExtremumDto() { Id = 7, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto8 = new ExtremumDto() { Id = 8, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            extrema.AddRange(new ExtremumDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdateExtrema(extrema);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1);
            IEnumerable<ExtremumDto> actualRecords = repository.GetExtrema(queryDef);
            IEnumerable<ExtremumDto> expectedRecords = new ExtremumDto[] { dto1, dto2, dto3, dto4 };

            //Assert
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetExtrema_ReturnsEmptyContainer_IfThereIsNoDataForGivenCombinationOfAssetAndTimeframe()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<ExtremumDto> extrema = new List<ExtremumDto>();
            ExtremumDto dto1 = new ExtremumDto() { Id = 1, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto2 = new ExtremumDto() { Id = 2, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto3 = new ExtremumDto() { Id = 3, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto4 = new ExtremumDto() { Id = 4, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto5 = new ExtremumDto() { Id = 5, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto6 = new ExtremumDto() { Id = 6, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto7 = new ExtremumDto() { Id = 7, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto8 = new ExtremumDto() { Id = 8, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            extrema.AddRange(new ExtremumDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdateExtrema(extrema);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(2, 2);
            IEnumerable<ExtremumDto> actualRecords = repository.GetExtrema(queryDef);

            //Assert
            bool isEmptyCollection = (actualRecords.Count() == 0);
            Assert.IsTrue(isEmptyCollection);

        }

        [TestMethod]
        public void GetExtrema_ReturnsDataFromGivenStartDateOnly()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<ExtremumDto> extrema = new List<ExtremumDto>();
            ExtremumDto dto1 = new ExtremumDto() { Id = 1, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto2 = new ExtremumDto() { Id = 2, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto3 = new ExtremumDto() { Id = 3, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto4 = new ExtremumDto() { Id = 4, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto5 = new ExtremumDto() { Id = 5, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto6 = new ExtremumDto() { Id = 6, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto7 = new ExtremumDto() { Id = 7, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto8 = new ExtremumDto() { Id = 8, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            extrema.AddRange(new ExtremumDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdateExtrema(extrema);

            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1) { StartDate = new DateTime(2016, 1, 15, 22, 35, 0) };
            IEnumerable<ExtremumDto> actualRecords = repository.GetExtrema(queryDef);

            //Assert
            IEnumerable<ExtremumDto> expectedRecords = new ExtremumDto[] { dto3, dto4 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetExtrema_ReturnsDataToGivenEndDateOnly()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<ExtremumDto> extrema = new List<ExtremumDto>();
            ExtremumDto dto1 = new ExtremumDto() { Id = 1, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto2 = new ExtremumDto() { Id = 2, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto3 = new ExtremumDto() { Id = 3, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto4 = new ExtremumDto() { Id = 4, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto5 = new ExtremumDto() { Id = 5, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto6 = new ExtremumDto() { Id = 6, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto7 = new ExtremumDto() { Id = 7, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto8 = new ExtremumDto() { Id = 8, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            extrema.AddRange(new ExtremumDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdateExtrema(extrema);


            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1) { EndDate = new DateTime(2016, 1, 15, 22, 35, 0) };
            IEnumerable<ExtremumDto> actualRecords = repository.GetExtrema(queryDef);

            //Assert
            IEnumerable<ExtremumDto> expectedRecords = new ExtremumDto[] { dto1, dto2, dto3 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }


        [TestMethod]
        public void GetExtrema_ReturnsDataFromGivenStartDateAndToGivenEndDateOnly()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<ExtremumDto> extrema = new List<ExtremumDto>();
            ExtremumDto dto1 = new ExtremumDto() { Id = 1, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto2 = new ExtremumDto() { Id = 2, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto3 = new ExtremumDto() { Id = 3, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto4 = new ExtremumDto() { Id = 4, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto5 = new ExtremumDto() { Id = 5, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto6 = new ExtremumDto() { Id = 6, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto7 = new ExtremumDto() { Id = 7, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto8 = new ExtremumDto() { Id = 8, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            extrema.AddRange(new ExtremumDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdateExtrema(extrema);


            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1)
            {
                StartDate = new DateTime(2016, 1, 15, 22, 30, 0),
                EndDate = new DateTime(2016, 1, 15, 22, 35, 0)
            };
            IEnumerable<ExtremumDto> actualRecords = repository.GetExtrema(queryDef);

            //Assert
            IEnumerable<ExtremumDto> expectedRecords = new ExtremumDto[] { dto2, dto3 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetExtrema_ReturnsEmptyContainer_IfThereIsNoDataInGivenDateRange()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<ExtremumDto> extrema = new List<ExtremumDto>();
            ExtremumDto dto1 = new ExtremumDto() { Id = 1, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto2 = new ExtremumDto() { Id = 2, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto3 = new ExtremumDto() { Id = 3, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto4 = new ExtremumDto() { Id = 4, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto5 = new ExtremumDto() { Id = 5, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto6 = new ExtremumDto() { Id = 6, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto7 = new ExtremumDto() { Id = 7, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto8 = new ExtremumDto() { Id = 8, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            extrema.AddRange(new ExtremumDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdateExtrema(extrema);


            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1)
            {
                StartDate = new DateTime(2016, 1, 15, 23, 30, 0),
                EndDate = new DateTime(2016, 1, 15, 22, 35, 0)
            };
            IEnumerable<ExtremumDto> actualRecords = repository.GetExtrema(queryDef);

            //Assert
            bool isEmpty = actualRecords.Count() == 0;
            Assert.IsTrue(isEmpty);

        }

        [TestMethod]
        public void GetExtrema_ReturnsOnlyLimitedNumberOfPricesFromLeft_IfStartDateIsGiven()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<ExtremumDto> extrema = new List<ExtremumDto>();
            ExtremumDto dto1 = new ExtremumDto() { Id = 1, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto2 = new ExtremumDto() { Id = 2, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto3 = new ExtremumDto() { Id = 3, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto4 = new ExtremumDto() { Id = 4, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto5 = new ExtremumDto() { Id = 5, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto6 = new ExtremumDto() { Id = 6, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto7 = new ExtremumDto() { Id = 7, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto8 = new ExtremumDto() { Id = 8, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            extrema.AddRange(new ExtremumDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdateExtrema(extrema);


            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1)
            {
                StartDate = new DateTime(2016, 1, 15, 22, 25, 0),
                EndDate = new DateTime(2016, 1, 15, 22, 35, 0),
                Limit = 2
            };
            IEnumerable<ExtremumDto> actualRecords = repository.GetExtrema(queryDef);

            //Assert
            IEnumerable<ExtremumDto> expectedRecords = new ExtremumDto[] { dto1, dto2 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetExtrema_ReturnsOnlyLimitedNumberOfPricesFromRight_IfOnlyEndDateIsGiven()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<ExtremumDto> extrema = new List<ExtremumDto>();
            ExtremumDto dto1 = new ExtremumDto() { Id = 1, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto2 = new ExtremumDto() { Id = 2, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto3 = new ExtremumDto() { Id = 3, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto4 = new ExtremumDto() { Id = 4, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto5 = new ExtremumDto() { Id = 5, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto6 = new ExtremumDto() { Id = 6, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto7 = new ExtremumDto() { Id = 7, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto8 = new ExtremumDto() { Id = 8, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            extrema.AddRange(new ExtremumDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdateExtrema(extrema);


            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1)
            {
                EndDate = new DateTime(2016, 1, 15, 22, 35, 0),
                Limit = 2
            };
            IEnumerable<ExtremumDto> actualRecords = repository.GetExtrema(queryDef);

            //Assert
            IEnumerable<ExtremumDto> expectedRecords = new ExtremumDto[] { dto2, dto3 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetExtrema_ReturnsOnlyLimitedNumberOfPricesFromLeft_IfNoDateIsGiven()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<ExtremumDto> extrema = new List<ExtremumDto>();
            ExtremumDto dto1 = new ExtremumDto() { Id = 1, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto2 = new ExtremumDto() { Id = 2, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto3 = new ExtremumDto() { Id = 3, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto4 = new ExtremumDto() { Id = 4, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto5 = new ExtremumDto() { Id = 5, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto6 = new ExtremumDto() { Id = 6, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto7 = new ExtremumDto() { Id = 7, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto8 = new ExtremumDto() { Id = 8, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            extrema.AddRange(new ExtremumDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdateExtrema(extrema);


            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1)
            {
                Limit = 2
            };
            IEnumerable<ExtremumDto> actualRecords = repository.GetExtrema(queryDef);

            //Assert
            IEnumerable<ExtremumDto> expectedRecords = new ExtremumDto[] { dto1, dto2 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        [TestMethod]
        public void GetExtrema_ReturnsPrices_EvenIfPriceIsNotIncludedInAnalysisTypeParamOfQueryDef()
        {

            //Arrange
            EFPriceRepository repository = new EFPriceRepository();
            List<ExtremumDto> extrema = new List<ExtremumDto>();
            ExtremumDto dto1 = new ExtremumDto() { Id = 1, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto2 = new ExtremumDto() { Id = 2, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto3 = new ExtremumDto() { Id = 3, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto4 = new ExtremumDto() { Id = 4, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 1, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto5 = new ExtremumDto() { Id = 5, Date = new DateTime(2016, 1, 15, 22, 25, 0), IndexNumber = 1, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto6 = new ExtremumDto() { Id = 6, Date = new DateTime(2016, 1, 15, 22, 30, 0), IndexNumber = 2, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto7 = new ExtremumDto() { Id = 7, Date = new DateTime(2016, 1, 15, 22, 35, 0), IndexNumber = 3, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            ExtremumDto dto8 = new ExtremumDto() { Id = 8, Date = new DateTime(2016, 1, 15, 22, 40, 0), IndexNumber = 4, AssetId = 2, TimeframeId = 1, LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), ExtremumType = 1, Volatility = 1.23, EarlierCounter = 15, EarlierAmplitude = 7.45, EarlierChange1 = 1.12, EarlierChange2 = 2.21, EarlierChange3 = 3.12, EarlierChange5 = 4.56, EarlierChange10 = 5.28, LaterCounter = 16, LaterAmplitude = 1.23, LaterChange1 = 0.72, LaterChange2 = 0.54, LaterChange3 = 1.57, LaterChange5 = 2.41, LaterChange10 = 3.15, IsOpen = true, Timestamp = DateTime.Now, Value = 123.42 };
            extrema.AddRange(new ExtremumDto[] { dto1, dto2, dto3, dto4, dto5, dto6, dto7, dto8 });
            clearPricesTable();
            repository.UpdateExtrema(extrema);


            //Act
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1)
            {
                AnalysisTypes = new AnalysisType[] { AnalysisType.Macd, AnalysisType.Adx }
            };
            IEnumerable<ExtremumDto> actualRecords = repository.GetExtrema(queryDef);

            //Assert
            IEnumerable<ExtremumDto> expectedRecords = new ExtremumDto[] { dto1, dto2, dto3, dto4 };
            bool areEqualArrays = expectedRecords.HasEqualItems(actualRecords);
            Assert.IsTrue(areEqualArrays);

        }

        #endregion GET_EXTREMA_WITH_QUERY_DEF


    }
}
