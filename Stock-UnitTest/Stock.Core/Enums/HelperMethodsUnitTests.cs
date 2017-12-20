using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Core;
using Stock.Utils;
using System.Collections;
using System.Collections.Generic;

namespace Stock_UnitTest.Stock.Core.Enums
{
    [TestClass]
    public class HelperMethodsUnitTests
    {

        #region TO_ANALYSIS_TYPE

        [TestMethod]
        public void ToAnalysisType_ReturnsProperSetOfValues_ForLowerCaseNames()
        {

            //Arrange
            var actualAnalysisTypes = new AnalysisType[] { "quotations".ToAnalysisType(), "macd".ToAnalysisType(), "adx".ToAnalysisType(), 
                                                           "candlesticks".ToAnalysisType(), "prices".ToAnalysisType(), "trendlines".ToAnalysisType(),
                                                            "dataset".ToAnalysisType() };

            //Assert
            var expectedAnalysisTypes = new AnalysisType[] { AnalysisType.Quotations, AnalysisType.Macd, AnalysisType.Adx, 
                                                             AnalysisType.Candlesticks, AnalysisType.Prices, AnalysisType.Trendlines, 
                                                             AnalysisType.DataSet };
            bool areEqual = expectedAnalysisTypes.HasEqualItems(actualAnalysisTypes);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void ToAnalysisType_ReturnsProperSetOfValues_ForMixedCaseNames()
        {

            //Arrange
            var actualAnalysisTypes = new AnalysisType[] { "QUOTATIONS".ToAnalysisType(), "Macd".ToAnalysisType(), "ADX".ToAnalysisType(), 
                                                           "Candlesticks".ToAnalysisType(), "PRICES".ToAnalysisType(), "TreNdliNes".ToAnalysisType(),
                                                            "Dataset".ToAnalysisType() };

            //Assert
            var expectedAnalysisTypes = new AnalysisType[] { AnalysisType.Quotations, AnalysisType.Macd, AnalysisType.Adx, 
                                                             AnalysisType.Candlesticks, AnalysisType.Prices, AnalysisType.Trendlines, AnalysisType.DataSet };
            bool areEqual = expectedAnalysisTypes.HasEqualItems(actualAnalysisTypes);
            Assert.IsTrue(areEqual);

        }


        [TestMethod]
        [ExpectedException(typeof(Exception), "Unknown analysis type: abc")]
        public void ToAnalysisType_ThrowsException_ForUnknownString()
        {

            //Arrange
            var actualAnalysisTypes = "abc".ToAnalysisType();

        }

        [TestMethod]
        public void ToAnalysisTypes_ReturnsProperSetOfValues_ForLowerCaseNames(){

            //Arrange
            string[] names = new string[] { "quotations", "macd", "adx", "candlesticks", "prices", "trendlines" };

            //Act
            var actualAnalysisTypes = names.ToAnalysisTypes();

            //Assert
            var expectedAnalysisTypes = new AnalysisType[] { AnalysisType.Quotations, AnalysisType.Macd, AnalysisType.Adx, 
                                                             AnalysisType.Candlesticks, AnalysisType.Prices, AnalysisType.Trendlines };
            bool areEqual = expectedAnalysisTypes.HasEqualItems(actualAnalysisTypes);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void ToAnalysisTypes_ReturnsProperSetOfValues_ForMixedCaseNames()
        {

            //Arrange
            string[] names = new string[] { "QUOTATIONS", "Macd", "Adx", "CANDLESTICKS", "prices", "TReNDLiNES" };

            //Act
            var actualAnalysisTypes = names.ToAnalysisTypes();

            //Assert
            var expectedAnalysisTypes = new AnalysisType[] { AnalysisType.Quotations, AnalysisType.Macd, AnalysisType.Adx, 
                                                             AnalysisType.Candlesticks, AnalysisType.Prices, AnalysisType.Trendlines };
            bool areEqual = expectedAnalysisTypes.HasEqualItems(actualAnalysisTypes);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Unknown analysis type: zzz")]
        public void ToAnalysisTypes_ThrowsException_IfAnyOfStringsIsUnknown()
        {

            //Arrange
            string[] names = new string[] { "prices", "adx", "macd", "zzz", "trendlines" };

            //Acttre
            var actualAnalysisTypes = names.ToAnalysisTypes();


        }

        #endregion TO_ANALYSIS_TYPE

    }
}
