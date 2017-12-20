using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.DAL.Repositories;
using Moq;
using Stock.Domain.Entities;
using Stock.DAL.TransferObjects;
using Stock.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using Stock.Domain.Enums;

namespace Stock_UnitTest.Stock.Domain.Services.MarketServices
{
    [TestClass]
    public class TimeframeServiceUnitTest
    {

        private const int DEFAULT_ID = 1;
        private const string DEFAULT_NAME = "M5";
        private const int DEFAULT_UNITS_COUNTER = 5;
        private const TimeframeUnit DEFAULT_UNIT_TYPE = TimeframeUnit.Minutes;



        #region INFRASTRUCTURE

        private ITimeframeService testServiceInstance(Mock<ITimeframeRepository> mockedRepository)
        {
            ITimeframeService service = new TimeframeService(mockedRepository.Object);
            return service;
        }

        private TimeframeDto[] getTimeframeDtos()
        {
            List<TimeframeDto> list = new List<TimeframeDto>();
            list.Add(new TimeframeDto { Id = 1, Symbol = "M5", PeriodUnit = "MINUTES", PeriodCounter = 5 });
            list.Add(new TimeframeDto { Id = 2, Symbol = "H1", PeriodUnit = "HOURS", PeriodCounter = 1 });
            list.Add(new TimeframeDto { Id = 3, Symbol = "D1", PeriodUnit = "DAYS", PeriodCounter = 1 });
            return list.ToArray();
        }

        private TimeframeDto defaultTimeframeDto()
        {
            return new TimeframeDto
            {
                Id = DEFAULT_ID,
                Symbol = DEFAULT_NAME,
                PeriodCounter = DEFAULT_UNITS_COUNTER,
                PeriodUnit = DEFAULT_UNIT_TYPE.ToString()
            };
        }

        private Mock<ITimeframeRepository> mockedTimeframeRepositoryForUnitTests()
        {
            Mock<ITimeframeRepository> mockedRepository = new Mock<ITimeframeRepository>();
            TimeframeDto dto = defaultTimeframeDto();
            mockedRepository.Setup(s => s.GetTimeframeById(DEFAULT_ID)).Returns(dto);
            mockedRepository.Setup(s => s.GetTimeframeBySymbol(DEFAULT_NAME)).Returns(dto);
            return mockedRepository;
        }

        #endregion INFRASTRUCTURE



        [TestMethod]
        public void GetTimeframeById_ReturnsTimeframe_IfExists()
        {

            //Arrange
            Mock<ITimeframeRepository> mockedRepository = mockedTimeframeRepositoryForUnitTests();
            ITimeframeService service = testServiceInstance(mockedRepository);

            //Act
            Timeframe expectedTimeframe = new Timeframe(DEFAULT_ID, DEFAULT_NAME, DEFAULT_UNIT_TYPE, DEFAULT_UNITS_COUNTER);
            Timeframe timeframe = service.GetTimeframeById(DEFAULT_ID);

            //Assert
            Assert.AreEqual(expectedTimeframe, timeframe);

        }

        [TestMethod]
        public void GetTimeframeById_ReturnsNull_IfDoesntExist()
        {

            //Arrange
            Mock<ITimeframeRepository> mockedRepository = mockedTimeframeRepositoryForUnitTests();
            TimeframeDto dto = null;
            mockedRepository.Setup(c => c.GetTimeframeById(DEFAULT_ID)).Returns(dto);

            //Act
            ITimeframeService service = testServiceInstance(mockedRepository);
            Timeframe Timeframe = service.GetTimeframeById(DEFAULT_ID);

            //Assert
            Assert.IsNull(Timeframe);

        }

        [TestMethod]
        public void GetTimeframeId_ReturnsAlwaysTheSameInstance()
        {

            //Arrange
            Mock<ITimeframeRepository> mockedRepository = mockedTimeframeRepositoryForUnitTests();

            //Act
            ITimeframeService service = testServiceInstance(mockedRepository);
            Timeframe timeframe1 = service.GetTimeframeById(DEFAULT_ID);
            Timeframe timeframe2 = service.GetTimeframeById(DEFAULT_ID);

            //Assert
            Assert.AreSame(timeframe1, timeframe2);

        }

