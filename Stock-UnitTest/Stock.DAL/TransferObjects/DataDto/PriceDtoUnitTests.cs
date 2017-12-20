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
    public class PriceDtoUnitTests
    {

        private PriceDto getDefaultPriceDto()
        {
            return new PriceDto()
            {
                Id = 1,
                PriceDate = new DateTime(2017, 3, 4, 21, 10, 0),
                AssetId = 1,
                TimeframeId = 1,
                DeltaClosePrice = 1.04,
                PriceDirection2D = 1,
                PriceDirection3D = 1,
                PriceGap = 0.05,
                CloseRatio = 0.23,
                ExtremumRatio = 1,
                IndexNumber = 51
            };
        }


        #region COPY_PROPERTIES

        [TestMethod]
        public void CopyProperties_AfterwardAllPropertiesAreEqual()
        {

            //Arrange
            var baseItem = new PriceDto()
            {
                Id = 1,
                PriceDate = new DateTime(2017, 3, 4, 21, 10, 0),
                AssetId = 1,
                TimeframeId = 1,
                DeltaClosePrice = 1.04,
                PriceDirection2D = 1,
                PriceDirection3D = 1,
                PriceGap = 0.05,
                CloseRatio = 0.23,
                ExtremumRatio = 1,
                IndexNumber = 51
            };

            var comparedItem = new PriceDto()
            {
                Id = 2,
                PriceDate = new DateTime(2017, 3, 4, 21, 15, 0),
                AssetId = 1,
                TimeframeId = 1,
                DeltaClosePrice = 1.04,
                PriceDirection2D = 0,
                PriceDirection3D = -1,
                PriceGap = 0.07,
                CloseRatio = 0.23,
                ExtremumRatio = 1,
                IndexNumber = 51
            };

            //Act
            comparedItem.CopyProperties(baseItem);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }


        #endregion COPY_PROPERTIES


        #region EQUALS

        [TestMethod]
        public void Equals_ReturnsFalse_IfComparedToObjectOfOtherType()
        {

            //Arrange
            var baseItem = getDefaultPriceDto();
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
            var baseItem = getDefaultPriceDto();
            var comparedItem = getDefaultPriceDto();
            
            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultPriceDto();
            var comparedItem = getDefaultPriceDto();

            //Act
            comparedItem.Id++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfSimulationIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultPriceDto();
            var comparedItem = getDefaultPriceDto();

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
            var baseItem = getDefaultPriceDto();
            var comparedItem = getDefaultPriceDto();

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
            var baseItem = getDefaultPriceDto();
            var comparedItem = getDefaultPriceDto();

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
            var baseItem = getDefaultPriceDto();
            var comparedItem = getDefaultPriceDto();

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
            var baseItem = getDefaultPriceDto();
            var comparedItem = getDefaultPriceDto();

            //Act
            comparedItem.IndexNumber++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfDeltaClosePriceIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultPriceDto();
            var comparedItem = getDefaultPriceDto();

            //Act
            comparedItem.DeltaClosePrice += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfPriceDirection2DIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultPriceDto();
            var comparedItem = getDefaultPriceDto();

            //Act
            comparedItem.PriceDirection2D *= -1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfPriceDirection3DIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultPriceDto();
            var comparedItem = getDefaultPriceDto();

            //Act
            comparedItem.PriceDirection3D *= -1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfPriceGapIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultPriceDto();
            var comparedItem = getDefaultPriceDto();

            //Act
            comparedItem.PriceGap += 0.015;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfCloseRatioIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultPriceDto();
            var comparedItem = getDefaultPriceDto();

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
            var baseItem = getDefaultPriceDto();
            var comparedItem = getDefaultPriceDto();

            //Act
            comparedItem.ExtremumRatio += 0.15;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }     

        #endregion EQUALS


    }

}
