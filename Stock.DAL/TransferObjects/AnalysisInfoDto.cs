using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock.DAL.TransferObjects
{
    public class AnalysisInfoDto
    {
        [Key, Column(Order = 0)]
        public int AssetId { get; set; }
        [Key, Column(Order = 1)]
        public int TimeframeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public decimal MinLevel { get; set; }
        public decimal MaxLevel { get; set; }
        public int Counter { get; set; }
    }
}