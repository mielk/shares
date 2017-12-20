//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Stock.Domain.Entities;
//using Stock.Domain.Enums;
//using System.Collections.Generic;
//using Stock.Domain.Services;
//using System.Linq;
//using System.Globalization;

//namespace Stock_UnitTest.Stock.Services
//{
//    [TestClass]
//    public class TrendlineAnalyzerUnitTests
//    {

//        private Asset asset = new Asset(9, "AUDJPY");
//        private Timeframe timeframe = Timeframe.GetTimeframe(TimeframeSymbol.D1);
//        private TrendlineAnalyzer analyzer;
//        private DataItem[] items;
//        private DataItem[] extrema;


//        [ClassInitialize]
//        private void PrepareTestEnvironment()
//        {

//            if (analyzer == null)
//            {

//                this.items = UnitTestInitializer.GetData();
//                this.extrema = items.Where(indexNumber => indexNumber.Price.IsExtremum()).ToArray();

//                analyzer = new TrendlineAnalyzer(asset, timeframe);
//                analyzer.Items = this.items;
//                //analyzer.Extrema = this.extrema;
//            }

//        }


//        private void initializeAnalyzer(DataItem initialItem, double initialLevel, DataItem boundItem, double boundLevel)
//        {
//            var difference = boundItem.Index - initialItem.Index;
//            var slope = (boundLevel - initialLevel) / (double)difference;

//            analyzer.Initialize(initialItem, initialLevel, boundItem, boundLevel);

//        }






//        //#region CalculateDistance

//        //[TestMethod]
//        //[TestCategory("CalculateDistance")]
//        //public void CalculateDistance_peaks_returns_1_if_high_at_the_same_level_as_trend()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = extrema[0];
//        //    DataItem bound = extrema[2];
//        //    initializeAnalyzer(item, 106.86, bound, 107.34);


//        //    Assert.AreEqual(1d, analyzer.calculateDistance(item, TrendlineType.Resistance));

//        //}

//        //[TestMethod]
//        //[TestCategory("CalculateDistance")]
//        //public void CalculateDistance_peaks_returns_1_if_higher_from_open_close_at_the_same_level_as_trend()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = extrema[0];
//        //    DataItem bound = extrema[2];
//        //    initializeAnalyzer(item, 106.36, bound, 107.34);


//        //    Assert.AreEqual(1d, analyzer.calculateDistance(item, TrendlineType.Resistance));

//        //}

//        //[TestMethod]
//        //[TestCategory("CalculateDistance")]
//        //public void CalculateDistance_peaks_returns_less_than_1_if_lower_from_open_close_at_the_same_level_as_trend()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = extrema[0];
//        //    DataItem bound = extrema[2];
//        //    initializeAnalyzer(item, 105.49, bound, 107.34);
//        //    var distanceScore = analyzer.calculateDistance(item, TrendlineType.Resistance);

//        //    Assert.IsTrue(distanceScore < 1d);

//        //}

//        //[TestMethod]
//        //[TestCategory("CalculateDistance")]
//        //public void CalculateDistance_peaks_returns_0_if_distance_from_trendline_more_than_limit()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = new DataItem { Index = 3, Quotation = new Quotation { Open = 105.47, High = 105.50, Close = 105.48 } };
//        //    DataItem bound = extrema[2];
//        //    initializeAnalyzer(item, 106.36, bound, 107.34);


//        //    Assert.AreEqual(0d, analyzer.calculateDistance(item, TrendlineType.Resistance));

//        //}

//        //[TestMethod]
//        //[TestCategory("CalculateDistance")]
//        //public void CalculateDistance_peaks_returns_at_least_half_if_candle_shadow_cross_trendline()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = new DataItem { Index = 3, Quotation = new Quotation { Open = 105.47, High = 106.80, Close = 106.05 } };
//        //    DataItem bound = extrema[2];
//        //    initializeAnalyzer(item, 106.36, bound, 107.34);
//        //    var distanceScore = analyzer.calculateDistance(item, TrendlineType.Resistance);

//        //    Assert.IsTrue(distanceScore >= 0.5d);

//        //}

//        //[TestMethod]
//        //[TestCategory("CalculateDistance")]
//        //public void CalculateDistance_troughs_returns_1_if_low_at_the_same_level_as_trend()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = extrema[1];
//        //    DataItem bound = extrema[6];
//        //    initializeAnalyzer(item, 99.35, bound, 95.83);

