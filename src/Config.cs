using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace AdofaiSRM.src
{
    public class Config
    {
        public static string virusTotalKey = "";
        public static string twitchKey = "";
        public static string youtubeKey = "";
        public static string language = "EN";
        public static bool workshopOnlyMode = true;
        public static List<string> channels = new List<string>();

        public Config()
        {
            using (StreamReader reader = new StreamReader($"{AdofaiSRM.mod.Path}{Path.DirectorySeparatorChar}info.json"))
            {
                JsonObjects.ConfigJson config = JsonConvert.DeserializeObject<JsonObjects.ConfigJson>(reader.ReadToEnd());
                virusTotalKey = config.Config.VirusTotalKey;
                twitchKey = config.Config.TwitchKey;
                youtubeKey = config.Config.YoutubeKey;
                language = config.Config.Language;
                workshopOnlyMode = config.Config.WorkshopOnlyMode;
                channels = config.Config.Channels;
            }
        }
    }
}