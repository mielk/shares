using System;
using System.ComponentModel.DataAnnotations;

namespace Stock.DAL.TransferObjects
{
    public class QuotationDto
    {
        [Key]
        public int Id { get; set; }
        public int ShareId { get; set; }
        public DateTime Date { get; set; }
        public int DateIndex { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
    }
}