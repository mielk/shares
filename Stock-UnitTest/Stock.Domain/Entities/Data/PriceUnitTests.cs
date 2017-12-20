using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using Stock.Domain.Enums;
using Stock.DAL.TransferObjects;

namespace Stock_UnitTest.Stock.Domain
{
    [TestClass]
    public class PriceUnitTests
    {

        private const int DEFAULT_ASSET_ID = 1;
        private const int DEFAULT_TIMEFRAME_ID = 1;
        private const int DEFAULT_SIMULATION_ID = 1;
        private const int DEFAULT_INDEX_NUMBER = 1;
        private DateTime DEFAULT_BASE_DATE = new DateTime(2017, 5, 2, 12, 15, 0);



        #region INFRASTRUCTURE

        private DataSet getDataSet(int indexNumber)
        {
            return getDataSet(DEFAULT_ASSET_ID, DEFAULT_TIMEFRAME_ID, indexNumber);
        }

        private DataSet getDataSet(int assetId, int timeframeId, int indexNumber)
        {
            var timeframe = getTimeframe(timeframeId);
            DateTime date = timeframe.AddTimeUnits(DEFAULT_BASE_DATE, indexNumber - 1);
            DataSet ds = new DataSet(assetId, timeframeId, date, indexNumber);
            return ds;
        }


        private Quotation getQuotation(DataSet ds)
        {
            return new Quotation(ds)
            {
                Id = ds.IndexNumber,
                Open = 1.09191,
                High = 1.09218,
                Low = 1.09186,
                Close = 1.09194,
                Volume = 1411
            };
        }

        private Quotation getQuotation(int indexNumber)
        {
            return getQuotation(DEFAULT_ASSET_ID, DEFAULT_TIMEFRAME_ID, indexNumber);
        }

        private Quotation getQuotation(int assetId, int timeframeId, int indexNumber)
        {
            DataSet ds = getDataSet(assetId, timeframeId, indexNumber);
            return new Quotation(ds)
            {
                Id = indexNumber,
                Open = 1.09191,
                High = 1.09218,
                Low = 1.09186,
                Close = 1.09194,
                Volume = 1411
            };
        }

        private QuotationDto getQuotationDto(int indexNumber)
        {
            return getQuotationDto(DEFAULT_ASSET_ID, DEFAULT_TIMEFRAME_ID, indexNumber);
        }

        private QuotationDto getQuotationDto(int assetId, int timeframeId, int indexNumber)
        {
            var timeframe = getTimeframe(timeframeId);
            DateTime date = timeframe.AddTimeUnits(DEFAULT_BASE_DATE, indexNumber - 1);
            return new QuotationDto()
            {
                PriceDate = date,
                AssetId = assetId,
                TimeframeId = timeframeId,
                OpenPrice = 1.09191,
                HighPrice = 1.09218,
                LowPrice = 1.09186,
                ClosePrice = 1.09194,
                Volume = 1411
            };
        }



        private Price getPrice(DataSet ds)
        {
            return getPrice(ds);
        }

        private Price getPrice(int indexNumber)
        {
            return getPrice(DEFAULT_ASSET_ID, DEFAULT_TIMEFRAME_ID, indexNumber);
        }

        private Price getPrice(int assetId, int timeframeId, int indexNumber)
        {
            DataSet ds = getDataSet(assetId, timeframeId, indexNumber);
            return new Price(ds)
            {
                Id = indexNumber,
                CloseDelta = 1.05,
                Direction2D = 1,
                Direction3D = 0,
                PriceGap = 1.23,
                CloseRatio = 1.23,
                ExtremumRatio = 2.34
            };
        }

        private PriceDto getPriceDto(int indexNumber)
        {
            return getPriceDto(DEFAULT_ASSET_ID, DEFAULT_TIMEFRAME_ID, indexNumber);
        }

