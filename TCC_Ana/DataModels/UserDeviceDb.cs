using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCC_Ana.DataModels
{
    public class UserDeviceDb
    {
        [Key]
        public int EventId { get; set; }

        public string EndDeviceId { get; set; }

        public string ApplicationId { get; set; }
        public string DevEui { get; set; }
        public string DevAddr { get; set; }

        public string GatewayId { get; set; }
        public string GatewayEui { get; set; }


        public string ReceivedAt { get; set; }
        public int FPort { get; set; }
        public int FCnt { get; set; }
        public string FrmPayload { get; set; }
        public double AnalogIn1 { get; set; }
        public double AnalogIn2 { get; set; }

    }

}