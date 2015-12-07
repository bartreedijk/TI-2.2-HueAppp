using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.Web.Http;
using TI2._2_HueApp.Enitity;
using Windows.Storage;


namespace TI2._2_HueApp.Connector
{
    public class HueAPIConnector
    {
        public string Username { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }

        public HueAPIConnector(Setting setting)
        {
            //Username = setting.Username;
            //Ip = setting.IP;
            //Port = setting.Port;

            LoadSettings();
        }

        public HueAPIConnector(string username, string ip, int port)
        {
            Username = username;
            Ip = ip;
            Port = port;
        }

        public HueAPIConnector()
        {
            //Username = "420b651cc9027734a8c4852b25a0ab";
            //Ip = "145.48.205.190";
            //Port = 80;

            LoadSettings();
        }

        private async Task<string> HttpPost(string json)
        {
            return await HttpPost("", json);
        }

        private async Task<string> HttpPost(string relativeUri, string json)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(5000);

            try
            {
                HttpClient client = new HttpClient();

                Uri uri = new Uri($"http://{Ip}:{Port}/api/{relativeUri}");
                HttpStringContent httpContent = new HttpStringContent(json);
                HttpResponseMessage response = await client.PostAsync(uri, httpContent).AsTask(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine(jsonResponse);

                return jsonResponse;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<string> HttpPut(string relativeUri, string json)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(5000);

            try
            {
                HttpClient client = new HttpClient();

                Uri uri = new Uri($"http://{Ip}:{Port}/api/{Username}/{relativeUri}");
                HttpStringContent httpContent = new HttpStringContent(json);
                HttpResponseMessage response = await client.PutAsync(uri, httpContent).AsTask(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine(jsonResponse);

                return jsonResponse;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private async Task<string> HttpGet(string relativeUri)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(5000);

            try
            {
                HttpClient client = new HttpClient();

                Uri uri = new Uri($"http://{Ip}:{Port}/api/{Username}/{relativeUri}");
                HttpResponseMessage response = await client.GetAsync(uri).AsTask(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine(jsonResponse);

                return jsonResponse;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<string> RetrieveLights()
        {
            return await HttpGet("lights/");
        }

        public async Task<string> RetrieveSpecificLight(int lightIndex)
        {
            return await HttpGet($"lights/{lightIndex}/");
        }
        
        public async Task<string> Register(string deviceName)
        {
            //Register user
            string jsonIn = "{\"devicetype\":\"";
            jsonIn += "LightsOut#" + deviceName;
            jsonIn += "\"}";
            string json = await HttpPost(jsonIn);
            JsonArray tempArray;
            bool parseOK = JsonArray.TryParse(json, out tempArray);
            if (parseOK)
            {
                JsonObject successObject = tempArray[0].GetObject();
                JsonObject outputObject;
                try
                {
                    outputObject = successObject.GetNamedObject("success");
                    string username = outputObject.GetNamedString("username", "");
                    this.Username = username;
                }
                catch (Exception)
                {
                    try
                    {
                        outputObject = successObject.GetNamedObject("error");

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.StackTrace);
                    }
                }
                return Username;
            }
            return "";
        }

        private void LoadSettings()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            // try to read the values
            object value1 = localSettings.Values["BridgeIP"];
            object value2 = localSettings.Values["BridgeUsername"];
            object value3 = localSettings.Values["BridgePort"];

            // check if read is passed
            if (value1 == null)
            {
                localSettings.Values["BridgeIP"] = "145.48.205.190";
                this.Ip = "145.48.205.190";
            }
            else
            {
                this.Ip = value1 as string;
            }
            if (value2 == null)
            {
                localSettings.Values["BridgeUsername"] = "";
                this.Username = "";
            }
            else
            {
                this.Username = value2 as string;
            }
            if (value3 == null)
            {
                localSettings.Values["BridgePort"] = "80";
                this.Port = 80;
            }
            else
            {
                this.Port = int.Parse(value3 as string);
            }

        }

        public void SaveSettings(string bridgeIp, string bridgeUsername, string bridgePort)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (!string.IsNullOrEmpty(bridgeIp))
            {
                localSettings.Values["BridgeIP"] = bridgeIp;
            }
            if (!string.IsNullOrEmpty(bridgeUsername))
            {
                localSettings.Values["BridgeUsername"] = bridgeUsername;
                
            }
            if (!string.IsNullOrEmpty(bridgePort) && int.Parse(bridgePort) != 0)
            {
                localSettings.Values["BridgePort"] = bridgePort;
            }
            
            
        }

    }
}
