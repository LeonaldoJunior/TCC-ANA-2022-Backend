using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TCC_Ana.DataModels;

namespace TCC_Ana.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebHookController : ControllerBase
    {
        readonly IConfiguration _configuration;
        public  WebHookController(IConfiguration iConfig)
        {
            _configuration = iConfig;
        }

        [HttpGet]
        public IActionResult Get()
        {

            return Ok(_configuration.GetValue<string>("LeonaldoTest"));
            //return Ok(VaultClient.GetSecret("LeonaldoTest").Value.Value);

        }

        // POST: WebHookController/
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EndDeviceEvent endDeviceEvent)
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
    }
}
