using System;
using System.ComponentModel.DataAnnotations;

namespace Stock.DAL.TransferObjects
{
    public class TrendlineDto
    {
        [Key]
        public int Id { get; set; }
        public int ShareId { get; set; }
        public int BaseStartIndex { get; set; }
        public double BaseLevel { get; set; }
        public int CounterStartIndex { get; set; }
        public double CounterLevel { get; set; }
        public int? StartDateIndex { get; set; }
        public int? EndDateIndex { get; set; }
        public double Value { get; set; }
        public bool ShowOnChart { get; set; }
        public double Slope { get; set; }
    }
}
