using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using Stock.Domain.Enums;
using Stock.DAL.TransferObjects;
using Stock.Core;
using Stock.Utils;

namespace Stock_UnitTest.Helpers
{

    public class UTFactory
    {

        private DateTime baseDateTime = new DateTime(2017, 5, 1, 12, 0, 0);

        public DataSet getDataSetWithQuotation(int indexNumber, double open, double high, double low, double close, double volume)
        {
            return getDataSetWithQuotation(UTDefaulter.DEFAULT_ASSET_ID, UTDefaulter.DEFAULT_TIMEFRAME_ID, indexNumber, open, high, low, close, volume);
        }

        public DataSet getDataSetWithQuotation(int assetId, int timeframeId, int indexNumber, double open, double high, double low, double close, double volume)
        {
            var timeframe = getTimeframe(timeframeId);
            DateTime date = timeframe.AddTimeUnits(baseDateTime, indexNumber - 1);
            DataSet ds = new DataSet(assetId, timeframeId, date, indexNumber);
            Quotation q = new Quotation(ds) { Open = open, High = high, Low = low, Close = close, Volume = volume };
            return ds;

        }

        public DataSet getDataSetWithQuotationAndPrice(int indexNumber, double open, double high, double low, double close, double volume)
        {
            return getDataSetWithQuotationAndPrice(UTDefaulter.DEFAULT_ASSET_ID, UTDefaulter.DEFAULT_TIMEFRAME_ID, indexNumber, open, high, low, close, volume);
        }

        public DataSet getDataSetWithQuotationAndPrice(int assetId, int timeframeId, int indexNumber, double open, double high, double low, double close, double volume)
        {
            var timeframe = getTimeframe(timeframeId);
            DateTime date = timeframe.AddTimeUnits(baseDateTime, indexNumber - 1);
            DataSet ds = new DataSet(assetId, timeframeId, date, indexNumber);
            Quotation q = new Quotation(ds) { Open = open, High = high, Low = low, Close = close, Volume = volume };
            Price p = new Price(ds);
            return ds;
        }


        public DataSet getDataSet(int indexNumber)
        {
            return getDataSet(UTDefaulter.DEFAULT_ASSET_ID, UTDefaulter.DEFAULT_TIMEFRAME_ID, indexNumber);
        }

        public DataSet getDataSet(int assetId, int timeframeId, int indexNumber)
        {
            var timeframe = getTimeframe(timeframeId);
            DateTime date = timeframe.AddTimeUnits(baseDateTime, indexNumber - 1);
            DataSet ds = new DataSet(assetId, timeframeId, date, indexNumber);
            return ds;
        }


        public Quotation getQuotation(DataSet ds)
        {
            return new Quotation(ds)
            {
                Id = ds.IndexNumber,
                Open = 1.09191,
                High = 1.09218,
                Low = 1.09186,
                Close = 1.09194,
                Volume = 1411
            };
        }

        public Quotation getQuotation(int indexNumber)
        {
            return getQuotation(UTDefaulter.DEFAULT_ASSET_ID, UTDefaulter.DEFAULT_TIMEFRAME_ID, indexNumber);
        }

        public Quotation getQuotation(int assetId, int timeframeId, int indexNumber)
        {
            DataSet ds = getDataSet(assetId, timeframeId, indexNumber);
            return new Quotation(ds)
            {
                Id = indexNumber,
                Open = 1.09191,
                High = 1.09218,
                Low = 1.09186,
                Close = 1.09194,
                Volume = 1411
            };
        }

        public QuotationDto getQuotationDto(int indexNumber)
        {
            return getQuotationDto(UTDefaulter.DEFAULT_ASSET_ID, UTDefaulter.DEFAULT_TIMEFRAME_ID, indexNumber);
        }

        public QuotationDto getQuotationDto(int assetId, int timeframeId, int indexNumber)
        {
            var timeframe = getTimeframe(timeframeId);
            DateTime date = timeframe.AddTimeUnits(baseDateTime, indexNumber - 1);
            return new QuotationDto()
            {
                QuotationId = indexNumber,
                PriceDate = date,
                AssetId = assetId,
                TimeframeId = timeframeId,
                IndexNumber = indexNumber,
                OpenPrice = 1.09191,
                HighPrice = 1.09218,
                LowPrice = 1.09186,
                ClosePrice = 1.09194,
                Volume = 1411
            };
        }

        public IEnumerable<Quotation> getDefaultQuotationsCollection()
        {
            return new Quotation[] { getQuotation(1), getQuotation(2), getQuotation(3), getQuotation(4) };
        }

        public IEnumerable<QuotationDto> getDefaultQuotationDtosCollection()
        {
            return new QuotationDto[] { getQuotationDto(1), getQuotationDto(2), getQuotationDto(3), getQuotationDto(4) };
        }



        public Price getPrice(DataSet ds)
        {
            return new Price(ds)
            {
                Id = ds.IndexNumber,
                CloseDelta = 1.05,
                Direction2D = 1,
                Direction3D = 0,
                PriceGap = 1.23,
                CloseRatio = 1.23,
                ExtremumRatio = 2.34
            };
        }

