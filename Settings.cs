using System;
using System.IO;
using System.Web.Script.Serialization;

namespace GorgaTech
{
    public class Settings : AppSettings<Settings>
    {
        public string IPport { get; set; }
        public string[] Nicks { get; set; }
        public string[] Enemys { get; set; }
        public short RadarPosX { get; set; }
        public short RadarPosY { get; set; }
        public double RadarOpacity { get; set; }
    }


    public class AppSettings<T> where T : new()
    {
        private const string DEFAULT_FILENAME = "settings.json";

        public void Save(string fileName = DEFAULT_FILENAME)
        {
            File.WriteAllText(fileName, (new JavaScriptSerializer()).Serialize(this));
        }

        public static void Save(T pSettings, string fileName = DEFAULT_FILENAME)
        {
            File.WriteAllText(fileName, (new JavaScriptSerializer()).Serialize(pSettings));
        }

        public static T Load(string fileName = DEFAULT_FILENAME)
        {
            T t = new T();
            if (File.Exists(fileName))
                t = (new JavaScriptSerializer()).Deserialize<T>(File.ReadAllText(fileName));
            return t;
        }
    }
}
