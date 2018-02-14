using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock.DAL.TransferObjects
{
    public class QuotationDto
    {
        [Key, Column(Order = 0)]
        public int AssetId { get; set; }
        [Key, Column(Order = 1)]
        public int TimeframeId { get; set; }
        [Key, Column(Order = 2)]
        public int DateIndex { get; set; }
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
    }
}