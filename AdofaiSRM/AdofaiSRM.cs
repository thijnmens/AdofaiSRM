
using System;
using UnityEngine;
using UnityModManagerNet;

namespace AdofaiSRM
{
    public class Main
    {

        private static SRMBot bot;

        public static bool enabled;
        public static UnityModManager.ModEntry mod;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                mod = modEntry;
                bot = new SRMBot();

                modEntry.OnToggle = OnToggle;
                modEntry.OnUpdate = OnUpdate;
                modEntry.OnGUI = OnGUI;

                return true;

            }
            catch (Exception e)
            {
                modEntry.Logger.Error(e.ToString());
                return false;
            }
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool active)
        {
            if (active)
            {
                Run();
            }
            else
            {
                Stop();
            }

            enabled = active;
            return true;
        }

        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            bot.ReadChat();
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.Label("Twitch bot token");
            if (GUILayout.Button("New twitch Token"))
            {
                modEntry.Logger.Log("Yep");
            }
        }

        static void Run()
        {

        }

        static void Stop()
        {

        }
    }
}
