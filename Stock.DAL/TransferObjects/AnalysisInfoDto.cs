using System;
using System.ComponentModel.DataAnnotations;

namespace Stock.DAL.TransferObjects
{
    public class AnalysisInfoDto
    {
        [Key]
        public int ShareId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public decimal MinLevel { get; set; }
        public decimal MaxLevel { get; set; }
        public int Counter { get; set; }
    }
}