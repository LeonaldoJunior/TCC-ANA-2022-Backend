using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC_Ana.DataModels
{
    public class VolumeCalculation
    {
        [Key]
        public int UserDeviceId { get; set; }
        public int EventId { get; set; }
        public decimal currentVolume { get; set; }
    }

}