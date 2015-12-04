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
        public static T Deserialize<T>(string json)
        {
            var _Bytes = Encoding.Unicode.GetBytes(json);
            using (MemoryStream _Stream = new MemoryStream(_Bytes))
            {
                var _Serializer = new DataContractJsonSerializer(typeof(T));
                return (T)_Serializer.ReadObject(_Stream);
            }
        }

        public static string Serialize(object instance)
        {
            using (MemoryStream _Stream = new MemoryStream())
            {
                var _Serializer = new DataContractJsonSerializer(instance.GetType());
                _Serializer.WriteObject(_Stream, instance);
                _Stream.Position = 0;
                using (StreamReader _Reader = new StreamReader(_Stream))
                { return _Reader.ReadToEnd(); }
            }
        }


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


        public static void SaveSettings()
        {
            File.WriteAllText(@"Settings.json", Serialize(Global.Settings));
        }

        public static void GetSettings()
        {
            JsonArray array;
            JsonArray.TryParse(File.ReadAllText(@"Settings.json"), out array);
            
            foreach(Setting s in array)
            {
                Global.Settings.Add(s);
            }

        }

    }
}
