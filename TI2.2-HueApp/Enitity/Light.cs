using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TI2._2_HueApp.lib;
using Windows.UI;

namespace TI2._2_HueApp.Enitity
{
    public class Light
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }
        public int Hue { get; set; }
        public int Saturation { get; set; }
        public int Brightness { get; set; }
        public Color RGBColor { get; set; }

        public Light(int id, string name, bool state, int hue, int sat, int bri)
        {
            ID = id;
            Name = name;
            State = state;
            Hue = hue;
            Saturation = sat;
            Brightness = bri;
            RGBColor = ColorUtil.getColor(this);
        }

        public string getStateToJson()
        {
            return "{\"on\"" + (State ? "true" : "false") + "}";
        }

        public string getSatToJson()
        {
            return "{\"sat\"" + Saturation.ToString() + "}";
        }

        public string getHueToJson()
        {
            return "{\"hue\"" + Hue.ToString() + "}";
        }

        public string getBriToJson()
        {
            return "{\"bri\"" + Brightness.ToString() + "}";
        }

        public string getToJson()
        {
            return "{\"on\"" + (State ? "true" : "false") + ", \"hue\"" + Hue.ToString() + ",\"sat\"" + Saturation.ToString() + ",\"bri\"" + Brightness.ToString() + "}";
        }

    }
}
