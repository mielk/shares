
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using Stock.Domain.Enums;
using Stock.DAL.TransferObjects;

namespace Stock_UnitTest.Stock.Domain
{
    [TestClass]
    public class ExtremumUnitTests
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
            var timeframe= getTimeframe(timeframeId);
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
            var timeframe= getTimeframe(timeframeId);
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
            var timeframe= getTimeframe(timeframeId);
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
            var timeframe= getTimeframe(timeframeId);
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
            var timeframe= getTimeframe(timeframeId);
            DateTime date = timeframe.AddTimeUnits(DEFAULT_BASE_DATE, indexNumber - 1);
            DataSet ds = new DataSet(assetId, timeframeId, date, indexNumber);
            Quotation q = new Quotation(ds) { Open = open, High = high, Low = low, Close = close, Volume = volume };
            Price p = new Price(ds);
            return ds;

        }

        #endregion INFRASTRUCTURE


        #region FROM_DTO

        [TestMethod]
        public void FromDto_ReturnsProperExtremumObject()
        {

            //Arrange

            ExtremumDto dto = new ExtremumDto() { 
                Id = 1,
                SimulationId = DEFAULT_SIMULATION_ID,
                Date = DEFAULT_BASE_DATE,
                IndexNumber = DEFAULT_INDEX_NUMBER,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID, 
                LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0), 
                ExtremumType = 1, 
                Volatility = 1.23, 
                EarlierCounter = 15, 
                EarlierAmplitude = 7.45, 
                EarlierChange1 = 1.12, 
                EarlierChange2 = 2.21, 
                EarlierChange3 = 3.12, 
                EarlierChange5 = 4.56, 
                EarlierChange10 = 5.28, 
                LaterCounter = 16, 
                LaterAmplitude = 1.23, 
                LaterChange1 = 0.72, 
                LaterChange2 = 0.54, 
                LaterChange3 = 1.57, 
                LaterChange5 = 2.41, 
                LaterChange10 = 3.15, 
                IsOpen = true, 
                Timestamp = DateTime.Now,
                Value = 123.42 
            };


            //Act
            Price price = getPrice(DEFAULT_INDEX_NUMBER);
            Extremum actualExtremum = Extremum.FromDto(price, dto);

            //Assert
            Extremum expectedExtremum = new Extremum(price, ExtremumType.PeakByClose)
            {
                ExtremumId = 1,
                SimulationId = DEFAULT_SIMULATION_ID,
                LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0),
                Volatility = 1.23,
                EarlierCounter = 15,
                EarlierAmplitude = 7.45,
                EarlierChange1 = 1.12,
                EarlierChange2 = 2.21,
                EarlierChange3 = 3.12,
                EarlierChange5 = 4.56,
                EarlierChange10 = 5.28,
                LaterCounter = 16,
                LaterAmplitude = 1.23,
                LaterChange1 = 0.72,
                LaterChange2 = 0.54,
                LaterChange3 = 1.57,
                LaterChange5 = 2.41,
                LaterChange10 = 3.15,
                Open = true,
                Value = 123.42 
            };
            Assert.AreEqual(expectedExtremum, actualExtremum);

        }

        #endregion FROM_DTO


        #region TO_DTO

        [TestMethod]
        public void ToDto_ReturnsProperExtremumDtoObject()
        {

            //Arrange
            Price price = getPrice(DEFAULT_INDEX_NUMBER);
            Extremum extremum = new Extremum(price, ExtremumType.PeakByClose)
            {
                ExtremumId = 1,
                SimulationId = DEFAULT_SIMULATION_ID,
                LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0),
                Volatility = 1.23,
                EarlierCounter = 15,
                EarlierAmplitude = 7.45,
                EarlierChange1 = 1.12,
                EarlierChange2 = 2.21,
                EarlierChange3 = 3.12,
                EarlierChange5 = 4.56,
                EarlierChange10 = 5.28,
                LaterCounter = 16,
                LaterAmplitude = 1.23,
                LaterChange1 = 0.72,
                LaterChange2 = 0.54,
                LaterChange3 = 1.57,
                LaterChange5 = 2.41,
                LaterChange10 = 3.15,
                Open = true,
                Value = 123.42
            };

            //Act
            ExtremumDto actualDto = extremum.ToDto();

            //Assert
            ExtremumDto expectedDto = new ExtremumDto()
            {
                Id = 1,
                SimulationId = DEFAULT_SIMULATION_ID,
                Date = DEFAULT_BASE_DATE,
                IndexNumber = DEFAULT_INDEX_NUMBER,
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0),
                ExtremumType = 1,
                Volatility = 1.23,
                EarlierCounter = 15,
                EarlierAmplitude = 7.45,
                EarlierChange1 = 1.12,
                EarlierChange2 = 2.21,
                EarlierChange3 = 3.12,
                EarlierChange5 = 4.56,
                EarlierChange10 = 5.28,
                LaterCounter = 16,
                LaterAmplitude = 1.23,
                LaterChange1 = 0.72,
                LaterChange2 = 0.54,
                LaterChange3 = 1.57,
                LaterChange5 = 2.41,
                LaterChange10 = 3.15,
                IsOpen = true,
                Value = 123.42
            };

            Assert.AreEqual(expectedDto, actualDto);

        }

        #endregion TO_DTO


        #region EQUALS

        private Extremum getDefaultExtremum()
        {
            Price price = getPrice(DEFAULT_INDEX_NUMBER);
            Extremum extremum = new Extremum(price, ExtremumType.PeakByClose)
            {
                ExtremumId = 1,
                LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0),
                Volatility = 1.23,
                EarlierCounter = 15,
                EarlierAmplitude = 7.45,
                EarlierChange1 = 1.12,
                EarlierChange2 = 2.21,
                EarlierChange3 = 3.12,
                EarlierChange5 = 4.56,
                EarlierChange10 = 5.28,
                LaterCounter = 16,
                LaterAmplitude = 1.23,
                LaterChange1 = 0.72,
                LaterChange2 = 0.54,
                LaterChange3 = 1.57,
                LaterChange5 = 2.41,
                LaterChange10 = 3.15,
                Open = true,
                Value = 123.42 
            };
            return extremum;
        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfComparedToObjectOfOtherType()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
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
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfISimulationdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.SimulationId++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfExtremumDateIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.Price.DataSet.Date = comparedItem.Price.DataSet.Date.AddMinutes(5);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfAssetIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.Price.DataSet.AssetId++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfTimeframeIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.Price.DataSet.TimeframeId++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfIndexNumberIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.Price.DataSet.IndexNumber++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfExtremumTypeIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            int comparedType = (int)comparedItem.Type;
            comparedType++;
            comparedItem.Type = (ExtremumType)comparedType;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfVolatilityIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.Volatility += -1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfEarlierCounterIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.EarlierCounter++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfEarlierAmplitudeIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.EarlierAmplitude += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfEarlierChange1IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.EarlierChange1 += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfEarlierChange2IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.EarlierChange2 += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfEarlierChange3IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.EarlierChange3 += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfEarlierChange5IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.EarlierChange5 += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }
        
        [TestMethod]
        public void Equals_ReturnsFalse_IfEarlierChange10IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.EarlierChange10 += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }


        [TestMethod]
        public void Equals_ReturnsFalse_IfLaterCounterIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.LaterCounter++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfLaterAmplitudeIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.LaterAmplitude += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfLaterChange1IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.LaterChange1 += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfLaterChange2IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.LaterChange2 += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfLaterChange3IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.LaterChange3 += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfLaterChange5IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.LaterChange5 += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfLaterChange10IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.LaterChange10 += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }


        [TestMethod]
        public void Equals_ReturnsFalse_IfIsOpenIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.Open = !comparedItem.Open;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfValueIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremum();
            var comparedItem = getDefaultExtremum();

            //Act
            comparedItem.Value += 2.15;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }


        #endregion EQUALS



    }
}
