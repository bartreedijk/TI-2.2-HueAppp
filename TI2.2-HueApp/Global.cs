using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TI2._2_HueApp.Connector;
using TI2._2_HueApp.lib;
using TI2._2_HueApp.Enitity;

namespace TI2._2_HueApp
{
    internal class Global
    {
        public List<Light> Lights { get; private set; }

        public HueAPIConnector Connector { get; }

        private static Global _instance;

        public static Global Instance => _instance ?? (_instance = new Global());

        private Global()
        {
            Lights = new List<Light>();
            Connector = new HueAPIConnector();
        }

        public async Task<bool> InitializeConnection()
        {
            string json = await Connector.RetrieveLights();
            Lights = JsonUtil.convertJsonToLights(json);
            return !(json == "" || json == "[]");
        }

    }
}
