using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using Stock.Domain.Enums;
using Stock.DAL.TransferObjects;

namespace Stock_UnitTest.Stock.Domain
{
    [TestClass]
    public class DataSetUnitTests
    {

        private DateTime DEFAULT_DATETIME = new DateTime(2017, 3, 4, 21, 10, 0);
        private const int DEFAULT_ASSET_ID = 1;
        private const int DEFAULT_TIMEFRAME_ID = 1;
        private const int DEFAULT_INDEX_NUMBER = 1;



        #region INFRASTRUCTURE

        private DataSet getDefaultDataSet()
        {
            return new DataSet(DEFAULT_ASSET_ID, DEFAULT_TIMEFRAME_ID, DEFAULT_DATETIME, DEFAULT_INDEX_NUMBER);
        }

        private Quotation getDefaultQuotation(DataSet ds)
        {
            return new Quotation(ds)
            {
                Id = 1,
                Open = 1.234,
                High = 1.45,
                Low = 1.11,
                Close = 1.42,
                Volume = 200
            };
        }

        private Price getDefaultPrice(DataSet ds)
        {
            return new Price(ds)
            {
                Id = 1,
                CloseDelta = 1.04,
                Direction2D = 1,
                Direction3D = 1,
                PriceGap = 0.05,
                CloseRatio = 0.23,
                ExtremumRatio = 1,
            };
        }

        #endregion INFRASTRUCTURE



        #region EQUALS

        [TestMethod]
        public void Equals_ReturnsFalse_IfComparedToObjectOfOtherType()
        {

            //Arrange
            var baseItem = getDefaultDataSet();
            var comparedItem = new { Id = 1 };

            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfAllPropertiesAreEqualAndAllObjectsAreNull()
        {

            //Arrange
            var baseItem = getDefaultDataSet();
            var comparedItem = getDefaultDataSet();

            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfDateIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultDataSet();
            var comparedItem = getDefaultDataSet();

            //Act
            comparedItem.Date = comparedItem.Date.AddMinutes(5);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfAssetIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultDataSet();
            var comparedItem = getDefaultDataSet();

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
            var baseItem = getDefaultDataSet();
            var comparedItem = getDefaultDataSet();

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
            var baseItem = getDefaultDataSet();
            var comparedItem = getDefaultDataSet();

            //Act
            comparedItem.IndexNumber++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfAllPropertiesTheSameAndBothHaveEqualQuotation()
        {

            //Arrange
            var baseItem = getDefaultDataSet();
            var comparedItem = getDefaultDataSet();

            //Act
            baseItem.SetQuotation(getDefaultQuotation(baseItem));
            comparedItem.SetQuotation(getDefaultQuotation(comparedItem));
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfAllPropertiesTheSameButBothHaveDifferentQuotation()
        {

            //Arrange
            var baseItem = getDefaultDataSet();
            var comparedItem = getDefaultDataSet();

            //Act
            Quotation baseQuotation = getDefaultQuotation(baseItem);
            Quotation comparedQuotation = getDefaultQuotation(comparedItem);
            comparedQuotation.Open += 1;
            baseItem.SetQuotation(baseQuotation);
            comparedItem.SetQuotation(comparedQuotation);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfAllPropertiesTheSameButOnlyBaseItemHaveNullQuotation()
        {

            //Arrange
            var baseItem = getDefaultDataSet();
            var comparedItem = getDefaultDataSet();

            //Act
            comparedItem.SetQuotation(getDefaultQuotation(comparedItem));
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfAllPropertiesTheSameButOnlyComparedItemHaveNullQuotation()
        {

            //Arrange
            var baseItem = getDefaultDataSet();
            var comparedItem = getDefaultDataSet();

            //Act
            baseItem.SetQuotation(getDefaultQuotation(baseItem));
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfAllPropertiesTheSameAndBothHaveEqualPrice()
        {

            //Arrange
            var baseItem = getDefaultDataSet();
            var comparedItem = getDefaultDataSet();

            //Act
            baseItem.SetPrice(getDefaultPrice(baseItem));
            comparedItem.SetPrice(getDefaultPrice(comparedItem));
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfAllPropertiesTheSameButBothHaveDifferentPrice()
        {

            //Arrange
            var baseItem = getDefaultDataSet();
            var comparedItem = getDefaultDataSet();

            //Act
            Price basePrice = getDefaultPrice(baseItem);
            Price comparedPrice = getDefaultPrice(comparedItem);
            basePrice.CloseDelta = 1;
            comparedPrice.CloseDelta = 2;
            baseItem.SetPrice(basePrice);
            comparedItem.SetPrice(comparedPrice);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfAllPropertiesTheSameButOnlyBaseItemHaveNullPrice()
        {

            //Arrange
            var baseItem = getDefaultDataSet();
            var comparedItem = getDefaultDataSet();

            //Act
            comparedItem.SetPrice(getDefaultPrice(comparedItem));
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfAllPropertiesTheSameButOnlyComparedItemHaveNullPrice()
        {

            //Arrange
            var baseItem = getDefaultDataSet();
            var comparedItem = getDefaultDataSet();

            //Act
            baseItem.SetPrice(getDefaultPrice(baseItem));
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        #endregion EQUALS


        #region SETTING_SUBOBJECTS

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "It is not allowed to set getQuotation with different DataSet to a given DataSet")]
        public void SetQuotation_IfTryToSetQuotationWithDifferentDataSet_RaiseException()
        {

            //Arrange.
            DataSet ds = getDefaultDataSet();
            DataSet ds2 = new DataSet(DEFAULT_ASSET_ID + 1, DEFAULT_TIMEFRAME_ID + 1, DEFAULT_DATETIME, DEFAULT_TIMEFRAME_ID + 1);

            //Act.
            Quotation quotation = new Quotation(ds2);
            ds.SetQuotation(quotation);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "It is not allowed to set getPrice with different DataSet to a given DataSet")]
        public void SetPrice_IfTryToSetPriceWithDifferentDataSet_RaiseException()
        {

            //Arrange.
            DataSet ds = getDefaultDataSet();
            DataSet ds2 = new DataSet(DEFAULT_ASSET_ID + 1, DEFAULT_TIMEFRAME_ID + 1, DEFAULT_DATETIME, DEFAULT_TIMEFRAME_ID + 1);

            //Act.
            Price price = new Price(ds2);
            ds.SetPrice(price);

        }


        #endregion SETTING_SUBOBJECTS

    }
}
