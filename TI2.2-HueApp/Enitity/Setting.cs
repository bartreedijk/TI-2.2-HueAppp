using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TI2._2_HueApp.Enitity
{
    [DataContract]
    public class Setting
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string IP { get; set; }
        [DataMember]
        public int Port { get; set; }


        public Setting(string name, string username, string ip, int port)
        {
            Name = name;
            Username = username;
            IP = ip;
            Port = port;
        }

        public Setting(string[] settingProps)
        {
            Name = settingProps[0];
            Username = settingProps[1];
            IP = settingProps[2];
            Port = Convert.ToInt32(settingProps[3]);
        }


        public string toJson()
        {
            return Name +"," + Username +","+ IP + "," + Port;
        }



    }
}
