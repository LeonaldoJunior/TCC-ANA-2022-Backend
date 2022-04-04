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
    public class UserUiController : ControllerBase
    {
        readonly IConfiguration _configuration;
        public UserUiController(IConfiguration iConfig)
        {
            _configuration = iConfig;
        }

        [HttpGet()]
        public IActionResult GetUserbyId(string id)
        {
            using Context myContext = new Context(_configuration);

            //var userIDExist = myContext.UserLists.Where(p => p.UserId == id).Any();

            if (myContext.UserList.Any(user => user.UserId == id))
            {
                return Ok(id);

            }
            else
            {
                return NotFound(id);
            }
        }

        [HttpGet]
        public IActionResult GetAllWaterTank()
        {
            using Context myContext = new Context(_configuration);

            var watertankList = myContext.WaterTankList.ToList();
            return Ok(watertankList);

            //return Ok(VaultClient.GetSecret("LeonaldoTest").Value.Value);
        }


        // POST: WebHookController/
        [HttpPost]
        public IActionResult NewUserDevice([FromForm] string userId, [FromForm] string endDeviceID, [FromForm] int waterTankId, [FromForm] string waterTankName)
        //public async Task<IActionResult> Post()
        {
            using Context myContext = new Context(_configuration);

            if (myContext.EndDeviceList.Any(device => device.EndDeviceId == endDeviceID))
            {
                if(!myContext.UsersAndDevices.Any(userDevice => userDevice.EndDeviceID == endDeviceID))
                {
                    var newUsersAndDevices = new UsersAndDevices();
                    newUsersAndDevices.UserId = userId;
                    newUsersAndDevices.EndDeviceID = endDeviceID;
                    newUsersAndDevices.WaterTankId = waterTankId;
                    newUsersAndDevices.WaterTankName = waterTankName;
                    newUsersAndDevices.isSelected = false;

                    myContext.Add(newUsersAndDevices);
                    myContext.SaveChanges();

                    return Ok();

                }
                else
                {
                    return BadRequest();
                }

            }
            else
            {
                return NotFound();
            }
        }

        

        [HttpGet()]
        public IActionResult GetAllDevicesByUserId(string id)
        {
            using Context myContext = new Context(_configuration);

            var usersDevices = myContext.UsersAndDevices.Where(user => user.UserId == id).ToList();

            if(usersDevices.Count > 0)
            {
                return Ok(usersDevices);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet()]
        public IActionResult GetVolumeCalculationByUsersAndDevicesId(int id)
        {
            using Context myContext = new Context(_configuration);

            var usersDevices = myContext.VolumeCalculation.OrderByDescending(volumeCalc => volumeCalc.UsersAndDevicesId == id).ToList();


            var userDeviceAndWaterTank = (from volumeCalc in myContext.VolumeCalculation
                                          join userDevice in myContext.UsersAndDevices on volumeCalc.UsersAndDevicesId equals userDevice.UsersAndDevicesId
                                          join waterTank in myContext.WaterTankList on userDevice.WaterTankId equals waterTank.WaterTankId
                                          select new
                                          {
                                              waterTank,
                                              userDevice,
                                              volumeCalc
                                          }).OrderByDescending(x => x.volumeCalc.EventId).FirstOrDefault();



            if (usersDevices.Count > 0)
            {
                return Ok(usersDevices);
            }
            else
            {
                return NotFound();
            }
        }
        

        [HttpGet()]
        public IActionResult GetSelectedDeviceByUserId(string id)
        {
            using Context myContext = new Context(_configuration);

            var usersDevices = myContext.UsersAndDevices.Where(user => (user.UserId == id && user.isSelected == true)).FirstOrDefault();

            if (usersDevices != null)
            {
                return Ok(usersDevices);
            }
            else
            {
                return NotFound();
            }
        }

        

        // POST: WebHookController/
        [HttpPatch()]
        public IActionResult PatchUsersAndDevicesById([FromForm] int usersAndDevicesId, [FromForm] string userId)
        //public async Task<IActionResult> Post()
        {
            using Context myContext = new Context(_configuration);

            var usersDevices = myContext.UsersAndDevices.Where(user => user.UserId == userId).ToList();

            if(usersDevices.Count > 1)
            {
                return NoContent();

            }
            else
            {
                usersDevices[0].isSelected = true;
                myContext.SaveChanges();
                return Ok();

            }
        }


        [HttpDelete()]
        public IActionResult DelDeviceByDeviceId(string id)
        {
            using Context myContext = new Context(_configuration);

            var usersDevice = myContext.UsersAndDevices.Where(user => user.EndDeviceID == id).FirstOrDefault();

            if(usersDevice.EndDeviceID != "")
            {
                myContext.UsersAndDevices.Remove(usersDevice);
                myContext.SaveChanges();

                return Ok();
            }
            else{
                return NotFound();
            }

        }

    }
}
