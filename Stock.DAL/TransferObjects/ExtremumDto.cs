using System;
using System.ComponentModel.DataAnnotations;

namespace Stock.DAL.TransferObjects
{
    public class ExtremumDto
    {
        [Key]
        public int Id { get; set; }
        public int ShareId { get; set; }
        public int DateIndex { get; set; }
        public int ExtremumType { get; set; }
        public double Value { get; set; }
    }
}