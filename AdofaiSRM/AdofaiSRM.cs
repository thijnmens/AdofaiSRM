using System;
using System.Timers;
using UnityModManagerNet;
using WebSocketSharp;

namespace AdofaiSRM
{
    public class AdofaiSRM
    {

        public static WebSocket ws;
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        private static System.Timers.Timer connectionRetryTimer;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                ws = InitClient("localhost", 666);

                ws.OnMessage += OnMessage;

                mod = modEntry;

                modEntry.OnToggle += OnToggle;
                modEntry.OnUpdate += OnUpdate;

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

        private static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {

        }

        private static void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            mod.Logger.Log(messageEventArgs.Data);
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
}
