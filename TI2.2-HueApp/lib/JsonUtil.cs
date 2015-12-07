using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using TI2._2_HueApp.Enitity;
using Windows.Data.Json;

namespace TI2._2_HueApp.lib
{
    public class JsonUtil
    {
       
        public static List<Light> convertJsonToLights(string json)
        {
            List<Light> lights = new List<Light>();

            JsonObject jsonObject;
            bool parseOk = JsonObject.TryParse(json, out jsonObject);

            if (!parseOk)
            {
                Debug.WriteLine(json);
            }


            foreach (string lightId in jsonObject.Keys)
            {
                JsonObject lightToAdd;
                Light l = null;

                try
                {
                    lightToAdd = jsonObject.GetNamedObject(lightId, null);
                    JsonObject lightState = lightToAdd.GetNamedObject("state", null);
                    if (lightState != null)
                    {
                        l = new Light(
                            Convert.ToInt32(lightId),
                            lightToAdd.GetNamedString("name", string.Empty),

                            lightState.GetNamedBoolean("on", false),
                            Convert.ToInt32(lightState.GetNamedNumber("hue", 0)),
                            Convert.ToInt32(lightState.GetNamedNumber("sat", 255)),
                            Convert.ToInt32(lightState.GetNamedNumber("bri", 255))
                            );
                        lights.Add(l);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                }

            }
            return lights;
        }


        private static string[] Serialize(List<Setting> settingList)
        {
            string settingListJSON = "";

            foreach(Setting s in settingList)
            {
                settingListJSON += s.toJson() + "|";
            }
            return settingListJSON.Split('|');
        }


        public static void SaveSettings()
        {
            File.WriteAllLines(@"Settings.txt", Serialize(Global.Settings));
        }

        public static void GetSettings()
        {
            string[] lines = File.ReadAllLines(@"Settings.txt");

            foreach (string line in lines)
            {
                if (line != "")
                {
                    string[] props = line.Split(',');
                    Global.Settings.Add(new Setting(props));
                }
            }

        }

    }
}
