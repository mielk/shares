using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using System.Collections.Generic;
using Stock.DAL.TransferObjects;
using Stock.Domain.Services;
using Stock.Utils;

namespace Stock_UnitTest.Stock.Domain.Entities
{
    [TestClass]
    public class TrendlineDtoUnitTests
    {

        private TrendlineDto getDefaultTrendlineDto()
        {
            return new TrendlineDto()
            {
                Id = 1,
                AssetId = 1,
                TimeframeId = 1,
                SimulationId = 1,
                StartIndex = 3,
                StartLevel = 1.2345,
                EndIndex = null,
                FootholdIndex = 15,
                FootholdLevel = 1.2532,
                Value = 1.234,
                LastUpdateIndex = 20
            };
        }


        #region COPY_PROPERTIES

        [TestMethod]
        public void CopyProperties_AfterwardAllPropertiesAreEqual()
        {

            //Arrange
            var baseItem = getDefaultTrendlineDto();
            var comparedItem = new TrendlineDto()
            {
                Id = 1,
                AssetId = 1,
                TimeframeId = 1,
                SimulationId = 1,
                StartIndex = 5,
                StartLevel = 2.2345,
                EndIndex = 20,
                FootholdLevel = 2.2532,
                Value = 2.234,
                LastUpdateIndex = 25
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
            var baseItem = getDefaultTrendlineDto();
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
            var baseItem = getDefaultTrendlineDto();
            var comparedItem = getDefaultTrendlineDto();
            
            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }
        
        [TestMethod]
        public void Equals_ReturnsFalse_IfAssetIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendlineDto();
            var comparedItem = getDefaultTrendlineDto();

            //Act
            comparedItem.AssetId += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfTimeframeIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendlineDto();
            var comparedItem = getDefaultTrendlineDto();

            //Act
            comparedItem.TimeframeId += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfSimulationIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendlineDto();
            var comparedItem = getDefaultTrendlineDto();

            //Act
            comparedItem.SimulationId += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfStartIndexIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendlineDto();
            var comparedItem = getDefaultTrendlineDto();

            //Act
            comparedItem.StartIndex = comparedItem.StartIndex + 2;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfStartLevelIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendlineDto();
            var comparedItem = getDefaultTrendlineDto();

            //Act
            comparedItem.StartLevel += 0.1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfFootholdIndexIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendlineDto();
            var comparedItem = getDefaultTrendlineDto();

            //Act
            comparedItem.FootholdIndex += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfFootholdLevelIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendlineDto();
            var comparedItem = getDefaultTrendlineDto();

            //Act
            comparedItem.FootholdLevel += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfValueIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendlineDto();
            var comparedItem = getDefaultTrendlineDto();

            //Act
            comparedItem.Value += 1;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfLastUpdateIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendlineDto();
            var comparedItem = getDefaultTrendlineDto();

            //Act
            comparedItem.LastUpdateIndex = comparedItem.LastUpdateIndex + 5;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfEndIndexIsEqual()
        {

            //Arrange
            var baseItem = getDefaultTrendlineDto();
            var comparedItem = getDefaultTrendlineDto();

            //Act
            baseItem.EndIndex = 100;
            comparedItem.EndIndex = 100;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfEndIndexIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultTrendlineDto();
            var comparedItem = getDefaultTrendlineDto();

            //Act
            baseItem.EndIndex = 50;
            comparedItem.EndIndex = 100;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBaseItemHasEndIndexNull()
        {

            //Arrange
            var baseItem = getDefaultTrendlineDto();
            var comparedItem = getDefaultTrendlineDto();

            //Act
            baseItem.EndIndex = null;
            comparedItem.EndIndex = 100;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedItemHasEndIndexNull()
        {

            //Arrange
            var baseItem = getDefaultTrendlineDto();
            var comparedItem = getDefaultTrendlineDto();

            //Act
            baseItem.EndIndex = 100;
            comparedItem.EndIndex = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfBothItemHasEndIndexNull()
        {

            //Arrange
            var baseItem = getDefaultTrendlineDto();
            var comparedItem = getDefaultTrendlineDto();

            //Act
            comparedItem.EndIndex = null;
            baseItem.EndIndex = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        #endregion EQUALS


    }

}
