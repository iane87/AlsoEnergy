using AE.CoreUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
namespace challenge
{
    [Serializable()]
    public class User
    {
        //Declare user properties
        public string username;
        public ConsoleColor FavoriteColor;
        public DateTime CreateDate;
        public TimeZoneInfo timeZone;
        public challenge.PermissionSet permissions;

        public string GenString()
        {
            string resp = "Username: " + username + "\n"; //Declare response and Add Username
            resp+= "Created Date: " + CreateDate + "\n"; //Add Created Date
            resp += "Time Zone: " + timeZone.ToString() + "\n"; //Add Time Zone
            resp += "Favorite Color: " + FavoriteColor.ToString() + "\n"; //Add Favorite Color
            resp += "Permissions: \n"; //Add Permissions Label
            foreach (Permission permission in permissions) //Loop through permissions
            {
                resp += permission.name + ":\n"; //Add Permission Name
                resp += "Read: " + permission.read.ToString() + "\n"; //Add Read Access
                resp += "Write: " + permission.write.ToString() + "\n"; //Add Write Access
            }
            return resp; //Return Details as String
        }

        public static void Serialize(User obj, Stream stream)
        {
            BlobIO blob = new BlobIO(); //Declare BlobIO
            BinaryFormatter bf = new BinaryFormatter(); //Declare Formatter
            blob.usr = obj; //Add user to BlobIO
            bf.Serialize(stream, blob); //Serialize as BlobIO
        }

        public static BlobIO DeSerialize(Stream stream)
        {
            BinaryFormatter bf = new BinaryFormatter(); //Declare Formatter
            return (BlobIO)bf.Deserialize(stream); //Deserialize as BlobIO
        }
    }
    public class users
    {
        ArrayList UserList; //Declare list of all users
        public users()
        {
            UserList = new ArrayList(); //Initialize list of all users
        }
        public User GetUser(string username) //Return user if found by name or return null
        {

            foreach (User u in UserList) //Loop through all users
            {
                if (u.username.ToLower() == username.ToLower()) //Detect match case insensitive
                    return u; //Return found user
            }
            return null; //User not found return null
        }
        public void AddUser(User u)
        {
            UserList.Add(u); //Add new user to list
        }

    }
}
