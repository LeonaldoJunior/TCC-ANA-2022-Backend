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

            //watertankList.ForEach(waterTank => waterTank.MaxVolume = MaxVolumeCalculation(waterTank));
            //myContext.SaveChanges();



            return Ok(watertankList);

            //return Ok(VaultClient.GetSecret("LeonaldoTest").Value.Value);
        }

        private double MaxVolumeCalculation(WaterTankList waterTank)
        {
            var height = Convert.ToDouble(waterTank.Height) - 0.08;
            var baseRadius = Convert.ToDouble(waterTank.BaseRadius);
            var topRadius = Convert.ToDouble(waterTank.TopRadius);

            var volume = (((Math.PI * height)/3) * (Math.Pow(baseRadius, 2) + (baseRadius * topRadius) + Math.Pow(topRadius, 2)))*1000;

            return volume;

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

            //var usersDevices = myContext.UsersAndDevices.Where(user => user.UserId == id).ToList();

            var userDeviceAndWaterTank = (from userDevice in myContext.UsersAndDevices
                                          where userDevice.UserId == id
                                          join waterTank in myContext.WaterTankList on userDevice.WaterTankId equals waterTank.WaterTankId
                                          select new
                                          {
                                              waterTank,
                                              userDevice,
                                          }).ToList();




            if (userDeviceAndWaterTank.Count > 0)
            {
                return Ok(userDeviceAndWaterTank);
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

            //var usersDevices = myContext.VolumeCalculation.OrderByDescending(volumeCalc => volumeCalc.UsersAndDevicesId == id).ToList();


            var userDeviceAndWaterTank = (from volumeCalc in myContext.VolumeCalculation
                                          join userDevice in myContext.UsersAndDevices on volumeCalc.UsersAndDevicesId equals userDevice.UsersAndDevicesId
                                          join waterTank in myContext.WaterTankList on userDevice.WaterTankId equals waterTank.WaterTankId
                                          join eventsEndDevice in myContext.EventsEndDevice on volumeCalc.EventId equals eventsEndDevice.EventId
                                          select new
                                          {
                                              waterTank,
                                              userDevice,
                                              volumeCalc,
                                              eventsEndDevice
                                          }).OrderByDescending(x => x.volumeCalc.EventId).FirstOrDefault();

            if (userDeviceAndWaterTank != null)
            {
                return Ok(userDeviceAndWaterTank);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet()]
        public IActionResult GetVolumeCalculationByUsersAndDevicesIdList(int id)
        {
            using Context myContext = new Context(_configuration);

            //var usersDevices = myContext.VolumeCalculation.OrderByDescending(volumeCalc => volumeCalc.UsersAndDevicesId == id).ToList();

            var userDeviceAndWaterTank = (from volumeCalc in myContext.VolumeCalculation
                                          join userDevice in myContext.UsersAndDevices on volumeCalc.UsersAndDevicesId equals userDevice.UsersAndDevicesId
                                          join waterTank in myContext.WaterTankList on userDevice.WaterTankId equals waterTank.WaterTankId
                                          join eventsEndDevice in myContext.EventsEndDevice on volumeCalc.EventId equals eventsEndDevice.EventId
                                          select new
                                          {
                                              waterTank,
                                              userDevice,
                                              volumeCalc,
                                              eventsEndDevice
                                          }).OrderByDescending(x => x.volumeCalc.EventId).Take(24).ToList();

            if (userDeviceAndWaterTank.Count > 0)
            {
                return Ok(userDeviceAndWaterTank);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet()]
        public IActionResult GetVolumeCalculationByUsersAndDevicesIdListFilterDay(int id, string lastDays)
        {
            using Context myContext = new Context(_configuration);

            //var usersDevices = myContext.VolumeCalculation.OrderByDescending(volumeCalc => volumeCalc.UsersAndDevicesId == id).ToList();
            var WhenSubmittedFilter = DateTime.Now.AddDays(-30);
            if (!String.IsNullOrEmpty(lastDays))
            {
                var isNumeric = int.TryParse(lastDays, out int Days);
                if(isNumeric)
                {
                    WhenSubmittedFilter = DateTime.Now.AddDays(-Days);
                }
            }

            var teste = DateTime.Parse("2022-04-14T01:32:49.534735717Z");

            var novoteste = DateTime.Parse("2022-04-14T01:32:49.534735717Z") >= WhenSubmittedFilter;

            var userDeviceAndWaterTank = (from volumeCalc in myContext.VolumeCalculation
                                          join userDevice in myContext.UsersAndDevices on volumeCalc.UsersAndDevicesId equals userDevice.UsersAndDevicesId
                                          join waterTank in myContext.WaterTankList on userDevice.WaterTankId equals waterTank.WaterTankId
                                          join eventsEndDevice in myContext.EventsEndDevice on volumeCalc.EventId equals eventsEndDevice.EventId
                                          where DateTime.Parse(eventsEndDevice.ReceivedAt) >= WhenSubmittedFilter
                                          select new
                                          {
                                              waterTank,
                                              userDevice,
                                              volumeCalc,
                                              eventsEndDevice
                                          }).OrderByDescending(x => x.volumeCalc.EventId).Take(24).ToList();

            if (userDeviceAndWaterTank.Count > 0)
            {
                return Ok(userDeviceAndWaterTank);
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

            //var usersDevices = myContext.UsersAndDevices.Where(user => (user.UserId == id && user.isSelected == true)).FirstOrDefault();

            var userDeviceAndWaterTank = (from userDevice in myContext.UsersAndDevices where userDevice.UserId == id && userDevice.isSelected == true
                                          join waterTank in myContext.WaterTankList on userDevice.WaterTankId equals waterTank.WaterTankId
                                          select new
                                          {
                                              waterTank,
                                              userDevice,
                                          }).FirstOrDefault();




            if (userDeviceAndWaterTank != null)
            {
                return Ok(userDeviceAndWaterTank);
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
                var deviceSelected = usersDevices.Find(user => user.isSelected = true);
                deviceSelected.isSelected = false;

                var deviceToChangeSelect = usersDevices.Find(user => user.UsersAndDevicesId == usersAndDevicesId);
                deviceToChangeSelect.isSelected = true;

                myContext.SaveChanges();
                return Ok();

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