        public Price getPrice(int indexNumber)
        {
            return getPrice(UTDefaulter.DEFAULT_ASSET_ID, UTDefaulter.DEFAULT_TIMEFRAME_ID, indexNumber);
        }

        public Price getPrice(int assetId, int timeframeId, int indexNumber)
        {
            DataSet ds = getDataSet(assetId, timeframeId, indexNumber);
            return new Price(ds)
            {
                Id = indexNumber,
                CloseDelta = 1.05,
                Direction2D = 1,
                Direction3D = 0,
                PriceGap = 1.23,
                CloseRatio = 1.23,
                ExtremumRatio = 2.34
            };
        }

        public PriceDto getPriceDto(int indexNumber)
        {
            return getPriceDto(UTDefaulter.DEFAULT_ASSET_ID, UTDefaulter.DEFAULT_TIMEFRAME_ID, indexNumber);
        }

        public PriceDto getPriceDto(int assetId, int timeframeId, int indexNumber)
        {
            var timeframe = getTimeframe(timeframeId);
            DateTime date = timeframe.AddTimeUnits(baseDateTime, indexNumber - 1);
            return new PriceDto()
            {
                Id = indexNumber,
                PriceDate = date,
                AssetId = assetId,
                TimeframeId = timeframeId,
                IndexNumber = indexNumber,
                DeltaClosePrice = 1.05,
                PriceDirection2D = 1,
                PriceDirection3D = 0,
                PriceGap = 1.23,
                CloseRatio = 1.23,
                ExtremumRatio = 2.34
            };
        }

        public IEnumerable<Price> getDefaultPricesCollection()
        {
            return new Price[] { getPrice(1), getPrice(2), getPrice(3), getPrice(4) };
        }

        public IEnumerable<PriceDto> getDefaultPriceDtosCollection()
        {
            return new PriceDto[] { getPriceDto(1), getPriceDto(2), getPriceDto(3), getPriceDto(4) };
        }



        public Timeframe getTimeframe(int timeframeId)
        {
            return new Timeframe(timeframeId, "5M", TimeframeUnit.Minutes, 5);
        }

        public Trendline getDefaultTrendline()
        {
            AtsSettings settings = new AtsSettings(UTDefaulter.DEFAULT_ASSET_ID, UTDefaulter.DEFAULT_TIMEFRAME_ID, UTDefaulter.DEFAULT_SIMULATION_ID);

            Price basePrice = getPrice(UTDefaulter.DEFAULT_START_INDEX);
            Extremum baseMaster = new Extremum(basePrice, ExtremumType.PeakByClose);
            ExtremumGroup baseGroup = new ExtremumGroup(baseMaster, null);
            TrendlinePoint basePoint = new TrendlinePoint(baseGroup, UTDefaulter.DEFAULT_START_LEVEL);

            Price secondPrice = getPrice(UTDefaulter.DEFAULT_FOOTHOLD_INDEX);
            Extremum secondMaster = new Extremum(secondPrice, ExtremumType.PeakByClose);
            ExtremumGroup secondGroup = new ExtremumGroup(secondMaster, null);
            TrendlinePoint footholdPoint = new TrendlinePoint(secondGroup, UTDefaulter.DEFAULT_FOOTHOLD_LEVEL);

            Trendline trendline = new Trendline(settings, basePoint, footholdPoint);
            trendline.Id = UTDefaulter.DEFAULT_ID;
            trendline.Value = UTDefaulter.DEFAULT_VALUE;
            trendline.LastUpdateIndex = UTDefaulter.DEFAULT_LAST_UPDATE_INDEX;
            trendline.FootholdSlaveIndex = UTDefaulter.DEFAULT_FOOTHOLD_SLAVE_INDEX;
            trendline.CurrentIsPeak = UTDefaulter.DEFAULT_CURRENT_IS_PEAK;

            return trendline;

        }

        public TrendlineDto getDefaultTrendlineDto()
        {
            return new TrendlineDto()
            {
                Id = 1,
                AssetId = UTDefaulter.DEFAULT_ASSET_ID,
                TimeframeId = UTDefaulter.DEFAULT_TIMEFRAME_ID,
                SimulationId = UTDefaulter.DEFAULT_SIMULATION_ID,
                StartIndex = UTDefaulter.DEFAULT_START_INDEX,
                StartLevel = UTDefaulter.DEFAULT_START_LEVEL,
                FootholdIndex = UTDefaulter.DEFAULT_FOOTHOLD_INDEX,
                FootholdLevel = UTDefaulter.DEFAULT_FOOTHOLD_LEVEL,
                FootholdSlaveIndex = UTDefaulter.DEFAULT_FOOTHOLD_SLAVE_INDEX,
                FootholdIsPeak = UTDefaulter.DEFAULT_FOOTHOLD_TYPE,
                Value = 1.2345,
                LastUpdateIndex = 51
            };
        }

    }

}
