﻿using System.ComponentModel;
using System.Threading.Tasks;
using TI2._2_HueApp.Connector;
using TI2._2_HueApp.lib;
using Windows.UI;

namespace TI2._2_HueApp.Enitity
{
    public class Light : INotifyPropertyChanged
    {
        #region Properties region

        private double _hue, _sat, _bri;
        private bool _state;
        private Color _color;

        public event PropertyChangedEventHandler PropertyChanged;

        public Color RGBColor
        {
            get { return _color; }
            set
            {
                if (value != _color)
                {
                    _color = value;
                    NotifyPropertyChanged(nameof(_color));
                }
            }

        }

        public int ID { get; set; }
        public string Name { get; set; }
        public bool State
        {
            get { return _state; }
            set
            {
                if (value != _state)
                {
                    _state = value;
                    NotifyPropertyChanged(nameof(_state));
                }
            }

        }
        public double Hue
        {
            get { return _hue; }
            set
            {
                if (value != this._hue)
                {
                    _hue = value;
                    NotifyPropertyChanged(nameof(_hue));
                    CalculateColor();
                    NotifyPropertyChanged(nameof(RGBColor));
                }

            }
        }
        public double Saturation
        {
            get { return _sat; }
            set
            {
                if (value != this._sat)
                {
                    _sat = value / 100.0;
                    NotifyPropertyChanged(nameof(_sat));
                    CalculateColor();
                    NotifyPropertyChanged(nameof(RGBColor));
                }
            }
        }
        public double Brightness
        {
            get { return _bri; }
            set
            {
                if (value != this._bri)
                {
                    _bri = value;
                    NotifyPropertyChanged(nameof(_bri));
                    CalculateColor();
                    NotifyPropertyChanged(nameof(RGBColor));
                }
            }
        }

        #endregion


        public Light(int id, string name, bool state, int hue, int sat, int bri)
        {
            ID = id;
            Name = name;
            State = state;
            Hue = ((double)hue * 360.0f) / 65535.0f;
            Saturation = (double)(sat / 255.0f);
            Brightness = (double)(bri / 255.0f);
            CalculateColor();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
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
            return "{\"hue\"" + value.ToString() + "}";
        }

        public string getSatToJson()
        {
            int value = (int)(Saturation * 255.0f);
            return "{\"sat\"" + value.ToString() + "}";
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

        #region SendJsonToBridge region

        /*
        public async Task SendStateToBridge(string relativeURIOfLight)
        {
            await HueAPIConnector.HttpPut(relativeURIOfLight, getStateToJson());
        }

        public async Task SendHueToBridge(string relativeURIOfLight)
        {
            await HueAPIConnector.HttpPut(relativeURIOfLight, getHueToJson());
        }

        public async Task SendSaturationToBridge(string relativeURIOfLight)
        {
            await HueAPIConnector.HttpPut(relativeURIOfLight, getSatToJson());
        }

        public async Task SendBrightnessToBridge(string relativeURIOfLight)
        {
            await HueAPIConnector.HttpPut(relativeURIOfLight, getBriToJson());
        }

        public async Task SendValuesToBridge(string relativeURIOfLight)
        {
            await HueAPIConnector.HttpPut(relativeURIOfLight, getToJson());
        }
        */
        #endregion

    }
}
