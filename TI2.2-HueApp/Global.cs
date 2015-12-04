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
        public static List<Light> Lights { get; private set; } = new List<Light>();
        public static List<Setting> Settings { get; set; } = new List<Setting>();

        public static HueAPIConnector Connector { get; private set; }

        public static void InitializeConnector(HueAPIConnector connector)
        {
            Connector = connector;
        }

        public static async Task<bool> InitializeLightsAsync()
        {
            string json = await Connector.RetrieveLights();
            Lights = JsonUtil.convertJsonToLights(json);
            return !(json == "" || json == "[]");
        }
    }
}
