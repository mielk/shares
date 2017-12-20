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
    public class AnalysisTimestampDtoUnitTests
    {

        private AnalysisTimestampDto getDefaultAnalysisTimestampDto()
        {
            return new AnalysisTimestampDto()
            {
                Id = 1,
                SimulationId = 1,
                AnalysisTypeId = 1,
                LastAnalysedItem = new DateTime(2017, 4, 20, 19, 45, 0),
                LastAnalysedIndex = 100
            };
        }


        #region COPY_PROPERTIES

        [TestMethod]
        public void CopyProperties_AfterwardAllPropertiesAreEqual()
        {

            //Arrange
            var baseItem = new AnalysisTimestampDto()
            {
                Id = 1,
                SimulationId = 1,
                AnalysisTypeId = 1,
                AssetId = 1,
                TimeframeId = 2,
                LastAnalysedItem = new DateTime(2017, 4, 20, 19, 45, 0)
            };

            var comparedItem = new AnalysisTimestampDto()
            {
                Id = 1,
                SimulationId = 2,
                AnalysisTypeId = 2,
                AssetId = 1,
                TimeframeId = 2,
                LastAnalysedItem = new DateTime(2017, 4, 18, 19, 30, 0)
            };

            //Act
            comparedItem.CopyProperties(baseItem);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);
            Assert.IsFalse(baseItem == comparedItem);

        }


        #endregion COPY_PROPERTIES


        #region EQUALS

        [TestMethod]
        public void Equals_ReturnsFalse_IfComparedToObjectOfOtherType()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestampDto();
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
            var baseItem = getDefaultAnalysisTimestampDto();
            var comparedItem = getDefaultAnalysisTimestampDto();
            
            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfSimulationIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestampDto();
            var comparedItem = getDefaultAnalysisTimestampDto();

            //Act
            comparedItem.SimulationId++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfAssetIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestampDto();
            var comparedItem = getDefaultAnalysisTimestampDto();

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
            var baseItem = getDefaultAnalysisTimestampDto();
            var comparedItem = getDefaultAnalysisTimestampDto();

            //Act
            comparedItem.TimeframeId++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfAnalysisTypeIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestampDto();
            var comparedItem = getDefaultAnalysisTimestampDto();

            //Act
            comparedItem.AnalysisTypeId++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfLastItemDateIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestampDto();
            var comparedItem = getDefaultAnalysisTimestampDto();

            //Act
            comparedItem.LastAnalysedItem = ((DateTime)comparedItem.LastAnalysedItem).AddMinutes(30);
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBaseLastItemDateIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestampDto();
            var comparedItem = getDefaultAnalysisTimestampDto();

            //Act
            baseItem.LastAnalysedItem = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedLastItemDateIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestampDto();
            var comparedItem = getDefaultAnalysisTimestampDto();

            //Act
            comparedItem.LastAnalysedItem = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfBothLastItemDatesAreNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestampDto();
            var comparedItem = getDefaultAnalysisTimestampDto();

            //Act
            baseItem.LastAnalysedItem = null;
            comparedItem.LastAnalysedItem = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }


        [TestMethod]
        public void Equals_ReturnsFalse_IfLastItemIndexIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestampDto();
            var comparedItem = getDefaultAnalysisTimestampDto();

            //Act
            comparedItem.LastAnalysedIndex = comparedItem.LastAnalysedIndex + 5;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyBaseLastItemIndexIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestampDto();
            var comparedItem = getDefaultAnalysisTimestampDto();

            //Act
            baseItem.LastAnalysedIndex = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfOnlyComparedLastItemIndexIsNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestampDto();
            var comparedItem = getDefaultAnalysisTimestampDto();

            //Act
            comparedItem.LastAnalysedIndex = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsTrue_IfBothLastItemIndexesAreNull()
        {

            //Arrange
            var baseItem = getDefaultAnalysisTimestampDto();
            var comparedItem = getDefaultAnalysisTimestampDto();

            //Act
            baseItem.LastAnalysedIndex = null;
            comparedItem.LastAnalysedIndex = null;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }


        #endregion EQUALS


    }

}
