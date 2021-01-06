using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge;
using AE.CoreUtility;
using System.IO;

namespace WebApplication11.Controllers
{

    [ApiController]
    [Route("[controller]/user/{username}")]  //Path to get user info {username} variable comes from URL
    public class AlsoEnergyController : ControllerBase
    {
        private readonly ILogger<AlsoEnergyController> _logger;
        private readonly GlobalVars _globalVars; //Declare Global Variables
        

        public AlsoEnergyController(ILogger<AlsoEnergyController> logger, GlobalVars globalVars)
        {
            _logger = logger;
            _globalVars = globalVars; //Set to single instance of Global Variables
        }

        [HttpGet]
        public string Get(string username)
        { 
            User foundUser = _globalVars.users.GetUser(username);  //Try to find existing User
            Stream sUser =new MemoryStream(); //Declare user stream
            StreamReader streamReader = new StreamReader(sUser); //Declare StreamReader
            string resp; //Declare response string
            if (foundUser is null) //User not found
            {
                User NewUser = new User(); //Create new user
                var rng = new Random(); //Declare Random Number Generator
                int c = rng.Next(0, 15); //Generate Random Number for Color
                int z = rng.Next(0, TimeZoneInfo.GetSystemTimeZones().Count - 1); //Generate Random Number for Time Zone
                ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor)); //Get console colors
                PermissionSet PS = new PermissionSet(); //Declare new Permissions
                foreach (ConsoleColor color in colors) //Loop through all console colors
                {
                    if (color.ToInt<ConsoleColor>() == c) 
                        NewUser.FavoriteColor = color; //Set Favorite Color equal to Color corresponding to random number
                }
                Object o = TimeZoneInfo.GetSystemTimeZones().ToArray().GetValue(z); //Random Time Zone Object
                TimeZoneInfo TZ = (TimeZoneInfo)o; //Convert object to TimeZoneInfo
                foreach (TimeZoneInfo timeZone in TimeZoneInfo.GetSystemTimeZones()) //Loop through all system Time Zones
                {
                    if (TZ == timeZone)
                        NewUser.timeZone = timeZone; //Set Time Zone equal to Time Zone corresponding to random number
                }
                NewUser.username = username; //Set username as provided by URL
                NewUser.CreateDate = DateTime.Now; //Set Create Date to Now
                for (int i = 1; i <= 100; i++) //Add 100 permissions
                {
                    challenge.Permission P = new challenge.Permission(); //Declare Permission

                    P.name = "PERM" + i; //Name Permission
                    P.read = Convert.ToBoolean(rng.Next(-1, 1)); //Random read access
                    P.write = Convert.ToBoolean(rng.Next(-1, 1)); //Random write access

                    PS.AddPermission(P); //Add permission to set
                }
                NewUser.permissions = PS; //Add permission set to user
                _globalVars.users.AddUser(NewUser); //Add user to User List
                challenge.User.Serialize(NewUser, sUser); //Serialize new user
                resp = NewUser.GenString(); //Generate string for new user
            }
            else
            {
                challenge.User.Serialize(foundUser, sUser); //Serialize found user
                resp = foundUser.GenString(); //Generate string for found user
            }
            sUser.Seek(0, SeekOrigin.Begin); //Start at begining of stream
            resp += "Serialized Data:\n" + streamReader.ReadToEnd();
            return resp; //Return user
        }

    }
}
