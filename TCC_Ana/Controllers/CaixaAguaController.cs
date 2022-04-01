using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TCC_Ana.DataModels;
using System.Linq;
using TCC_Ana.Services;

namespace TCC_Ana.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CaixaAguaController : ControllerBase
    {
        readonly IConfiguration _configuration;
        readonly IKeyvaultManagement _keyvaultManagement;
        public CaixaAguaController(IConfiguration config, IKeyvaultManagement keyvaultManagement)
        {
            _configuration = config;
            _keyvaultManagement = keyvaultManagement;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            using Context myContext = new Context(_configuration);

            var caixaAguaList = myContext.WaterTankLists.ToList();
            return Ok(caixaAguaList);

            //return Ok(VaultClient.GetSecret("LeonaldoTest").Value.Value);
        }


        [HttpGet()]
        public IActionResult GetByIdMax(string id)
        {
            using Context myContext = new Context(_configuration);

            //var events = myContext.EventsEndDevices.Where(s => s.EndDeviceId == id).ToList();
            //var events = myContext.EventsEndDevices.Select(s => s.FirstOrDefault(w => w.));

            var maxEvent = myContext.EventsEndDevices.OrderByDescending(p => p.EventId).FirstOrDefault(x =>x.EndDeviceId == id);


            return Ok(maxEvent);
        }

        // POST: WebHookController/
        [HttpPost]
        public IActionResult Post([FromBody] EndDeviceEvent endDeviceEvent)
        //public async Task<IActionResult> Post()
        {
            using Context myContext = new Context(_configuration);


            myContext.Add(
            new EventsEndDevice()
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