//        //    Assert.AreEqual(1d, analyzer.calculateDistance(item, TrendlineType.Support));

//        //}

//        //[TestMethod]
//        //[TestCategory("CalculateDistance")]
//        //public void CalculateDistance_troughs_returns_1_if_lower_from_open_close_at_the_same_level_as_trend()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = extrema[1];
//        //    DataItem bound = extrema[6];
//        //    initializeAnalyzer(item, 101.46, bound, 95.83);

//        //    Assert.AreEqual(1d, analyzer.calculateDistance(item, TrendlineType.Support));

//        //}

//        //[TestMethod]
//        //[TestCategory("CalculateDistance")]
//        //public void CalculateDistance_troughs_returns_less_than_1_if_lower_from_open_close_at_the_same_level_as_trend()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = extrema[1];
//        //    DataItem bound = extrema[6];
//        //    initializeAnalyzer(item, 101.52, bound, 95.83);
//        //    var distanceScore = analyzer.calculateDistance(item, TrendlineType.Support);

//        //    Assert.IsTrue(distanceScore < 1d);

//        //}

//        //[TestMethod]
//        //[TestCategory("CalculateDistance")]
//        //public void CalculateDistance_troughs_returns_0_if_distance_from_trendline_more_than_limit()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = new DataItem { Index = 8, Quotation = new Quotation { Open = 106.47, High = 106.50, Close = 106.48, Low = 106.20 } };
//        //    DataItem bound = extrema[6];
//        //    initializeAnalyzer(item, 105.20, bound, 107.34);


//        //    Assert.AreEqual(0d, analyzer.calculateDistance(item, TrendlineType.Support));


//        //}

//        //[TestMethod]
//        //[TestCategory("CalculateDistance")]
//        //public void CalculateDistance_troughs_returns_at_least_half_if_candle_shadow_cross_trendline()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = new DataItem { Index = 8, Quotation = new Quotation { Open = 106.80, High = 107.10, Close = 107.05, Low = 105.90 } };
//        //    DataItem bound = extrema[6];
//        //    initializeAnalyzer(item, 106.36, bound, 107.34);
//        //    var distanceScore = analyzer.calculateDistance(item, TrendlineType.Support);

//        //    Assert.IsTrue(distanceScore >= 0.5d);

//        //}

//        //#endregion



//        //#region CalculateSlopeScore

//        //[TestMethod]
//        //[TestCategory("CalculateSlopeScore")]
//        //public void CalculateSlopeScore_returns_1_for_horizontal_trendline()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = extrema[1];
//        //    DataItem bound = extrema[6];
//        //    initializeAnalyzer(item, 106.36, bound, 106.36);
//        //    var slopeScore = analyzer.calculateSlopeScore(item, bound);

//        //    Assert.IsTrue(slopeScore == 1d);

//        //}


//        //[TestMethod]
//        //[TestCategory("CalculateSlopeScore")]
//        //public void CalculateSlopeScore_Testy_sprawdzajace_te_funkcje_dla_innych_timeframeow()
//        //{
//        //    PrepareTestEnvironment();
//        //    TrendHit[] hits = new TrendHit[2];
//        //    DataItem item = extrema[0];
//        //    DataItem bound = extrema[2];
//        //    initializeAnalyzer(item, 106.86, bound, 107.34);
//        //    hits[0] = new TrendHit { Item = item };
//        //    hits[1] = new TrendHit { Item = bound };

//        //    analyzer.injectHits(hits);

//        //    //analyzer.Slope = 0d;
//        //    //Assert.AreEqual(0.8086d, Math.Round(analyzer.calculateSlopeScore(item, bound), 4));
//        //    Assert.Fail();

//        //}

//        //#endregion



//        #region CalculateTimeframePeriod
//        //[TestMethod]
//        //[TestCategory("CalculateTimeframePeriod")]
//        //public void CalculateTimeframePeriod_returns_1_for_D1_data_item()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = new DataItem{ Date = new DateTime(2010, 1, 1, 0, 0, 0), Index = 0 };
//        //    DataItem subitem = new DataItem { Date = new DateTime(2010, 1, 2, 0, 0, 0), Index = 1 };

//        //    Assert.AreEqual(1d, analyzer.calculateTimeframePeriod(item, subitem));

//        //}


