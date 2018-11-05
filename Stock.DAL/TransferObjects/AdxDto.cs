using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock.DAL.TransferObjects
{
    public class AdxDto
    {
        [Key, Column(Order = 0)]
        public int AssetId { get; set; }
        [Key, Column(Order = 1)]
        public int TimeframeId { get; set; }
        [Key, Column(Order = 2)]
        public int DateIndex { get; set; }
        public float DiPositive { get; set; }
        public float DiNegative { get; set; }
        public float Dx { get; set; }
        public float Adx { get; set; }
        //Indicators
        //public int DaysUnder20 { get; set; }
        //public int DaysUnder15 { get; set; }
        //public int Cross20 { get; set; }
        //public float DeltaDiPositive { get; set; }
        //public float DeltaDiNegative { get; set; }
        //public float DeltaAdx { get; set; }
        //public float DiPosDirection3D { get; set; }
        //public float DiPosDirection2D { get; set; }
        //public float DiNegDirection3D { get; set; }
        //public float DiNegDirection2D { get; set; }
    }
}
