using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TI2._2_HueApp.lib;
using TI2._2_HueApp.Enitity;

namespace TI2._2_HueApp
{
    class Global
    {
        public List<Light> Lights { get; private set; }
        public static Connector.HueAPIConnector Connector { get; private set; }

        private static Global _instance;

        public static Global Instance { get { return _instance ?? (_instance = new Global()); } }

        private Global()
        {

        }
        
    }
}