//        //[TestMethod]
//        //[TestCategory("CalculateTimeframePeriod")]
//        //public void CalculateTimeframePeriod_returns_proper_value_for_M5_data_item()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = new DataItem { Date = new DateTime(2010, 1, 1, 0, 0, 0), Index = 0 };
//        //    DataItem subitem = new DataItem { Date = new DateTime(2010, 1, 1, 0, 5, 0), Index = 1 };
//        //    double fiveMinuteEquivalent = 5d / (60d * 24d);

//        //    Assert.AreEqual(fiveMinuteEquivalent, analyzer.calculateTimeframePeriod(item, subitem));

//        //}


//        //[TestMethod]
//        //[TestCategory("CalculateTimeframePeriod")]
//        //public void CalculateTimeframePeriod_returns_proper_value_for_M15_data_item()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = new DataItem { Date = new DateTime(2010, 1, 1, 0, 0, 0), Index = 0 };
//        //    DataItem subitem = new DataItem { Date = new DateTime(2010, 1, 1, 0, 15, 0), Index = 1 };
//        //    double fifteenMinuteEquivalent = 1d / 96d;

//        //    Assert.AreEqual(fifteenMinuteEquivalent, analyzer.calculateTimeframePeriod(item, subitem));

//        //}


//        //[TestMethod]
//        //[TestCategory("CalculateTimeframePeriod")]
//        //public void CalculateTimeframePeriod_returns_proper_value_for_M30_data_item()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = new DataItem { Date = new DateTime(2010, 1, 1, 0, 0, 0), Index = 0 };
//        //    DataItem subitem = new DataItem { Date = new DateTime(2010, 1, 1, 0, 30, 0), Index = 1 };
//        //    double halfHourEquivalent = 1d / 48d;

//        //    Assert.AreEqual(halfHourEquivalent, analyzer.calculateTimeframePeriod(item, subitem));

//        //}


//        //[TestMethod]
//        //[TestCategory("CalculateTimeframePeriod")]
//        //public void CalculateTimeframePeriod_returns_proper_value_for_H1_data_item()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = new DataItem { Date = new DateTime(2010, 1, 1, 0, 0, 0), Index = 0 };
//        //    DataItem subitem = new DataItem { Date = new DateTime(2010, 1, 1, 1, 0, 0), Index = 1 };
//        //    double oneHourEquivalent = 1d / 24d;

//        //    Assert.AreEqual(oneHourEquivalent, analyzer.calculateTimeframePeriod(item, subitem));

//        //}


//        //[TestMethod]
//        //[TestCategory("CalculateTimeframePeriod")]
//        //public void CalculateTimeframePeriod_returns_one_sixth_for_H4_data_item()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = new DataItem { Date = new DateTime(2010, 1, 1, 0, 0, 0), Index = 0 };
//        //    DataItem subitem = new DataItem { Date = new DateTime(2010, 1, 1, 4, 0, 0), Index = 1 };
//        //    double fourHourEquivalent = 1d / 6d;

//        //    Assert.AreEqual(fourHourEquivalent, analyzer.calculateTimeframePeriod(item, subitem));

//        //}


//        //[TestMethod]
//        //[TestCategory("CalculateTimeframePeriod")]
//        //public void CalculateTimeframePeriod_returns_proper_value_for_W1_data_item()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = new DataItem { Date = new DateTime(2010, 1, 1, 0, 0, 0), Index = 0 };
//        //    DataItem subitem = new DataItem { Date = new DateTime(2010, 1, 8, 0, 0, 0), Index = 1 };
//        //    double oneWeekEquivalent = 7d;

//        //    Assert.AreEqual(oneWeekEquivalent, analyzer.calculateTimeframePeriod(item, subitem));

//        //}


//        //[TestMethod]
//        //[TestCategory("CalculateTimeframePeriod")]
//        //public void CalculateTimeframePeriod_returns_proper_value_for_MN1_data_item()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = new DataItem { Date = new DateTime(2010, 1, 1, 0, 0, 0), Index = 0 };
//        //    DataItem subitem = new DataItem { Date = new DateTime(2010, 2, 1, 0, 0, 0), Index = 1 };
//        //    double oneMonthMinEquivalent = 28d;
//        //    double oneMonthMaxEquivalent = 31d;
//        //    double period = analyzer.calculateTimeframePeriod(item, subitem);

//        //    Assert.IsTrue(period >= oneMonthMinEquivalent && period <= oneMonthMaxEquivalent);

//        //}

//        #endregion



