using System;

namespace AdofaiSRM.src
{
    internal class Translations
    {
        private static readonly string[] languages = new string[3] { "EN", "JP", "KR" };

        private static readonly string[] startupMessage = new string[3] { "Request songs with !srm <code>", "JP startupMessage", "KR startupMessage" };
        private static readonly string[] songNotFound = new string[3] { "Sorry, we could not find a song with id {0}. Did you make a typo?", "JP songNotFound", "KR songNotFound" };
        private static readonly string[] addedToQueue = new string[3] { "I've sucessfully added {0} to the queue", "JP addedToQueue", "KR addedToQueue" };
        private static readonly string[] workshopOnlyMode = new string[3] { "WorkshopOnly mode is enabled, you can only request songs that are on the steam workshop", "JP workshopOnlyMode", "KR workshopOnlyMode" };
        private static readonly string[] downloadingFile = new string[3] { "Downloading file... This might take a while", "JP downloadingFile", "KR downloadingFile" };
        private static readonly string[] downloadError = new string[3] { "An error ocurred while downloading, is the download a direct link?", "JP downloadError", "KR downloadError" };
        private static readonly string[] uploadError = new string[3] { "An error ocurred while uploading, are the servers down?", "JP help", "KR help" };
        private static readonly string[] help = new string[3] { "!help | !srm <code> | !queue <?page>", "JP help", "KR help" };
        private static readonly string[] queueItems = new string[3] { "Items in queue: {0}", "JP queueItems", "KR queueItems" };
        private static readonly string[] queuePageOutOfRange = new string[3] { "This page does not exist", "JP queuePageOutOfRange", "KR queuePageOutOfRange" };

        public static string GetTranslation(string id, string msg = "")
        {
            int i = Array.IndexOf(languages, Config.language);
            switch (id)
            {
                case "startupMessage":
                    return string.Format(startupMessage[i], msg);

                case "songNotFound":
                    return string.Format(songNotFound[i], msg);

                case "addedToQueue":
                    return string.Format(addedToQueue[i], msg);

                case "workshopOnlyMode":
                    return string.Format(workshopOnlyMode[i], msg);

                case "downloadingFile":
                    return string.Format(downloadingFile[i], msg);

                case "downloadError":
                    return string.Format(downloadError[i], msg);

                case "uploadError":
                    return string.Format(uploadError[i], msg);

                case "help":
                    return string.Format(help[i], msg);

                case "queueItems":
                    return string.Format(queueItems[i], msg);

                case "queuePageOutOfRange":
                    return string.Format(queuePageOutOfRange[i], msg);

                default: return $"Unkown translation \"{id}\"";
            }
        }
    }
}