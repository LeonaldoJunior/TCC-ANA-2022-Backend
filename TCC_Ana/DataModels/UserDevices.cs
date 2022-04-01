using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TCC_Ana.DataModels
{
    [Table("UserDevices")]
    public class UserDevices
    {
        [Key]
        public int UserDeviceID { get; set; }
        public int WaterTankId { get; set; }
        public string EndDeviceID { get; set; }
    }


}