        [TestMethod]
        public void GetTimeframeByName_ReturnsTimeframe_IfExists()
        {

            //Arrange
            Mock<ITimeframeRepository> mockedRepository = mockedTimeframeRepositoryForUnitTests();
            ITimeframeService service = testServiceInstance(mockedRepository);

            //Act
            Timeframe expectedTimeframe = new Timeframe(DEFAULT_ID, DEFAULT_NAME, DEFAULT_UNIT_TYPE, DEFAULT_UNITS_COUNTER);
            Timeframe Timeframe = service.GetTimeframeByName(DEFAULT_NAME);

            //Assert
            Assert.AreEqual(expectedTimeframe, Timeframe);

        }

        [TestMethod]
        public void GetTimeframeByName_ReturnsNull_IfDoesntExist()
        {

            //Arrange
            Mock<ITimeframeRepository> mockedRepository = new Mock<ITimeframeRepository>();
            TimeframeDto dto = null;
            mockedRepository.Setup(s => s.GetTimeframeBySymbol(DEFAULT_NAME)).Returns(dto);

            //Act
            ITimeframeService service = testServiceInstance(mockedRepository);
            Timeframe Timeframe = service.GetTimeframeByName(DEFAULT_NAME);

            //Assert
            Assert.IsNull(Timeframe);

        }

        [TestMethod]
        public void GetTimeframeByName_ReturnsAlwaysTheSameInstance()
        {

            //Arrange
            Mock<ITimeframeRepository> mockedRepository = mockedTimeframeRepositoryForUnitTests();

            //Act
            ITimeframeService service = testServiceInstance(mockedRepository);
            Timeframe Timeframe1 = service.GetTimeframeByName(DEFAULT_NAME);
            Timeframe Timeframe2 = service.GetTimeframeByName(DEFAULT_NAME);

            //Assert
            Assert.AreSame(Timeframe1, Timeframe2);

        }

        [TestMethod]
        public void GetTimeframe_ReturnsAlwaysTheSameInstance()
        {

            //Arrange
            Mock<ITimeframeRepository> mockedRepository = mockedTimeframeRepositoryForUnitTests();

            //Act
            ITimeframeService service = testServiceInstance(mockedRepository);
            Timeframe timeframe1 = service.GetTimeframeById(DEFAULT_ID);
            Timeframe timeframe2 = service.GetTimeframeByName(DEFAULT_NAME);

            //Assert
            Assert.AreSame(timeframe1, timeframe2);

        }



        [TestMethod]
        public void GetAllTimeframes_ReturnsProperNumberOfItems()
        {
            //Arrange
            Mock<ITimeframeRepository> mockedRepository = new Mock<ITimeframeRepository>();
            TimeframeDto[] dtos = getTimeframeDtos();
            mockedRepository.Setup(r => r.GetAllTimeframes()).Returns(dtos);

            //Act
            ITimeframeService service = testServiceInstance(mockedRepository);
            IEnumerable<Timeframe> Timeframes = service.GetAllTimeframes();

            //Assert
            Assert.AreEqual(dtos.Length, ((List<Timeframe>)Timeframes).Count);

        }

        [TestMethod]
        public void GetAllTimeframes_AlreadyExistingCurrencyInstancesAreUsed()
        {
            //Arrange
            Mock<ITimeframeRepository> mockedRepository = new Mock<ITimeframeRepository>();
            TimeframeDto[] dtos = getTimeframeDtos();
            TimeframeDto dto = dtos[1];
            mockedRepository.Setup(r => r.GetTimeframeById(dto.Id)).Returns(dto);
            mockedRepository.Setup(r => r.GetAllTimeframes()).Returns(dtos);

            //Act
            ITimeframeService service = testServiceInstance(mockedRepository);
            Timeframe timeframe = service.GetTimeframeById(dto.Id);
            IEnumerable<Timeframe> Timeframes = service.GetAllTimeframes();

            //Assert
            Timeframe fromResultCollection = Timeframes.SingleOrDefault(a => a.GetId() == dto.Id);
            Assert.AreSame(fromResultCollection, timeframe);

        }


    }

}