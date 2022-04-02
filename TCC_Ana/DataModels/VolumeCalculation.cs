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
        public int VolumeCalculationId { get; set; }
        public int UsersAndDevicesId { get; set; }
        public int EventId { get; set; }
        public double CurrentVolume { get; set; }
        public double CurrentBatteryLevel { get; set; }
    }

}