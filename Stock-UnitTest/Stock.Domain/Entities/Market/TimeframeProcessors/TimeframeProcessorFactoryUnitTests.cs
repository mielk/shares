using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities.MarketObjects.TimeframeProcessors;
using Stock.Domain.Entities;
using Stock.Domain.Enums;

namespace Stock_UnitTest.Stock.Domain.Entities.MarketObjects.TimeframeProcessors
{
    [TestClass]
    public class TimeframeProcessorFactoryUnitTests
    {
        [TestMethod]
        public void GetProcessor_ReturnsMinutesProcessor_ForMinutes()
        {

            //Act
            ITimeframeProcessor processor = TimeframeProcessorFactory.GetProcessor(TimeframeUnit.Minutes);

            //Assert
            var result = (processor is MinutesProcessor);
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void GetProcessor_ReturnsHoursProcessor_ForHours()
        {

            //Act
            ITimeframeProcessor processor = TimeframeProcessorFactory.GetProcessor(TimeframeUnit.Hours);

            //Assert
            var result = (processor is HoursProcessor);
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void GetProcessor_ReturnsDaysProcessor_ForDays()
        {

            //Act
            ITimeframeProcessor processor = TimeframeProcessorFactory.GetProcessor(TimeframeUnit.Days);

            //Assert
            var result = (processor is DaysProcessor);
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void GetProcessor_ReturnsWeeksProcessor_ForWeeks()
        {

            //Act
            ITimeframeProcessor processor = TimeframeProcessorFactory.GetProcessor(TimeframeUnit.Weeks);

            //Assert
            var result = (processor is WeeksProcessor);
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void GetProcessor_ReturnsMonthsProcessor_ForMonths()
        {

            //Act
            ITimeframeProcessor processor = TimeframeProcessorFactory.GetProcessor(TimeframeUnit.Months);

            //Assert
            var result = (processor is MonthsProcessor);
            Assert.IsTrue(result);

        }

    }
}
