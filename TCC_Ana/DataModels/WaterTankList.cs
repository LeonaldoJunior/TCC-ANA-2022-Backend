using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TCC_Ana.DataModels
{
    [Table("WaterTankList")]
    public class WaterTankList
    {
        [Key]
        public int WaterTankId { get; set; }
        public string Brand { get; set; }
        public int TheoVolume { get; set; }
        public decimal BaseRadius { get; set; }
        public decimal TopRadius { get; set; }
        public decimal Height { get; set; }
        public double MaxVolume { get; set; }
    }

}