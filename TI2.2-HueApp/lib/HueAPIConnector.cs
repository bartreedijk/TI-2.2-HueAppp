using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Web.Http;

namespace TI2._2_HueApp.Connector
{
    public class HueAPIConnector
    {
        public string Username { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }

        public HueAPIConnector()
        {
            RetrieveSettings();
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

                Uri uri = new Uri($"http://{IP}:{Port}/api/{relativeUri}");
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

        private async Task<string> HttpPut(string relativeUri, string json)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(5000);

            try
            {
                HttpClient client = new HttpClient();

                Uri uri = new Uri($"http://{IP}:{Port}/api/{Username}/{relativeUri}");
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

                Uri uri = new Uri($"http://{IP}:{Port}/api/{Username}/{relativeUri}");
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

        public void RetrieveSettings()
        {
            IP = "169.254.80.80";
            Port = 80;
        }

        public async Task<string> RetrieveLights()
        {
            return await HttpGet("lights/");
        }

        public async Task<string> RetrieveSpecificLight(int lightIndex)
        {
            return await HttpGet($"lights/{lightIndex}/");
        }

        public async Task Register()
        {
            //Register user
            
            string json = await HttpPost(("{\"devicetype\":\"mobileHueApp#WinPhoneBart\"}"));
            JsonArray tempArray;
            bool parseOK = JsonArray.TryParse(json, out tempArray);            
            JsonObject successObject = tempArray[0].GetObject();
            JsonObject outputSuccessObject = successObject.GetNamedObject("success");
            string username = outputSuccessObject.GetNamedString("username", "");
            this.Username = username;
        }

        public void SetLight()
        {

        }

    }
}
