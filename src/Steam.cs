using Steamworks;

namespace AdofaiSRM.src
{
    internal class Steam
    {
        public static void SubcribeToItem(ulong id)
        {
            SteamUGC.SubscribeItem(new PublishedFileId_t(id));
        }
    }
}