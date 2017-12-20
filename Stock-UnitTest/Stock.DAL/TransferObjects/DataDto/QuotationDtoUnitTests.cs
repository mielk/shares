using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using System.Collections.Generic;
using Stock.DAL.TransferObjects;
using System.Linq;
using Stock.Domain.Services;
using Stock.Utils;
using Moq;

namespace Stock_UnitTest.Stock.Domain.Entities
{
    [TestClass]
    public class QuotationDtoUnitTests
    {

        private const int DEFAULT_ASSET_ID = 1;
        private const int DEFAULT_TIMEFRAME_ID = 1;
        private const int DEFAULT_INDEX_NUMBER = 10;

        #region COPY_PROPERTIES

        [TestMethod]
        public void CopyProperties_AfterwardAllPropertiesAreEqual()
        {

            //Arrange
            var baseItem = new QuotationDto()
            {
                QuotationId = 1,
                PriceDate = new DateTime(2017, 3, 4, 21, 10, 0),
                AssetId = 1,
                TimeframeId = 1,
                OpenPrice = 1.03,
                HighPrice = 1.04,
                LowPrice = 1.02,
                ClosePrice = 1.04,
                Volume = 100,
                IndexNumber = 51
            };

            var comparedItem = new QuotationDto()
            {
                QuotationId = 2,
                PriceDate = new DateTime(2017, 3, 4, 21, 10, 0),
                AssetId = 2,
                TimeframeId = 1,
                OpenPrice = 1.05,
                HighPrice = 1.02,
                LowPrice = 1.01,
                ClosePrice = 1.03,
                Volume = 103,
                IndexNumber = 52
            };

            //Act
            comparedItem.CopyProperties(baseItem);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }


        #endregion COPY_PROPERTIES


        #region EQUALS

