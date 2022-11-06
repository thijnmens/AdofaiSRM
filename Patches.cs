using ADOFAI;
using GDMiniJSON;
using HarmonyLib;
using System.Collections.Generic;

namespace AdofaiSRM
{
    internal class Patches
    {
        // Way too broken so no ingame level select for now
       /* public static Dictionary<string, GenericDataCLS> extraLevels = new Dictionary<string, GenericDataCLS>();

        [HarmonyPatch(typeof(scnCLS), "Awake")]
        private static class AwakePatch
        {
            [HarmonyPrefix]
            private static void Prefix(scnCLS __instance)
            {

                FolderDataCLS folderDataCLS = new FolderDataCLS("AdofaiSRM", 10, "AdofaiSRM Mod", "Thijnmens", "Are you ready to experience real pain?", "portal.png", "icon.png", "B8E2F2".HexToColor());
                uint[] extraLevelsIds =  new uint[1] { 2830407654u };
                for (int i = 0; i < extraLevelsIds.Length; i++)
                {
                    uint id = extraLevelsIds[i];
                    LevelDataCLS value = DecodeLevel(id);
                    folderDataCLS.containingLevels.Add(id.ToString(), value);
                    extraLevels[id.ToString()].parentFolderName = "Folder:AdofaiSRM";
                }
                extraLevels.Add("Folder:AdofaiSRM", folderDataCLS);
            }
        }

        [HarmonyPatch(typeof(scnCLS), "ScanLevels")]
        private class ScanLevelsPatch
        {
            private void PostFix(scnCLS __instance)
            {
                bool featuredLevelMode = (bool)AccessTools.Field(typeof(scnCLS), "featuredLevelsMode").GetValue(__instance);
                Dictionary<string, bool> loadedLevelIsDeleted = (Dictionary<string, bool>)AccessTools.Field(typeof(scnCLS), "loadedLevelIsDeleted").GetValue(__instance);
                Dictionary<string, bool> isWorkshopLevel = (Dictionary<string, bool>)AccessTools.Field(typeof(scnCLS), "isWorkshopLevel").GetValue(__instance);
                if (featuredLevelMode)
                {
                    foreach (KeyValuePair<string, GenericDataCLS> extraLevel in extraLevels)
                    {
                        
                        string key = extraLevel.Key;
                        if (!scnCLS.instance.loadedLevels.ContainsKey(key))
                        {
                            scnCLS.instance.loadedLevels.Add(key, extraLevel.Value);
                            scnCLS.instance.loadedLevels.Add(key, null);
                            loadedLevelIsDeleted[key] = false;
                            isWorkshopLevel[key] = true;
                        }
                    }
                }
            }
        }

        private static LevelDataCLS DecodeLevel(uint id)
        {
            Dictionary<string, object> rootDict = Json.Deserialize(RDFile.ReadAllText($"C:\\Program Files (x86)\\Steam\\steamapps\\workshop\\content\\977950\\{id}\\main.adofai")) as Dictionary<string, object>;
            LevelDataCLS levelDataCLS = new LevelDataCLS();
            levelDataCLS.Setup();
            levelDataCLS.Decode(rootDict);
            return levelDataCLS;
        }*/
    }
}
