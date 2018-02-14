using System;
using Stock.DAL.TransferObjects;

namespace Stock.Domain.Entities
{
    public class Trendline
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public int TimeframeId { get; set; }
        public int BaseStartIndex { get; set; }
        public double BaseLevel { get; set; }
        public int CounterStartIndex { get; set; }
        public double CounterLevel { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public double Value { get; set; }
        public bool ShowOnChart { get; set; }
        public double Slope { get; set; }

        public static Trendline FromDto(TrendlineDto dto)
        {
            var trendline = new Trendline
            {
                Id = dto.Id,
                AssetId = dto.AssetId,
                TimeframeId = dto.TimeframeId,
                Slope = dto.Slope,
                ShowOnChart = dto.ShowOnChart,
                Value = dto.Value,
                BaseStartIndex = dto.BaseStartIndex,
                BaseLevel = dto.BaseLevel,
                CounterStartIndex = dto.CounterStartIndex,
                CounterLevel = dto.CounterLevel,
                StartIndex = dto.StartDateIndex ?? -1,
                EndIndex = dto.EndDateIndex ?? -1
            };
            return trendline;
        }

    }

}