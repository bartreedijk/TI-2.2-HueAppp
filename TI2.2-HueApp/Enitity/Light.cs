using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TI2._2_HueApp.Connector;

namespace TI2._2_HueApp.Enitity
{
    class Light
    {
        #region Properties region
        public int ID { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }
        public int Hue { get; set; }
        public int Saturation { get; set; }
        public int Brightness { get; set; }
        #endregion
        public Light(int id, string name, bool state, int hue, int sat, int bri)
        {
            ID = id;
            Name = name;
            State = state;
            Hue = hue;
            Saturation = sat;
            Brightness = bri;
        }


        #region privateGetJson region

        private string getStateToJson()
        {
            return "{\"on\"" + (State ? "true" : "false") + "}";
        }
        private string getSatToJson()
        {
            return "{\"sat\"" + Saturation.ToString() + "}";
        }
        private string getHueToJson()
        {
            return "{\"hue\"" + Hue.ToString() + "}";
        }
        private string getBriToJson()
        {
            return "{\"bri\"" + Brightness.ToString() + "}";
        }
        private string getToJson()
        {
            return "{\"on\"" + (State ? "true" : "false") + ", \"hue\"" + Hue.ToString() + ",\"sat\"" + Saturation.ToString() + ",\"bri\"" + Brightness.ToString() + "}";
        }

        #endregion

        #region SendJsonToBridge region

        public async Task SendStateToBridge(string relativeURIOfLight)
        {
            await Global.Connector.HttpPut(relativeURIOfLight, getStateToJson());
        }

        public async Task SendHueToBridge(string relativeURIOfLight)
        {
            await Global.Connector.HttpPut(relativeURIOfLight, getHueToJson());
        }

        public async Task SendSaturationToBridge(string relativeURIOfLight)
        {
            await Global.Connector.HttpPut(relativeURIOfLight, getSatToJson());
        }

        public async Task SendBrightnessToBridge(string relativeURIOfLight)
        {
            await Global.Connector.HttpPut(relativeURIOfLight, getBriToJson());
        }

        public async Task SendValuesToBridge(string relativeURIOfLight)
        {
            await Global.Connector.HttpPut(relativeURIOfLight, getToJson());
        }

        #endregion
    }
}
