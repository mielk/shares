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
    public class ExtremumDtoUnitTests
    {

        private ExtremumDto getDefaultExtremumDto()
        {
            return new ExtremumDto()
            {
                Id = 1,
                Date = new DateTime(2017, 3, 4, 21, 10, 0),
                AssetId = 1,
                TimeframeId = 1,
                SimulationId = 1,
                IndexNumber = 51,
                LastCheckedDateTime  = new DateTime(2017, 3, 5, 12, 0, 0),
                ExtremumType = 1,
                Volatility = 1.23,
                EarlierCounter = 15,
                EarlierAmplitude  = 7.45,
                EarlierChange1 = 1.12,
                EarlierChange2 = 2.21,
                EarlierChange3 = 3.12,
                EarlierChange5 = 4.56,
                EarlierChange10  = 5.28,
                LaterCounter  = 16,
                LaterAmplitude  = 1.23,
                LaterChange1  = 0.72,
                LaterChange2  = 0.54,
                LaterChange3  = 1.57,
                LaterChange5  = 2.41,
                LaterChange10  = 3.15,
                IsOpen = true,
                Timestamp = DateTime.Now,
                Value = 123.42
            };
        }



        #region COPY_PROPERTIES

        [TestMethod]
        public void CopyProperties_AfterwardAllPropertiesAreEqual()
        {

            //Arrange
            var baseItem = new ExtremumDto()
            {
                Id = 1,
                Date = new DateTime(2017, 3, 4, 21, 10, 0),
                AssetId = 1,
                TimeframeId = 1,
                SimulationId = 1,
                IndexNumber = 51,
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
                Value = 11.34
            };

            var comparedItem = new ExtremumDto()
            {
                Id = 2,
                Date = new DateTime(2017, 3, 4, 22, 10, 0),
                AssetId = 1,
                TimeframeId = 1,
                SimulationId = 1,
                IndexNumber = 55,
                LastCheckedDateTime = new DateTime(2017, 3, 5, 12, 0, 0),
                ExtremumType = 1,
                Volatility = 1.33,
                EarlierCounter = 16,
                EarlierAmplitude = 7.45,
                EarlierChange1 = 1.12,
                EarlierChange2 = 2.22,
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
                Value = 0
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
            var baseItem = getDefaultExtremumDto();
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
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();
            
            //Act
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfSimulationIdIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

            //Act
            comparedItem.SimulationId++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfDateIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

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
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

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
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

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
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

            //Act
            comparedItem.IndexNumber++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }
        
        [TestMethod]
        public void Equals_ReturnsFalse_IfExtremumTypeIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

            //Act
            comparedItem.ExtremumType++;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }
        
        [TestMethod]
        public void Equals_ReturnsFalse_IfVolatilityIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

            //Act
            comparedItem.Volatility += 0.15;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfEarlierCounterIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

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
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

            //Act
            comparedItem.EarlierAmplitude += 0.12;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }
        
        [TestMethod]
        public void Equals_ReturnsFalse_IfEarlierChange1IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

            //Act
            comparedItem.EarlierChange1 += 0.12;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfEarlierChange2IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

            //Act
            comparedItem.EarlierChange2 += 0.12;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }
        
        [TestMethod]
        public void Equals_ReturnsFalse_IfEarlierChange3IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

            //Act
            comparedItem.EarlierChange3 += 0.12;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }
        
        [TestMethod]
        public void Equals_ReturnsFalse_IfEarlierChange5IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

            //Act
            comparedItem.EarlierChange5 += 0.12;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }
        
        [TestMethod]
        public void Equals_ReturnsFalse_IfEarlierChange10IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

            //Act
            comparedItem.EarlierChange10 += 0.12;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfLaterCounterIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

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
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

            //Act
            comparedItem.LaterAmplitude += 0.12;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }
        
        [TestMethod]
        public void Equals_ReturnsFalse_IfLaterChange1IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

            //Act
            comparedItem.LaterChange1 += 0.12;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfLaterChange2IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

            //Act
            comparedItem.LaterChange2 += 0.12;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }
        
        [TestMethod]
        public void Equals_ReturnsFalse_IfLaterChange3IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

            //Act
            comparedItem.LaterChange3 += 0.12;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }
        
        [TestMethod]
        public void Equals_ReturnsFalse_IfLaterChange5IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

            //Act
            comparedItem.LaterChange5 += 0.12;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }
        
        [TestMethod]
        public void Equals_ReturnsFalse_IfLaterChange10IsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

            //Act
            comparedItem.LaterChange10 += 0.12;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }
        
        [TestMethod]
        public void Equals_ReturnsFalse_IfIsOpenIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

            //Act
            comparedItem.IsOpen = !comparedItem.IsOpen;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }

        [TestMethod]
        public void Equals_ReturnsFalse_IfValueIsDifferent()
        {

            //Arrange
            var baseItem = getDefaultExtremumDto();
            var comparedItem = getDefaultExtremumDto();

            //Act
            comparedItem.Value += 1.25;
            var areEqual = baseItem.Equals(comparedItem);

            //Assert
            Assert.IsFalse(areEqual);

        }


        #endregion EQUALS


    }

}
