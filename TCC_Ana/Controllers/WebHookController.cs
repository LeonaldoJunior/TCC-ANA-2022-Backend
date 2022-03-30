using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TCC_Ana.DataModels;
using System.Linq;

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

            //var events = myContext.EndDevices.Where(s => s.EndDeviceId == id).ToList();
            //var events = myContext.EndDevices.Select(s => s.FirstOrDefault(w => w.));

            var maxEvent = myContext.EndDevices.OrderByDescending(p => p.EventId).FirstOrDefault(x =>x.EndDeviceId == id);


            return Ok(maxEvent);
        }

        [HttpGet()]
        public IActionResult GetAllById(string id)
        {
            using Context myContext = new Context(_configuration);

            var allEvents = myContext.EndDevices.Where(s => s.EndDeviceId == id).ToList();
            //var events = myContext.EndDevices.Select(s => s.FirstOrDefault(w => w.));


            return Ok(allEvents);
        }

        // POST: WebHookController/
        [HttpPost]
        public IActionResult Post([FromBody] EndDeviceEvent endDeviceEvent)
        //public async Task<IActionResult> Post()
        {
            using Context myContext = new Context(_configuration);


            myContext.Add(
            new EndDeviceDb()
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
            });

            myContext.SaveChanges();

            return Ok();
        }

        // POST: WebHookController/
        [HttpPost]
        public IActionResult NewUserDevice([FromForm] string deviceName, [FromForm] string deviceId, [FromForm]  string selectedWaterTank)
        //public async Task<IActionResult> Post()
        {
     
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