        private QuotationDto getDefaultQuotationDto()
        {
            return new QuotationDto()
            {
                QuotationId = 1,
                PriceDate = new DateTime(2017, 3, 4, 21, 10, 0),
                AssetId = 1,
                TimeframeId = 1,
                OpenPrice = 1.03,
                HighPrice = 1.04,
                LowPrice = 1.02,
                ClosePrice = 1.04,
                Volume = 100,
                IndexNumber = 51
            };
        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfComparedToObjectOfOtherType()
        {

            //Arrange
            var baseItem = getDefaultQuotationDto();
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
            var baseItem = getDefaultQuotationDto();
            var comparedItem = getDefaultQuotationDto();
            
            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfQuotationIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultQuotationDto();
            var comparedItem = getDefaultQuotationDto();

            //Act
            comparedItem.QuotationId++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfPriceDateIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultQuotationDto();
            var comparedItem = getDefaultQuotationDto();

            //Act
            comparedItem.PriceDate = comparedItem.PriceDate.AddMinutes(5);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfAssetIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultQuotationDto();
            var comparedItem = getDefaultQuotationDto();

            //Act
            comparedItem.AssetId++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfTimeframeIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultQuotationDto();
            var comparedItem = getDefaultQuotationDto();

            //Act
            comparedItem.TimeframeId++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfIndexNumberIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultQuotationDto();
            var comparedItem = getDefaultQuotationDto();

            //Act
            comparedItem.IndexNumber++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOpenPriceIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultQuotationDto();
            var comparedItem = getDefaultQuotationDto();

            //Act
            comparedItem.OpenPrice += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfHighPriceIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultQuotationDto();
            var comparedItem = getDefaultQuotationDto();

            //Act
            comparedItem.HighPrice += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfLowPriceIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultQuotationDto();
            var comparedItem = getDefaultQuotationDto();

            //Act
            comparedItem.LowPrice += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfClosePriceIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultQuotationDto();
            var comparedItem = getDefaultQuotationDto();

            //Act
            comparedItem.ClosePrice += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfVolumeIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultQuotationDto();
            var comparedItem = getDefaultQuotationDto();

            //Act
            comparedItem.Volume++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        #endregion EQUALS


        private QuotationDto getDefaultQuotation_ForIsInIndexRangeTest()
        {
            return new QuotationDto()
            {
                AssetId = DEFAULT_ASSET_ID,
                TimeframeId = DEFAULT_TIMEFRAME_ID,
                IndexNumber = DEFAULT_INDEX_NUMBER,
                OpenPrice = 1,
                HighPrice = 1.1,
                LowPrice = 0.9,
                ClosePrice = 1
            };
        }

        [TestMethod]
        public void IsInIndexRange_ReturnsTrue_IfBothDelimitersAreNull()
        {

            //Arrange
            QuotationDto quotation = getDefaultQuotation_ForIsInIndexRangeTest();
            
            //Act
            var result = quotation.IsInIndexRange(null, null);

            //Assert
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void IsInIndexRange_ReturnsTrue_IfStartLimitIsNullAndEndLimitIsLater()
        {

            //Arrange
            QuotationDto quotation = getDefaultQuotation_ForIsInIndexRangeTest();

            //Act
            var result = quotation.IsInIndexRange(null, DEFAULT_INDEX_NUMBER + 5);

            //Assert
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void IsInIndexRange_ReturnsTrue_IfStartLimitIsNullAndEndLimitIsEqual()
        {

            //Arrange
            QuotationDto quotation = getDefaultQuotation_ForIsInIndexRangeTest();

            //Act
            var result = quotation.IsInIndexRange(null, DEFAULT_INDEX_NUMBER);

            //Assert
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void IsInIndexRange_ReturnsTrue_IfStartLimitIsEarlierAndEndLimitIsNull()
        {

            //Arrange
            QuotationDto quotation = getDefaultQuotation_ForIsInIndexRangeTest();

            //Act
            var result = quotation.IsInIndexRange(DEFAULT_INDEX_NUMBER - 2, null);

            //Assert
            Assert.IsTrue(result);

        }


        [TestMethod]
        public void IsInIndexRange_ReturnsTrue_IfStartLimitIsEarlierAndEndLimitIsLater()
        {

            //Arrange
            QuotationDto quotation = getDefaultQuotation_ForIsInIndexRangeTest();

            //Act
            var result = quotation.IsInIndexRange(DEFAULT_INDEX_NUMBER - 5, DEFAULT_INDEX_NUMBER + 5);

            //Assert
            Assert.IsTrue(result);

        }


        [TestMethod]
        public void IsInIndexRange_ReturnsTrue_IfStartLimitIsEarlierAndEndLimitIsEqual()
        {

            //Arrange
            QuotationDto quotation = getDefaultQuotation_ForIsInIndexRangeTest();

            //Act
            var result = quotation.IsInIndexRange(DEFAULT_INDEX_NUMBER - 5, DEFAULT_INDEX_NUMBER);

            //Assert
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void IsInIndexRange_ReturnsTrue_IfStartLimitIsEqualAndEndLimitIsNull()
        {

            //Arrange
            QuotationDto quotation = getDefaultQuotation_ForIsInIndexRangeTest();

            //Act
            var result = quotation.IsInIndexRange(DEFAULT_INDEX_NUMBER, null);

            //Assert
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void IsInIndexRange_ReturnsTrue_IfStartLimitIsEqualAndEndLimitIsEqual()
        {

            //Arrange
            QuotationDto quotation = getDefaultQuotation_ForIsInIndexRangeTest();

            //Act
            var result = quotation.IsInIndexRange(DEFAULT_INDEX_NUMBER, DEFAULT_INDEX_NUMBER);

            //Assert
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void IsInIndexRange_ReturnsTrue_IfStartLimitIsEqualAndEndLimitIsLater()
        {

            //Arrange
            QuotationDto quotation = getDefaultQuotation_ForIsInIndexRangeTest();

            //Act
            var result = quotation.IsInIndexRange(DEFAULT_INDEX_NUMBER, DEFAULT_INDEX_NUMBER + 5);

            //Assert
            Assert.IsTrue(result);

        }




        [TestMethod]
        public void IsInIndexRange_ReturnsFalse_IfStartLimitIsLaterAndEndLimitIsEarlier()
        {

            //Arrange
            QuotationDto quotation = getDefaultQuotation_ForIsInIndexRangeTest();

            //Act
            var result = quotation.IsInIndexRange(DEFAULT_INDEX_NUMBER + 1, DEFAULT_INDEX_NUMBER - 1);

            //Assert
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsInIndexRange_ReturnsFalse_IfStartLimitIsLaterAndEndLimitIsNull()
        {

            //Arrange
            QuotationDto quotation = getDefaultQuotation_ForIsInIndexRangeTest();

            //Act
            var result = quotation.IsInIndexRange(DEFAULT_INDEX_NUMBER + 5, null);

            //Assert
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsInIndexRange_ReturnsFalse_IfStartLimitIsLaterAndEndLimitIsLater()
        {

            //Arrange
            QuotationDto quotation = getDefaultQuotation_ForIsInIndexRangeTest();

            //Act
            var result = quotation.IsInIndexRange(DEFAULT_INDEX_NUMBER + 1, DEFAULT_INDEX_NUMBER + 1);

            //Assert
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsInIndexRange_ReturnsFalse_IfStartLimitIsLaterAndEndLimitIsEqual()
        {

            //Arrange
            QuotationDto quotation = getDefaultQuotation_ForIsInIndexRangeTest();

            //Act
            var result = quotation.IsInIndexRange(DEFAULT_INDEX_NUMBER + 1, DEFAULT_INDEX_NUMBER);

            //Assert
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsInIndexRange_ReturnsFalse_IfStartLimitIsNullAndEndLimitIsEarlier()
        {

            //Arrange
            QuotationDto quotation = getDefaultQuotation_ForIsInIndexRangeTest();

            //Act
            var result = quotation.IsInIndexRange(null, DEFAULT_INDEX_NUMBER - 1);

            //Assert
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsInIndexRange_ReturnsFalse_IfStartLimitIsEqualAndEndLimitIsEarlier()
        {

            //Arrange
            QuotationDto quotation = getDefaultQuotation_ForIsInIndexRangeTest();

            //Act
            var result = quotation.IsInIndexRange(DEFAULT_INDEX_NUMBER, DEFAULT_INDEX_NUMBER - 1);

            //Assert
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void IsInIndexRange_ReturnsFalse_IfStartLimitIsEarlierAndEndLimitIsEarlier()
        {

            //Arrange
            QuotationDto quotation = getDefaultQuotation_ForIsInIndexRangeTest();

            //Act
            var result = quotation.IsInIndexRange(DEFAULT_INDEX_NUMBER - 1, DEFAULT_INDEX_NUMBER - 1);

            //Assert
            Assert.IsFalse(result);
        }

    }

}
