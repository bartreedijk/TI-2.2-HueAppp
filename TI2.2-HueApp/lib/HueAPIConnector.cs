using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Web.Http;
using TI2._2_HueApp.Enitity;

namespace TI2._2_HueApp.Connector
{
    public class HueAPIConnector
    {
        public string Username { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }

        public HueAPIConnector(Setting setting)
        {
            Username = setting.Username;
            Ip = setting.IP;
            Port = setting.Port;
        }

        public HueAPIConnector(string username, string ip, int port)
        {
            Username = username;
            Ip = ip;
            Port = port;
        }

        public HueAPIConnector(string ip, int port)
        {
            //Username = "420b651cc9027734a8c4852b25a0ab";
            Ip = ip;
            Port = port;
        }

        public HueAPIConnector(string ip)
        {
            //Username = "420b651cc9027734a8c4852b25a0ab";
            Ip = ip;
            Port = 80;
        }

        public HueAPIConnector()
        {
            //Username = "420b651cc9027734a8c4852b25a0ab";
            Ip = "145.48.205.190";
            Port = 80;
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
                JsonObject outputSuccessObject = successObject.GetNamedObject("success");
                string username = outputSuccessObject.GetNamedString("username", "");
                this.Username = username;
                return Username;
            }
            return "";
        }

    }
}
