using System;
using System.IO;
using UnityModManagerNet;

namespace AdofaiSRM
{
    internal static class Startup
    {

        internal static void Load(UnityModManager.ModEntry modEntry)
        {
            LoadAssembly("Mods/AdofaiSRM/RestSharp.dll");

            Main.Load(modEntry);
        }

        private static void LoadAssembly(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
                AppDomain.CurrentDomain.Load(data);
            }
        }
    }
}
