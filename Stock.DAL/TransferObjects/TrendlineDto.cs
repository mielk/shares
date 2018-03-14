using System;
using System.ComponentModel.DataAnnotations;

namespace Stock.DAL.TransferObjects
{
    public class TrendlineDto
    {
        [Key]
        public int TrendlineId { get; set; }
        public int AssetId { get; set; }
        public int TimeframeId { get; set; }
        public int BaseDateIndex { get; set; }
        public double BaseLevel { get; set; }
        public int CounterDateIndex { get; set; }
        public double CounterLevel { get; set; }
        public int? StartDateIndex { get; set; }
        public int? EndDateIndex { get; set; }
        public double Value { get; set; }
        public bool ShowOnChart { get; set; }
        public double Angle { get; set; }
    }
}