        private PriceDto getPriceDto(int assetId, int timeframeId, int indexNumber)
        {
            var timeframe = getTimeframe(timeframeId);
            DateTime date = timeframe.AddTimeUnits(DEFAULT_BASE_DATE, indexNumber - 1);
            return new PriceDto()
            {
                Id = indexNumber,
                PriceDate = date,
                AssetId = assetId,
                TimeframeId = timeframeId,
                IndexNumber = indexNumber,
                DeltaClosePrice = 1.04,
                PriceDirection2D = 1,
                PriceDirection3D = 1,
                PriceGap = 0.05,
                CloseRatio = 0.23,
                ExtremumRatio = 1
            };
        }

        private Timeframe getTimeframe(int timeframeId)
        {
            return new Timeframe(timeframeId, "5M", TimeframeUnit.Minutes, 5);
        }

        private DataSet getDataSetWithQuotation(int indexNumber, double open, double high, double low, double close, double volume)
        {
            return getDataSetWithQuotation(DEFAULT_ASSET_ID, DEFAULT_TIMEFRAME_ID, indexNumber, open, high, low, close, volume);
        }

        private DataSet getDataSetWithQuotation(int assetId, int timeframeId, int indexNumber, double open, double high, double low, double close, double volume)
        {
            var timeframe = getTimeframe(timeframeId);
            DateTime date = timeframe.AddTimeUnits(DEFAULT_BASE_DATE, indexNumber - 1);
            DataSet ds = new DataSet(assetId, timeframeId, date, indexNumber);
            Quotation q = new Quotation(ds) { Open = open, High = high, Low = low, Close = close, Volume = volume };
            return ds;

        }

        private DataSet getDataSetWithQuotationAndPrice(int indexNumber, double open, double high, double low, double close, double volume)
        {
            return getDataSetWithQuotationAndPrice(DEFAULT_ASSET_ID, DEFAULT_TIMEFRAME_ID, indexNumber, open, high, low, close, volume);
        }

        private DataSet getDataSetWithQuotationAndPrice(int assetId, int timeframeId, int indexNumber, double open, double high, double low, double close, double volume)
        {
            var timeframe = getTimeframe(timeframeId);
            DateTime date = timeframe.AddTimeUnits(DEFAULT_BASE_DATE, indexNumber - 1);
            DataSet ds = new DataSet(assetId, timeframeId, date, indexNumber);
            Quotation q = new Quotation(ds) { Open = open, High = high, Low = low, Close = close, Volume = volume };
            Price p = new Price(ds);
            return ds;

        }

        #endregion INFRASTRUCTURE



        #region FROM_DTO

        [TestMethod]
        public void FromDto_ReturnsProperPriceObject()
        {

            //Arrange
            PriceDto dto = new PriceDto()
            {
                Id = 1,
                PriceDate = DEFAULT_BASE_DATE,
                IndexNumber = DEFAULT_INDEX_NUMBER,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                DeltaClosePrice = 1.15,
                PriceDirection2D = 1,
                PriceDirection3D = 0,
                PriceGap = 0,
                CloseRatio = 1.45,
                ExtremumRatio = 2.12
            };

            //Act
            DataSet ds = getDataSet(DEFAULT_INDEX_NUMBER);
            Price actualPrice = Price.FromDto(ds, dto);

            //Assert
            Price expectedPrice = new Price(ds)
            {
                Id = 1,
                CloseDelta = 1.15,
                Direction2D = 1,
                Direction3D = 0,
                PriceGap = 0,
                CloseRatio = 1.45,
                ExtremumRatio = 2.12
            };
            Assert.AreEqual(expectedPrice, actualPrice);

        }

        #endregion FROM_DTO


        #region TO_DTO

