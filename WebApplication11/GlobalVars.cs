using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebApplication11
{
    public class GlobalVars
    {
        public challenge.users users; //Declare Users
        public GlobalVars(){
            users = new challenge.users(); //Initialize Users
        }
    }
}
