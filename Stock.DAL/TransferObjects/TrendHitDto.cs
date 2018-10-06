using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.DAL.TransferObjects
{
    public class TrendHitDto
    {
        [Key]
        public int TrendHitId { get; set; }
        public int TrendlineId { get; set; }
        public int ExtremumGroupId { get; set; }
        public double? Value { get; set; }
        public double? PointsForDistance { get; set; }
        public double? PointsForValue { get; set; }
        public double? Gap { get; set; }
        public double? RelativeGap { get; set; }
    }
}