        [TestMethod]
        public void ToDto_ReturnsProperPriceDtoObject()
        {

            //Act
            DataSet ds = getDataSet(DEFAULT_INDEX_NUMBER);
            Price price = new Price(ds)
            {
                Id = 1,
                CloseDelta = 1.15,
                Direction2D = 1,
                Direction3D = 0,
                PriceGap = 0,
                CloseRatio = 1.45,
                ExtremumRatio = 2.12
            };
            Extremum peakByClose = new Extremum(price, ExtremumType.PeakByClose);

            //Act
            PriceDto actualDto = price.ToDto();

            //Assert
            PriceDto expectedDto = new PriceDto()
            {
                Id = 1,
                PriceDate = DEFAULT_BASE_DATE,
                IndexNumber = DEFAULT_INDEX_NUMBER,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                DeltaClosePrice = 1.15,
                PriceDirection2D = 1,
                PriceDirection3D = 0,
                PriceGap = 0,
                CloseRatio = 1.45,
                ExtremumRatio = 2.12
            };

            Assert.AreEqual(expectedDto, actualDto);

        }

        #endregion TO_DTO


        #region EQUALS

        [TestMethod]
        public void Equals_ReturnsFalse_IfComparedToObjectOfOtherType()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = new { Id = 1 };

            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfAllPropertiesAreEqual()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfIdIsDifferent()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            comparedItem.Id++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfISimulationdIsDifferent()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            comparedItem.SimulationId++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfPriceDateIsDifferent()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            comparedItem.DataSet.Date = comparedItem.DataSet.Date.AddMinutes(5);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfAssetIdIsDifferent()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            comparedItem.DataSet.AssetId++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfTimeframeIdIsDifferent()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            comparedItem.DataSet.TimeframeId++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfIndexNumberIsDifferent()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            comparedItem.DataSet.IndexNumber++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfDeltaClosePriceIsDifferent()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            comparedItem.CloseDelta += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfDirection2DIsDifferent()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            comparedItem.Direction2D *= -1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfDirection3DIsDifferent()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            baseItem.Direction3D = 1;
            comparedItem.Direction3D = -1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfPriceGapIsDifferent()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            comparedItem.PriceGap += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfPeakByCloseIsDifferent()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            baseItem.PeakByClose = new Extremum(baseItem, ExtremumType.PeakByClose);
            comparedItem.PeakByClose = new Extremum(comparedItem, ExtremumType.PeakByClose);
            baseItem.PeakByClose.Value = 2;
            comparedItem.PeakByClose.Value = 1;

            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfPeakByCloseIsNullOnlyInBasePrice()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            comparedItem.PeakByClose = new Extremum(comparedItem, ExtremumType.PeakByClose);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfPeakByCloseIsNullOnlyInComparedPrice()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            baseItem.PeakByClose = new Extremum(baseItem, ExtremumType.PeakByClose);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfPeakByCloseIsTheSameInBothPrices()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            baseItem.PeakByClose = new Extremum(baseItem, ExtremumType.PeakByClose);
            comparedItem.PeakByClose = new Extremum(comparedItem, ExtremumType.PeakByClose); 
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfPeakByHighIsDifferent()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            baseItem.PeakByHigh = new Extremum(baseItem, ExtremumType.PeakByHigh);
            comparedItem.PeakByHigh = new Extremum(comparedItem, ExtremumType.PeakByHigh);
            baseItem.PeakByHigh.Value = 2;
            comparedItem.PeakByHigh.Value = 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfPeakByHighIsNullOnlyInBasePrice()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            comparedItem.PeakByHigh = new Extremum(comparedItem, ExtremumType.PeakByHigh);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfPeakByHighIsNullOnlyInComparedPrice()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            baseItem.PeakByHigh = new Extremum(baseItem, ExtremumType.PeakByHigh);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfPeakByHighIsTheSameInBothPrices()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            baseItem.PeakByHigh = new Extremum(baseItem, ExtremumType.PeakByHigh);
            comparedItem.PeakByHigh = new Extremum(comparedItem, ExtremumType.PeakByHigh); 
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfTroughByCloseIsDifferent()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            baseItem.TroughByClose = new Extremum(baseItem, ExtremumType.TroughByClose);
            comparedItem.TroughByClose = new Extremum(comparedItem, ExtremumType.TroughByClose);
            baseItem.TroughByClose.Value = 1;
            comparedItem.TroughByClose.Value = 2;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfTroughByCloseIsNullOnlyInBasePrice()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            comparedItem.TroughByClose = new Extremum(comparedItem, ExtremumType.TroughByClose);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfTroughByCloseIsNullOnlyInComparedPrice()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            baseItem.TroughByClose = new Extremum(baseItem, ExtremumType.TroughByClose);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfTroughByCloseIsTheSameInBothPrices()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            baseItem.TroughByClose = new Extremum(baseItem, ExtremumType.TroughByClose);
            comparedItem.TroughByClose = new Extremum(comparedItem, ExtremumType.TroughByClose);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfTroughByLowIsDifferent()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            baseItem.TroughByLow = new Extremum(baseItem, ExtremumType.TroughByLow);
            comparedItem.TroughByLow = new Extremum(comparedItem, ExtremumType.TroughByLow);
            baseItem.TroughByLow.Value = 1;
            comparedItem.TroughByLow.Value = 2;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfTroughByLowIsNullOnlyInBasePrice()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            comparedItem.TroughByLow = new Extremum(comparedItem, ExtremumType.TroughByLow);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfTroughByLowIsNullOnlyInComparedPrice()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            baseItem.TroughByLow = new Extremum(baseItem, ExtremumType.TroughByLow);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfTroughByLowIsTheSameInBothPrices()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            baseItem.TroughByLow = new Extremum(baseItem, ExtremumType.TroughByLow);
            comparedItem.TroughByLow = new Extremum(comparedItem, ExtremumType.TroughByLow);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfCloseRatioIsDifferent()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            comparedItem.CloseRatio += 0.15;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfExtremumRatioIsDifferent()
        {

            //Arrange
            var baseItem = getPrice(1);
            var comparedItem = getPrice(1);

            //Act
            comparedItem.ExtremumRatio += 0.15;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        #endregion EQUALS


        #region SETTERS

        [TestMethod]
        public void PriceSetExtremum_AfterSettingPeakByClose_ProperExtremumIsAssigned()
        {

            //Arrange
            Price price = getPrice(1);

            //Act
            Extremum peakByClose = new Extremum(price, ExtremumType.PeakByClose);
            price.SetExtremum(peakByClose);

            //Assert
            Assert.IsTrue(peakByClose == price.PeakByClose);

        }

        [TestMethod]
        public void PriceSetExtremum_AfterSettingPeakByHigh_ProperExtremumIsAssigned()
        {

            //Arrange
            Price price = getPrice(1);

            //Act
            Extremum peakByHigh = new Extremum(price, ExtremumType.PeakByHigh);
            price.SetExtremum(peakByHigh);

            //Assert
            Assert.IsTrue(peakByHigh == price.PeakByHigh);

        }

        [TestMethod]
        public void PriceSetExtremum_AfterSettingTroughByClose_ProperExtremumIsAssigned()
        {

            //Arrange
            Price price = getPrice(1);

            //Act
            Extremum troughByClose = new Extremum(price, ExtremumType.TroughByClose);
            price.SetExtremum(troughByClose);

            //Assert
            Assert.IsTrue(troughByClose == price.TroughByClose);

        }

        [TestMethod]
        public void PriceSetExtremum_AfterSettingTroughByLow_ProperExtremumIsAssigned()
        {

            //Arrange
            Price price = getPrice(1);

            //Act
            Extremum troughByLow = new Extremum(price, ExtremumType.TroughByLow);
            price.SetExtremum(troughByLow);

            //Assert
            Assert.IsTrue(troughByLow == price.TroughByLow);

        }
        #endregion SETTERS



    }
}
