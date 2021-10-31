using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;



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
            var Options = new SecretClientOptions()
            {
                Retry = {
                    Delay = TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                }
            };

            var VaultClient = new SecretClient(new Uri(_configuration.GetValue<string>("VaultUri")), new DefaultAzureCredential(), Options);

            //return Ok(_configuration.GetValue<string>("LeonaldoTest"));
            return Ok(VaultClient.GetSecret("LeonaldoTest").Value.Value);

        }

        // POST: EndDeviceEvent/
        [HttpPost]
        //public async Task<IActionResult> Post([FromBody] collection)
        public async Task<IActionResult> Post()
        {   
            //EndDeviceEventFirebase endDeviceEventFirebase = new EndDeviceEventFirebase();
            //endDeviceEventFirebase.application_id = collection.end_device_ids.application_ids.application_id;
            //endDeviceEventFirebase.dev_addr = collection.end_device_ids.dev_addr;
            //endDeviceEventFirebase.dev_eui = collection.end_device_ids.dev_eui;
            //endDeviceEventFirebase.device_id = collection.end_device_ids.device_id;
            //endDeviceEventFirebase.eui = collection.uplink_message.rx_metadata[0].gateway_ids.eui;
            //endDeviceEventFirebase.f_cnt = collection.uplink_message.f_cnt;
            //endDeviceEventFirebase.frm_payload = collection.uplink_message.frm_payload;
            //endDeviceEventFirebase.gateway_id = collection.uplink_message.rx_metadata[0].gateway_ids.gateway_id;
            //endDeviceEventFirebase.received_at = collection.received_at;

            //CollectionReference collectionReference = _fireStoreDb.Collection("end-device-webhook");
            //await collectionReference.AddAsync(endDeviceEventFirebase);

            return Ok();
        }
    }
}
