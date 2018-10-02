using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.DAL.TransferObjects
{
    public class TrendBreakDto
    {
        [Key]
        public int TrendBreakId { get; set; }
        public int TrendlineId { get; set; }
        public int DateIndex { get; set; }
        public int BreakFromAbove { get; set; }
        public double? Value { get; set; }
    }
}
