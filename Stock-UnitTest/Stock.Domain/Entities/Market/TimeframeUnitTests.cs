using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using System.Collections.Generic;
using Stock.DAL.TransferObjects;
using System.Linq;
using Stock.Domain.Services;
using Stock.Utils;
using Stock.Domain.Enums;
using Moq;

namespace Stock_UnitTest.Stock.Domain.Entities
{
    [TestClass]
    public class TimeframeUnitTests
    {

        private const int DEFAULT_ID = 1;
        private const string DEFAULT_NAME = "M5";
        private const TimeframeUnit DEFAULT_UNIT_TYPE = TimeframeUnit.Minutes;
        private const int DEFAULT_UNITS_COUNTER = 5;


        #region INFRASTRUCTURE

        private Timeframe defaultTimeframe()
        {
            return new Timeframe(DEFAULT_ID, DEFAULT_NAME, DEFAULT_UNIT_TYPE, DEFAULT_UNITS_COUNTER);
        }

        private IEnumerable<Timeframe> getTimeframesCollection()
        {
            List<Timeframe> timeframes = new List<Timeframe>();
            timeframes.Add(new Timeframe(1, "M5", TimeframeUnit.Minutes, 5));
            timeframes.Add(new Timeframe(1, "H1", TimeframeUnit.Hours, 1));
            timeframes.Add(new Timeframe(1, "D1", TimeframeUnit.Days, 1));
            return timeframes;
        }

        #endregion INFRASTRUCTURE


        #region CONSTRUCTORS

        [TestMethod]
        public void constructor_newInstanceHasCorrectProperties()
        {

            //Act
            var timeframe = new Timeframe(DEFAULT_ID, DEFAULT_NAME, DEFAULT_UNIT_TYPE, DEFAULT_UNITS_COUNTER);

            //Assert
            Assert.AreEqual(DEFAULT_ID, timeframe.GetId());
            Assert.AreEqual(DEFAULT_NAME, timeframe.GetName());
            Assert.AreEqual(DEFAULT_UNIT_TYPE, timeframe.GetUnitType());
            Assert.AreEqual(DEFAULT_UNITS_COUNTER, timeframe.GetUnitsCounter());
            
        }

        [TestMethod]
        public void timeframeFromDto_hasSamePropertiesAsDto()
        {

            //Act
            TimeframeDto dto = new TimeframeDto { Id = DEFAULT_ID, Symbol = DEFAULT_NAME, PeriodUnit = DEFAULT_UNIT_TYPE.ToString(), PeriodCounter = DEFAULT_UNITS_COUNTER };
            Timeframe timeframe = Timeframe.FromDto(dto);

            //Assert
            Assert.AreEqual(DEFAULT_ID, timeframe.GetId());
            Assert.AreEqual(DEFAULT_NAME, timeframe.GetName());
            Assert.AreEqual(DEFAULT_UNIT_TYPE, timeframe.GetUnitType());
            Assert.AreEqual(DEFAULT_UNITS_COUNTER, timeframe.GetUnitsCounter());

        }

        #endregion CONSTRUCTORS


        #region EQUALS

        [TestMethod]
        public void Equals_returnFalse_forObjectOfOtherType()
        {

            //Act
            var baseObject = new Timeframe(DEFAULT_ID, DEFAULT_NAME, DEFAULT_UNIT_TYPE, DEFAULT_UNITS_COUNTER);
            var comparedObject = new { Id = 1, Value = 2 };

            //Assert
            Assert.IsFalse(baseObject.Equals(comparedObject));

        }

        [TestMethod]
        public void Equals_returnFalse_ifIdDifferent()
        {

            //Act
            var baseObject = new Timeframe(DEFAULT_ID, DEFAULT_NAME, DEFAULT_UNIT_TYPE, DEFAULT_UNITS_COUNTER);
            var comparedObject = new Timeframe(DEFAULT_ID + 1, DEFAULT_NAME, DEFAULT_UNIT_TYPE, DEFAULT_UNITS_COUNTER);

            //Assert
            Assert.IsFalse(baseObject.Equals(comparedObject));

        }

        [TestMethod]
        public void Equals_returnFalse_ifNameDifferent()
        {

            //Act
            var baseObject = new Timeframe(DEFAULT_ID, DEFAULT_NAME, DEFAULT_UNIT_TYPE, DEFAULT_UNITS_COUNTER);
            var comparedObject = new Timeframe(DEFAULT_ID, DEFAULT_NAME + "5", DEFAULT_UNIT_TYPE, DEFAULT_UNITS_COUNTER);

            //Assert
            Assert.IsFalse(baseObject.Equals(comparedObject));

        }

        #endregion EQUALS


        #region ACCESS_METHODS

