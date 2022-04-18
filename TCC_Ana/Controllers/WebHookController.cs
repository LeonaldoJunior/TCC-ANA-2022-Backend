using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TCC_Ana.DataModels;
using System.Linq;
using Newtonsoft.Json;
using System;

namespace TCC_Ana.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WebHookController : ControllerBase
    {
        readonly IConfiguration _configuration;
        public  WebHookController(IConfiguration iConfig)
        {
            _configuration = iConfig;
        }

        //[HttpGet]
        //public IActionResult Get()
        //{

        //    return Ok(_configuration.GetValue<string>("LeonaldoTest"));
        //    //return Ok(VaultClient.GetSecret("LeonaldoTest").Value.Value);

        //}


        [HttpGet()]
        public IActionResult GetByIdMax(string id)
        {
            using Context myContext = new Context(_configuration);

            //var events = myContext.EventsEndDevices.Where(s => s.EndDeviceId == id).ToList();
            //var events = myContext.EventsEndDevices.Select(s => s.FirstOrDefault(w => w.));

            var maxEvent = myContext.EventsEndDevice.OrderByDescending(p => p.EventId).FirstOrDefault(x =>x.EndDeviceId == id);


            return Ok(maxEvent);
        }

        [HttpGet()]
        public IActionResult GetAllById(string id)
        {
            using Context myContext = new Context(_configuration);

            var allEvents = myContext.EventsEndDevice.Where(s => s.EndDeviceId == id).ToList();
            //var events = myContext.EventsEndDevices.Select(s => s.FirstOrDefault(w => w.));


            return Ok(allEvents);
        }

        // POST: WebHookController/
        [HttpPost]
        public void Post([FromBody] EndDeviceEvent endDeviceEvent)
        //public async Task<IActionResult> Post()
        {
            using Context myContext = new Context(_configuration);


            var newEvent = new EventsEndDevice()
            {
                EndDeviceId = endDeviceEvent.EndDeviceIds.DeviceId,
                ApplicationId = endDeviceEvent.EndDeviceIds.ApplicationIds.ApplicationId,
                DevEui = endDeviceEvent.EndDeviceIds.DevEui,
                DevAddr = endDeviceEvent.EndDeviceIds.DevAddr,
                GatewayId = endDeviceEvent.UplinkMessage.RxMetadata[0].GatewayIds.GatewayId,
                GatewayEui = endDeviceEvent.UplinkMessage.RxMetadata[0].GatewayIds.Eui,
                ReceivedAt = endDeviceEvent.UplinkMessage.ReceivedAt,
                FPort = endDeviceEvent.UplinkMessage.FPort,
                FrmPayload = endDeviceEvent.UplinkMessage.FrmPayload,
                AnalogIn1 = endDeviceEvent.UplinkMessage.DecodedPayload.AnalogIn1,
                AnalogIn2 = endDeviceEvent.UplinkMessage.DecodedPayload.AnalogIn2
            };

            myContext.Add(newEvent);
            myContext.SaveChanges();

            if (isNewDeviceID(endDeviceEvent.EndDeviceIds.DeviceId))
            {
                var newDevice = new EndDeviceList();
                newDevice.EndDeviceId = endDeviceEvent.EndDeviceIds.DeviceId;
                myContext.EndDeviceList.Add(newDevice);
                myContext.SaveChanges();
            }


            var userDeviceAndWaterTank = (from waterTank in myContext.WaterTankList
                                           join userDevice in myContext.UsersAndDevices on waterTank.WaterTankId equals userDevice.WaterTankId
                                           where userDevice.EndDeviceID == newEvent.EndDeviceId
                                          select new {
                                               waterTank,
                                               userDevice
                                           }
                                           ).FirstOrDefault();

            if(userDeviceAndWaterTank != null)
            {
                var currentFluidHeight = fluidHeightCalculation(newEvent.AnalogIn2, userDeviceAndWaterTank.waterTank.Height);
            
                var currentVolume = VolumeCalculation(currentFluidHeight, userDeviceAndWaterTank.waterTank);

                var currentVolumeCalculation = new VolumeCalculation();

                currentVolumeCalculation.CurrentVolume = currentVolume;
                currentVolumeCalculation.EventId = newEvent.EventId;
                currentVolumeCalculation.UsersAndDevicesId = userDeviceAndWaterTank.userDevice.UsersAndDevicesId;
                currentVolumeCalculation.CurrentBatteryLevel = newEvent.AnalogIn1;

                myContext.Add(currentVolumeCalculation);
                myContext.SaveChanges();

            }


        }
        private bool isNewDeviceID(string endDeviceId)
        {
            using Context myContext = new Context(_configuration);
            var endDeviceFound = !myContext.UsersAndDevices.Where(e => e.EndDeviceID == endDeviceId).Any();
            return endDeviceFound;
        }


        private double VolumeCalculation(double fluidHeight, WaterTankList waterTank)
        {
            var baseRadius = Convert.ToDouble(waterTank.BaseRadius);
            var fluidRadius = fluidRadiusCalculation(fluidHeight, waterTank);
            var maxHeight = Convert.ToDouble(waterTank.Height) - 0.06;

            if (fluidHeight > 0.02)
            {
                return (Math.PI * fluidHeight) * (Math.Pow(baseRadius, 2) + (baseRadius * fluidRadius) + Math.Pow(fluidRadius, 2)) / 3;

            }
            else
            {
                if (fluidHeight > maxHeight)
                {
                    return waterTank.TheoVolume;
                }
                else
                {
                    return 0;
                }
            }
        }



        private double fluidHeightCalculation(double sensorMeasure, decimal waterTankHeight)
        {
            return Convert.ToDouble(waterTankHeight) - sensorMeasure;
        }

        private double fluidRadiusCalculation(double fluidHeight, WaterTankList waterTank)
        {
            var baseRadius = Convert.ToDouble(waterTank.BaseRadius);
            var topRadius = Convert.ToDouble(waterTank.TopRadius);
            var waterTankHeight = Convert.ToDouble(waterTank.Height);

            return (baseRadius - ((fluidHeight * (baseRadius - topRadius))) / (waterTankHeight));
    
        }



    }
}
