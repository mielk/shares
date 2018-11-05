using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.DAL.TransferObjects
{
    public class MacdDto
    {
        [Key, Column(Order = 0)]
        public int AssetId { get; set; }
        [Key, Column(Order = 1)]
        public int TimeframeId { get; set; }
        [Key, Column(Order = 2)]
        public int DateIndex { get; set; }
    }
}