//        #region calculateTimeRange

//        [TestMethod]
//        [TestCategory("CalculateTimeRange")]
//        public void CalculateTimeRange_tests()
//        {
//            Assert.Fail();
//        }

//        #endregion



//        #region FindNextExtremum
//        //[TestMethod]
//        //[TestCategory("FindNextExtremum")]
//        //public void FindNextExremum_returns_first_extremum_if_non_item_given()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = extrema[0];
//        //    DataItem subitem = extrema[2];
//        //    initializeAnalyzer(item, item.Quotation.Close, subitem, subitem.Quotation.Close);

//        //    Assert.AreEqual(item, analyzer.FindNextExtremumForProcess());
//        //}


//        //[TestMethod]
//        //[TestCategory("FindNextExtremum")]
//        //public void FindNextExremum_returns_next_extremum_at_the_same_side()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = extrema[0];
//        //    DataItem subitem = extrema[2];
//        //    initializeAnalyzer(item, item.Quotation.Close, subitem, subitem.Quotation.Close);

//        //    Assert.AreEqual(subitem, analyzer.FindNextExtremumForProcess(item));
//        //}


//        //[TestMethod]
//        //[TestCategory("FindNextExtremum")]
//        //public void FindNextExremum_returns_null_for_last_extremum()
//        //{

//        //    PrepareTestEnvironment();
//        //    DataItem item = extrema[596];
//        //    DataItem subitem = extrema[597];
//        //    initializeAnalyzer(item, item.Quotation.Close, subitem, subitem.Quotation.Close);

//        //    Assert.IsNull(analyzer.FindNextExtremumForProcess(item, TrendlineType.Support));
//        //}


//        //[TestMethod]
//        //[TestCategory("FindNextExtremum")]
//        //public void FindNextExtremum_skips_extrema_that_are_too_close()
//        //{

//        //    //given
//        //    PrepareTestEnvironment();
//        //    DataItem item = extrema[6];
//        //    DataItem subitem = extrema[8];
//        //    initializeAnalyzer(item, item.Quotation.Close, subitem, subitem.Quotation.Close);

//        //    //when
//        //    var extremum = analyzer.FindNextExtremumForProcess(item, TrendlineType.Support);

//        //    //then
//        //    Assert.IsNotNull(extremum);
//        //    Assert.AreSame(extrema[8], extremum);

//        //}




//        //[TestMethod]
//        //[TestCategory("FindNextExtremum")]
//        //public void FindNextExtremum_select_earlier_by_close_extremum_for_paired_for_resistance()
//        //{

//        //    //given
//        //    PrepareTestEnvironment();
//        //    DataItem item = items[45];
//        //    DataItem subitem = items[66];
//        //    initializeAnalyzer(item, 100, subitem, 100);

//        //    //when
//        //    var expected = items[55];

//        //    //then
//        //    Assert.AreEqual(expected, analyzer.FindNextExtremumForProcess(item));

//        //}


//        //[TestMethod]
//        //[TestCategory("FindNextExtremum")]
//        //public void FindNextExtremum_select_later_by_close_extremum_for_paired_for_resistance()
//        //{
//        //    Assert.Fail();
//        //}


//        //[TestMethod]
//        //[TestCategory("FindNextExtremum")]
//        //public void FindNextExtremum_select_earlier_by_high_extremum_for_paired_for_resistance()
//        //{

//        //    //given
//        //    PrepareTestEnvironment();
//        //    DataItem item = items[363];
//        //    DataItem subitem = items[391];
//        //    initializeAnalyzer(item, 100, subitem, 100.3);

//        //    //when
//        //    var expected = items[56];

//        //    //then
//        //    Assert.AreEqual(expected, analyzer.FindNextExtremumForProcess(item));

//        //}


//        //[TestMethod]
//        //[TestCategory("FindNextExtremum")]
//        //public void FindNextExtremum_select_later_by_high_extremum_for_paired_for_resistance()
//        //{

//        //    //given
//        //    PrepareTestEnvironment();
//        //    DataItem item = items[45];
//        //    DataItem subitem = items[66];
//        //    initializeAnalyzer(item, 100, subitem, 100.3);

//        //    //when
//        //    var expected = items[56];

//        //    //then
//        //    Assert.AreEqual(expected, analyzer.FindNextExtremumForProcess(item));

//        //}






