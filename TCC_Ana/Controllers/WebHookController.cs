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

            var maxEvent = myContext.EventsEndDevices.OrderByDescending(p => p.EventId).FirstOrDefault(x =>x.EndDeviceId == id);


            return Ok(maxEvent);
        }

        [HttpGet()]
        public IActionResult GetAllById(string id)
        {
            using Context myContext = new Context(_configuration);

            var allEvents = myContext.EventsEndDevices.Where(s => s.EndDeviceId == id).ToList();
            //var events = myContext.EventsEndDevices.Select(s => s.FirstOrDefault(w => w.));


            return Ok(allEvents);
        }

        // POST: WebHookController/
        [HttpPost]
        public IActionResult Post([FromBody] EndDeviceEvent endDeviceEvent)
        //public async Task<IActionResult> Post()
        {
            using Context myContext = new Context(_configuration);

            if (!isNewDeviceID(endDeviceEvent.EndDeviceIds.DeviceId)) {
                var newDevice = new EndDeviceList();
                newDevice.EndDeviceId = endDeviceEvent.EndDeviceIds.DeviceId;
                myContext.EndDeviceLists.Add(newDevice);
            }
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

            var userDeviceWaterTank = (from waterTank in myContext.WaterTankLists
                                       join userDevice in myContext.UsersDevices on waterTank.WaterTankId equals userDevice.WaterTankId
                                       select new {
                                           waterTank,
                                           userDevice
                                       }).FirstOrDefault();

            var currentFluidHeight = fluidHeightCalculation(newEvent.AnalogIn2, userDeviceWaterTank.waterTank.Height);
            
            var currentVolume = VolumeCalculation(currentFluidHeight, userDeviceWaterTank.waterTank);

            var currentVolumeCalculation = new VolumeCalculation();

            currentVolumeCalculation.currentVolume = Convert.ToDecimal(currentVolume);
            currentVolumeCalculation.EventId = newEvent.EventId;




            myContext.SaveChanges();

            return Ok();
        }

        private bool isNewDeviceID(string endDeviceId)
        {
            using Context myContext = new Context(_configuration);
            return myContext.EventsEndDevices.Where(e => e.EndDeviceId == endDeviceId).Any();
        }

        private double VolumeCalculation(double fluidHeight, WaterTankList waterTank)
        {
            var baseRadius = Convert.ToDouble(waterTank.BaseRadius);
            var fluidRadius = fluidRadiusCalculation(fluidHeight, waterTank);
                 
            return (Math.PI * fluidHeight) * (Math.Pow(baseRadius, 2) + (baseRadius * fluidRadius) + Math.Pow(fluidRadius, 2)) / 3;
        }

        private double MaxVolumeCalculation(double fluidHeigh, WaterTankList waterTank)
        {
            var height = Convert.ToDouble(waterTank.Height);
            var baseRadius = Convert.ToDouble(waterTank.BaseRadius);
            var topRadius = Convert.ToDouble(waterTank.TopRadius);

            return ((Math.PI * height) * (Math.Pow(baseRadius, 2) + (baseRadius * topRadius) + Math.Pow(topRadius, 2)) / 3);
        }

        private double fluidHeightCalculation(double sensorMeasure, decimal waterTankHeight)
        {
            return Convert.ToDouble(waterTankHeight) - sensorMeasure;
        }

        private double fluidRadiusCalculation(double fluidHeight, WaterTankList waterTank)
        {
            var height = Convert.ToDouble(waterTank.Height);
            var baseRadius = Convert.ToDouble(waterTank.BaseRadius);
            var topRadius = Convert.ToDouble(waterTank.TopRadius);
            var waterTankHeight = Convert.ToDouble(waterTank.Height);

            return (baseRadius - ((fluidHeight * (baseRadius - topRadius))) / (waterTankHeight));
    
        }


        // POST: WebHookController/
        [HttpPost]
        public IActionResult NewUserDevice([FromForm] string deviceName, [FromForm] string deviceId, [FromForm] string selectedWaterTank)
        //public async Task<IActionResult> Post()
        {
            var selectedWaterTankObj = JsonConvert.DeserializeObject<CaixaAguaModel>(selectedWaterTank);


            //using Context myContext = new Context(_configuration);


            //myContext.Add(
            //new EndDeviceDb()
            //{
            //    EndDeviceId = endDeviceEvent.EndDeviceIds.DeviceId,
            //    ApplicationId = endDeviceEvent.EndDeviceIds.ApplicationIds.ApplicationId,
            //    DevEui = endDeviceEvent.EndDeviceIds.DevEui,
            //    DevAddr = endDeviceEvent.EndDeviceIds.DevAddr,
            //    GatewayId = endDeviceEvent.UplinkMessage.RxMetadata[0].GatewayIds.GatewayId,
            //    GatewayEui = endDeviceEvent.UplinkMessage.RxMetadata[0].GatewayIds.Eui,
            //    ReceivedAt = endDeviceEvent.UplinkMessage.ReceivedAt,
            //    FPort = endDeviceEvent.UplinkMessage.FPort,
            //    FrmPayload = endDeviceEvent.UplinkMessage.FrmPayload,
            //    AnalogIn1 = endDeviceEvent.UplinkMessage.DecodedPayload.AnalogIn1,
            //    AnalogIn2 = endDeviceEvent.UplinkMessage.DecodedPayload.AnalogIn2
            //});

            //myContext.SaveChanges();

            return Ok();
        }
    }
}
