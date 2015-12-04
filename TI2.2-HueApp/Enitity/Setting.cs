using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI2._2_HueApp.Enitity
{
    public class Setting
    {

        public string Name { get; set; }
        public string Username { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }


        public Setting(string name, string username, string ip, int port)
        {
            Name = name;
            Username = username;
            IP = ip;
            Port = port;
        }



    }
}