        [TestMethod]
        public void ById_returnsNull_ifNotExistInRepository()
        {

            //Arrange
            Mock<ITimeframeService> mockService = new Mock<ITimeframeService>();
            Timeframe nullTimeframe = null;
            mockService.Setup(s => s.GetTimeframeById(DEFAULT_ID)).Returns(nullTimeframe);
            Timeframe.injectService(mockService.Object);

            //Act.
            Timeframe timeframe = Timeframe.ById(DEFAULT_ID);

            //Assert.
            Assert.IsNull(timeframe);

        }

        [TestMethod]
        public void ById_returnsExistingInstance()
        {

            //Arrange
            Mock<ITimeframeService> mockService = new Mock<ITimeframeService>();
            Timeframe expectedTimeframe = new Timeframe(DEFAULT_ID, DEFAULT_NAME, DEFAULT_UNIT_TYPE, DEFAULT_UNITS_COUNTER);
            mockService.Setup(s => s.GetTimeframeById(DEFAULT_ID)).Returns(expectedTimeframe);
            Timeframe.injectService(mockService.Object);

            //Act.
            Timeframe timeframe = Timeframe.ById(DEFAULT_ID);

            //Assert.
            Assert.AreSame(expectedTimeframe, timeframe);

        }

        [TestMethod]
        public void BySymbol_returnsExistingInstance()
        {

            //Arrange
            Mock<ITimeframeService> mockService = new Mock<ITimeframeService>();
            Timeframe expectedTimeframe = new Timeframe(DEFAULT_ID, DEFAULT_NAME, DEFAULT_UNIT_TYPE, DEFAULT_UNITS_COUNTER);
            mockService.Setup(s => s.GetTimeframeByName(DEFAULT_NAME)).Returns(expectedTimeframe);
            Timeframe.injectService(mockService.Object);


            //Act.
            Timeframe timeframe = Timeframe.ByName(DEFAULT_NAME);

            //Assert.
            Assert.AreSame(expectedTimeframe, timeframe);

        }

        [TestMethod]
        public void GetAllTimeframes_returnsProperCollection()
        {

            //Arrange
            Mock<ITimeframeService> mockService = new Mock<ITimeframeService>();
            var expectedTimeframes = getTimeframesCollection();
            mockService.Setup(s => s.GetAllTimeframes()).Returns(expectedTimeframes);
            Timeframe.injectService(mockService.Object);

            //Act.
            var timeframes = Timeframe.GetAllTimeframes();

            //Assert.
            bool areEquivalent = timeframes.HasTheSameItems(expectedTimeframes);
            Assert.IsTrue(areEquivalent);

        }

        #endregion ACCESS_METHODS


        #region DATA_PROCESSING

        [TestMethod]
        public void GetDifferenceBetweenDates_returnsValueFromProcessor()
        {

            //Arrange
            Mock<ITimeframeProcessor> mockProcessor = new Mock<ITimeframeProcessor>();
            mockProcessor.Setup(p => p.CountTimeUnits(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(10);
            Timeframe timeframe = defaultTimeframe();
            timeframe.InjectTimeframeProcessor(mockProcessor.Object);

            //Act.
            int difference = timeframe.GetDifferenceBetweenDates(new DateTime(), new DateTime());

            //Assert.
            Assert.AreEqual(10, difference);

        }

        [TestMethod]
        public void AddTimeUnits_returnsValueFromProcessor()
        {

            //Arrange
            Mock<ITimeframeProcessor> mockProcessor = new Mock<ITimeframeProcessor>();
            DateTime expectedDateTime = new DateTime();
            mockProcessor.Setup(p => p.AddTimeUnits(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>())).Returns(expectedDateTime);
            Timeframe timeframe = defaultTimeframe();
            timeframe.InjectTimeframeProcessor(mockProcessor.Object);

            //Act.
            DateTime datetime = timeframe.AddTimeUnits(new DateTime(), 5);

            //Assert.
            Assert.AreEqual(expectedDateTime, datetime);

        }

        [TestMethod]
        public void GetProperDateTime_returnsValueFromProcessor()
        {

            //Arrange
            Mock<ITimeframeProcessor> mockProcessor = new Mock<ITimeframeProcessor>();
            DateTime expectedDateTime = new DateTime();
            mockProcessor.Setup(p => p.GetProperDateTime(It.IsAny<DateTime>(), It.IsAny<int>())).Returns(expectedDateTime);
            Timeframe timeframe = defaultTimeframe();
            timeframe.InjectTimeframeProcessor(mockProcessor.Object);

            //Act.
            DateTime datetime = timeframe.GetProperDateTime(new DateTime());

            //Assert.
            Assert.AreEqual(expectedDateTime, datetime);

        }


        #endregion DATA_PROCESSING


    }

}