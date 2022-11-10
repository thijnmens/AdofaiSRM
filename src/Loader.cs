using System;
using System.IO;
using UnityModManagerNet;

namespace AdofaiSRM
{
    internal static class Loader
    {
        internal static void Load(UnityModManager.ModEntry modEntry)
        {
            File.Copy(Path.Combine(modEntry.Path, "System.Runtime.Serialization.dll"), Path.Combine(AppContext.BaseDirectory, "A Dance of Fire and Ice_Data/Managed/System.Runtime.Serialization.dll"), true);
            File.Copy(Path.Combine(modEntry.Path, "websocket-sharp.dll"), Path.Combine(AppContext.BaseDirectory, "A Dance of Fire and Ice_Data/Managed/websocket-sharp.dll"), true);
            File.Copy(Path.Combine(modEntry.Path, "System.Net.Http.dll"), Path.Combine(AppContext.BaseDirectory, "A Dance of Fire and Ice_Data/Managed/System.Net.Http.dll"), true);
            File.Copy(Path.Combine(modEntry.Path, "Newtonsoft.Json.dll"), Path.Combine(AppContext.BaseDirectory, "A Dance of Fire and Ice_Data/Managed/Newtonsoft.Json.dll"), true);

            AdofaiSRM.Load(modEntry);
        }
    }
}