using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TI2._2_HueApp.lib;
using Windows.UI;
using TI2._2_HueApp.Connector;

namespace TI2._2_HueApp.Enitity
{
    public class Light
    {
	#region Properties region

        private double _hue, _sat, _bri;

	public Color RGBColor { get; set; }

        public int ID { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }
        public double Hue
        {
            get { return _hue; }
            set
            {
                _hue = value;
                CalculateColor();
            }
        }
        public double Saturation
        {
            get { return _sat; }
            set
            {
                _sat = value;
                CalculateColor();
            }
        }
        public double Brightness
        {
            get { return _bri; }
            set
            {
                _bri = value;
                CalculateColor();
            }
        }

	#endregion

        
        public Light(int id, string name, bool state, int hue, int sat, int bri)
        {
            ID = id;
            Name = name;
            State = state;
            Hue = ((double)hue * 360.0f) / 65535.0f;
            Saturation = (double)sat / 255.0f;
            Brightness = (double)bri / 255.0f;
            CalculateColor();
        }

        public void CalculateColor()
        {
            RGBColor = ColorUtil.getColor(this);
        }


        #region GetJson region

        public string getStateToJson()
        {
            return "{\"on\"" + (State ? "true" : "false") + "}";
        }

        public string getHueToJson()
        {
            int value = (int)((Hue * 65535.0f) / 360);
            return "{\"sat\"" + value.ToString() + "}";
        }

        public string getSatToJson()
        {
            int value = (int)(Saturation * 255.0f);
            return "{\"hue\"" + value.ToString() + "}";
        }

        public string getBriToJson()
        {
            int value = (int)(Brightness * 255.0f);
            return "{\"bri\"" + value.ToString() + "}";
        }

        public string getToJson()
        {
            int hue = (int)((Hue * 65535.0f) / 360);
            int sat = (int)(Saturation * 255.0f);
            int bri = (int)(Brightness * 255.0f);
            return "{\"on\"" + (State ? "true" : "false") + ", \"hue\"" + hue.ToString() + ",\"sat\"" + sat.ToString() + ",\"bri\"" + bri.ToString() + "}";
        }

        #endregion
        /*
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
    */
    }
}