//        //[TestMethod]
//        //[TestCategory("FindNextExtremum")]
//        //public void FindNextExtremum_select_earlier_by_close_extremum_for_paired_for_support()
//        //{
//        //    Assert.Fail();
//        //}


//        //[TestMethod]
//        //[TestCategory("FindNextExtremum")]
//        //public void FindNextExtremum_select_later_by_close_extremum_for_paired_for_support()
//        //{
//        //    Assert.Fail();
//        //}


//        //[TestMethod]
//        //[TestCategory("FindNextExtremum")]
//        //public void FindNextExtremum_select_earlier_by_low_extremum_for_paired_for_support()
//        //{
//        //    Assert.Fail();
//        //}


//        //[TestMethod]
//        //[TestCategory("FindNextExtremum")]
//        //public void FindNextExtremum_select_later_by_low_extremum_for_paired_for_support()
//        //{
//        //    Assert.Fail();
//        //}

//        #endregion



//        //#region CalculateBreakPoints

//        //[TestMethod]
//        //[TestCategory("CalculateBreakPoints")]
//        //public void CalculateBreaksForSingleCandle_doesnt_change_isBroken_flag_if_close_above_support_line()
//        //{
//        //    PrepareTestEnvironment();
//        //    DataItem item = extrema[8];
//        //    DataItem subitem = extrema[18];
//        //    DataItem checkItem = new DataItem { Index = 20, Quotation = new Quotation { Open = 95, Low = 94, High = 96, Close = 96 } };
//        //    initializeAnalyzer(item, item.Quotation.Close, subitem, subitem.Quotation.Close);

//        //    var lineBroken = false;
//        //    analyzer.calculateSingleCandle(checkItem, out lineBroken);;

//        //    Assert.IsFalse(lineBroken);

//        //}


//        //[TestMethod]
//        //[TestCategory("CalculateBreakPoints")]
//        //public void CalculateBreaksForSingleCandle_doesnt_change_isBroken_flag_if_close_below_resistance_line()
//        //{
//        //    PrepareTestEnvironment();
//        //    DataItem item = extrema[0];
//        //    DataItem subitem = extrema[2];
//        //    DataItem checkItem = new DataItem { Index = 9, Quotation = new Quotation { Open = 106, Low = 101, High = 108, Close = 103 } };
//        //    initializeAnalyzer(item, item.Quotation.Close, subitem, subitem.Quotation.Close);

//        //    var lineBroken = false;
//        //    analyzer.calculateSingleCandle(checkItem, out lineBroken);;

//        //    Assert.IsFalse(lineBroken);
//        //}


//        //[TestMethod]
//        //[TestCategory("CalculateBreakPoints")]
//        //public void CalculateBreaksForSingleCandle_change_isBroken_flag_if_close_above_resistance_line()
//        //{
//        //    PrepareTestEnvironment();
//        //    DataItem item = extrema[0];
//        //    DataItem subitem = extrema[2];
//        //    DataItem checkItem = new DataItem { Index = 9, Quotation = new Quotation { Open = 105, Low = 103, High = 109, Close = 108 } };
//        //    initializeAnalyzer(item, item.Quotation.Close, subitem, subitem.Quotation.Close);

//        //    var lineBroken = false;
//        //    analyzer.calculateSingleCandle(checkItem, out lineBroken);;

//        //    Assert.IsTrue(lineBroken);

//        //}


//        //[TestMethod]
//        //[TestCategory("CalculateBreakPoints")]
//        //public void CalculateBreaksForSingleCandle_change_isBroken_flag_if_close_below_support_line()
//        //{
//        //    PrepareTestEnvironment();
//        //    DataItem item = extrema[8];
//        //    DataItem subitem = extrema[18];
//        //    DataItem checkItem = new DataItem { Index = 20, Quotation = new Quotation { Open = 92.5, Low = 91, High = 93, Close = 92 } };
//        //    initializeAnalyzer(item, item.Quotation.Close, subitem, subitem.Quotation.Close);

//        //    var lineBroken = false;
//        //    analyzer.calculateSingleCandle(checkItem, out lineBroken);;

//        //    Assert.IsTrue(lineBroken);

//        //}



//        //#endregion



//        //#region CheckIfTrendIsExpired

//        //[TestMethod]
//        //[TestCategory("CheckIfTrendIsExpired")]
//        //public void CheckIfTrendIsExpired_tests()
//        //{
//        //    Assert.Fail();
//        //}

//        //#endregion

//    }
//}
