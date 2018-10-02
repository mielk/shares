using System;
using Stock.DAL.TransferObjects;
using System.Collections.Generic;

namespace Stock.Domain.Entities
{
    public class Trendline
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public int TimeframeId { get; set; }
        public int BaseExtremumGroupId { get; set; }
        public ExtremumGroup BaseExtremumGroup { get; set; }
        public int BaseDateIndex { get; set; }
        public double BaseLevel { get; set; }
        public int CounterExtremumGroupId { get; set; }
        public ExtremumGroup CounterExtremumGroup { get; set; }
        public int CounterDateIndex { get; set; }
        public double CounterLevel { get; set; }
        public int? StartIndex { get; set; }
        public int? EndIndex { get; set; }
        public double Value { get; set; }
        public bool IsOpenFromLeft { get; set; }
        public bool IsOpenFromRight { get; set; }
        public bool ShowOnChart { get; set; }
        public double Slope { get; set; }
        public List<TrendRange> TrendRanges { get; set; }
        

        public static Trendline FromDto(TrendlineDto dto)
        {
            var trendline = new Trendline
            {
                Id = dto.TrendlineId,
                AssetId = dto.AssetId,
                TimeframeId = dto.TimeframeId,
                Slope = dto.Angle,
                ShowOnChart = dto.ShowOnChart,
                Value = dto.Value,
                BaseExtremumGroupId = dto.BaseExtremumGroupId,
                BaseDateIndex = dto.BaseDateIndex,
                BaseLevel = dto.BaseLevel,
                CounterExtremumGroupId = dto.CounterExtremumGroupId,
                CounterDateIndex = dto.CounterDateIndex,
                CounterLevel = dto.CounterLevel,
                StartIndex = dto.StartDateIndex,
                EndIndex = dto.EndDateIndex,
                IsOpenFromLeft = dto.IsOpenFromLeft,
                IsOpenFromRight = dto.IsOpenFromRight,
                TrendRanges = new List<TrendRange>()
            };
            return trendline;
        }

        public void AddTrendRange(TrendRange trendRange)
        {
            TrendRanges.Add(trendRange);
        }

    }

}