using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace challenge
{
    [Serializable()]
    public class PermissionSet : IEnumerable
    {
        int i; //Declare current permission number
        Permission[] PermissionList = new Permission[100]; //Declare Permission Array

        public byte[] getSerialialezed() //Serialize to bytes
        {
            byte[] bytes; //Declare byte array
            IFormatter formatter = new BinaryFormatter(); //Declare formatter
            using (MemoryStream stream = new MemoryStream()) //Declare Memory Stream
            {
                   formatter.Serialize(stream, PermissionList); //Serialize to stream
                   bytes = stream.ToArray(); //Stream to byte array
                }
            return bytes; //Return byte array
        }
        public string AddPermission(Permission p) //Add permission to Array
        {
            if (i > 99) //Already at or above 100 permissions return full
                return "Full";
            PermissionList.SetValue(p, i); //Add permission
            i++; //Increment total permissions
            return "Success"; //Permission successfully added return "Success"
        }
        //IEnumerator Needed to use foreach 
        // Implementation for the GetEnumerator method.
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public PermissionSetEnum GetEnumerator()
        {
            return new PermissionSetEnum(PermissionList);
        }
    }
    //IEnumerator Needed to use foreach
    // When you implement IEnumerable, you must also implement IEnumerator.
    public class PermissionSetEnum : IEnumerator
    {
        public Permission[] _perm;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public PermissionSetEnum(Permission[] list)
        {
            _perm= list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _perm.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public Permission Current
        {
            get
            {
                try
                {
                    return _perm[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }

    [Serializable()]
    public class Permission  //Hold individual Permission
    {
        public string name; //Permission name
        public bool read; //Read Access
        public bool write; //Write Access
    }
}
