﻿using System;
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
        public List<Light> Lights { get; set; }

        public Global()
        {
            InitializeConnection();
        }

        private async void InitializeConnection()
        {
            Connector.HueAPIConnector connector = new Connector.HueAPIConnector();
            await connector.Register();
            string json = await connector.RetrieveLights();
            Lights = JsonConversie.convertJsonToLights(json);
            System.Diagnostics.Debug.WriteLine(Lights.ToString());
        }
    }
}