using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TI2._2_HueApp.Enitity;
using Windows.Data.Json;

namespace TI2._2_HueApp.lib
{
    public static class JsonUtil
    {

        public static List<Light> ConvertJsonToLights(string json)
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

        public static string CheckIfErrorInJson(string json)
        {
            JsonObject description = null;
            bool parseOk;
            try
            {
                JsonArray jsonArray;
                parseOk = JsonArray.TryParse(json, out jsonArray);

                if (parseOk)
                {
                    JsonObject jsonObject = jsonArray.GetObjectAt(0);
                    description = jsonObject.GetNamedObject("error");
                }
            }
            catch (Exception)
            {
                // we gaan ervan uit dat bij een parse Error er geen error in de json zit.
                return false.ToString();
            }
            // if generic error
            if ( (description == null && parseOk) || !parseOk )
            {
                return true.ToString();
            }

            // if specific error
            JsonValue value = description.GetNamedValue("description");
            return value.GetString();

        }
    }
}
