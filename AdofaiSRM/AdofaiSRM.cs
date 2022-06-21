using System;
using System.Timers;
using UnityModManagerNet;
using WebSocketSharp;
using System.Collections.Generic;
using ADOFAI;
using GDMiniJSON;
using HarmonyLib;
using System.Linq;

namespace AdofaiSRM
{
    public class AdofaiSRM
    {

        public static WebSocket ws;
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static System.Timers.Timer connectionRetryTimer;
        public static Dictionary<string, GenericDataCLS> extraLevels = new Dictionary<string, GenericDataCLS>();
        //public static FolderDataCLS requestFolder = new FolderDataCLS("SongRequestManager", 1, "Thijnmens", "", "SongRequestManager desc", "portal.png", "icon.png", "ffffff".HexToColor());
        public static uint[] requestedIds = new uint[0];

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                ws = InitClient("localhost", 666);
                var harmony = new Harmony("com.thijnmens.adofaisrm");

                ws.OnMessage += OnMessage;

                mod = modEntry;

                modEntry.OnToggle += OnToggle;

                harmony.PatchAll();

                return true;

            }
            catch (Exception e)
            {
                modEntry.Logger.Error(e.ToString());
                return false;
            }
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool active)
        {
            enabled = active;

            if (active)
            {
                //ws.Start();
            }
            else
            {
                //ws.Stop();
            }

            return true;
        } 

        private static void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            string message = messageEventArgs.Data.Split('=')[1];
            if (message != "Connection Success!")
            {
                SteamWorkshop.Subscribe(new Steamworks.PublishedFileId_t(uint.Parse(message)));
                requestedIds = requestedIds.Concat(new uint[] { uint.Parse(message) }).ToArray();
                Console.WriteLine(requestedIds.Length);
            }
            
        }

        public static LevelDataCLS DecodeLevel(uint id)
        {
            Dictionary<string, object> rootDict = Json.Deserialize(RDFile.ReadAllText($"C:\\Program Files (x86)\\Steam\\steamapps\\workshop\\content\\977950\\{id}\\main.adofai")) as Dictionary<string, object>;
            LevelDataCLS levelDataCLS = new LevelDataCLS();
            levelDataCLS.Setup();
            levelDataCLS.Decode(rootDict);
            return levelDataCLS;
        }

        private static WebSocket InitClient(string IPAdress, int Port)
        {
            WebSocket ws = new WebSocket($"ws://{IPAdress}:{Port}");
            connectionRetryTimer = new System.Timers.Timer(2000) { AutoReset = true };

            connectionRetryTimer.Elapsed += (object sender, ElapsedEventArgs e) => {
                ws.Connect();
                if (ws.Ping())
                {
                    connectionRetryTimer.Stop();
                }
            };

            connectionRetryTimer.Start();
            
            return ws;
        }
    }

    [HarmonyPatch(typeof(scnCLS), "Awake")]
    public class AwakePatch
    {
        [HarmonyPrefix]
        public static void Prefix(scnCLS __instance)
        {
            bool featuredLevelsModeField = (bool) AccessTools.Field(typeof(scnCLS), "featuredLevelsMode").GetValue(__instance);
            if (!featuredLevelsModeField)
            {
                Dictionary<string, GenericDataCLS> loadedLevelsField = (Dictionary<string, GenericDataCLS>)AccessTools.Field(typeof(scnCLS), "loadedLevels").GetValue(__instance);
                Dictionary<string, GenericDataCLS> extraLevelsField = (Dictionary<string, GenericDataCLS>)AccessTools.Field(typeof(scnCLS), "extraLevels").GetValue(__instance);
                Dictionary<string, bool> loadedLevelIsDeletedField = (Dictionary<string, bool>)AccessTools.Field(typeof(scnCLS), "loadedLevelIsDeleted").GetValue(__instance);
                Dictionary<string, bool> isWorkshopLevelField = (Dictionary<string, bool>)AccessTools.Field(typeof(scnCLS), "isWorkshopLevel").GetValue(__instance);
                Dictionary<string, string> loadedLevelDirsField = (Dictionary<string, string>)AccessTools.Field(typeof(scnCLS), "loadedLevelDirs").GetValue(__instance);

                FolderDataCLS requestFolder = new FolderDataCLS("SongRequestManager", 1, "Thijnmens", "", "SongRequestManager desc", "portal.png", "icon.png", "ffffff".HexToColor());

                for (int i = 0; i < AdofaiSRM.requestedIds.Length; i++)
                {
                    uint id = AdofaiSRM.requestedIds[i];
                    LevelDataCLS levelData = AdofaiSRM.DecodeLevel(id);
                    requestFolder.containingLevels.Add(id.ToString(), levelData);
                    extraLevelsField.Add(id.ToString(), levelData);
                    loadedLevelsField.Add(id.ToString(), levelData);
                    loadedLevelIsDeletedField.Add(id.ToString(), false);
                    isWorkshopLevelField.Add(id.ToString(), true);
                    loadedLevelDirsField.Add(id.ToString(), $"C:\\Program Files (x86)\\Steam\\steamapps\\workshop\\content\\977950\\{id}");
                    extraLevelsField[id.ToString()].parentFolderName = "Folder:SongRequestManager";
                }

                extraLevelsField.Add("Folder:SongRequestManager", requestFolder);

                AccessTools.Field(typeof(scnCLS), "loadedLevels").SetValue(__instance, loadedLevelsField);
                AccessTools.Field(typeof(scnCLS), "extraLevels").SetValue(__instance, extraLevelsField);
                AccessTools.Field(typeof(scnCLS), "loadedLevelIsDeleted").SetValue(__instance, loadedLevelIsDeletedField);
                AccessTools.Field(typeof(scnCLS), "isWorkshopLevel").SetValue(__instance, isWorkshopLevelField);
                AccessTools.Field(typeof(scnCLS), "loadedLevelDirs").SetValue(__instance, loadedLevelDirsField);
            }
        }
    }
}
