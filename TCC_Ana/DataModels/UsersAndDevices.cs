using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TCC_Ana.DataModels
{
    public class UsersAndDevices
    {
        [Key]
        public int UsersAndDevicesId { get; set; }
        public string UserId { get; set; }
        public string EndDeviceID { get; set; }
        public int WaterTankId { get; set; }
        public string WaterTankName{ get; set; }
        public bool isSelected { get; set; }
    }


}