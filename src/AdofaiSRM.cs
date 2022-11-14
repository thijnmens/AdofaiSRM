using AdofaiSRM.src;
using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;
using WebSocketSharp;

namespace AdofaiSRM
{
    public class AdofaiSRM
    {
        public static UnityModManager.ModEntry mod;
        public static uint[] requestedIds = new uint[1] { 2830407654u };
        private static Twitch twitch;
        private static readonly WebClient webClient = new WebClient();

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                var harmony = new Harmony("com.thijnmens.adofaisrm");
                harmony.PatchAll();

                mod = modEntry;
                WebSocket ws = new WebSocket("wss://irc-ws.chat.twitch.tv:443");

                Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "CustomSongs"));

                new Config();
                twitch = new Twitch(ws, modEntry.Logger);

                ws.OnMessage += OnMessage;
                ws.OnOpen += (object sender, EventArgs e) => { twitch.Connect(); };

                ws.Connect();

                mod.OnGUI += Queue.OnGUI;

                return true;
            }
            catch (Exception e)
            {
                modEntry.Logger.Error(e.ToString());
                return false;
            }
        }

        private static async void OnMessage(object sender, MessageEventArgs e)
        {
            switch (e.Data)
            {
                case string privmsg when privmsg.Contains("PRIVMSG"):
                    string msg = privmsg.Split(':').Last().ToLower();
                    if (msg.StartsWith("!"))
                    {
                        string[] command = msg.Split(' ');
                        string msgID = e.Data.Split(';')[8].Split('=').Last();
                        switch (command[0].Trim())
                        {
                            //
                            // !help
                            // Shows help page with all commands
                            //
                            case "!help":
                                twitch.SendReply(msgID, Translations.GetTranslation("help"));
                                break;

                            //
                            // !srm <code>
                            //     code: int
                            // Add song to queue
                            //
                            case "!srm":
                                if (command[1].Trim().Length == 10)
                                {
                                    // Steam
                                    mod.Logger.Log(command[1].Trim());
                                    Queue.AddToQueueSteam(command[1].TrimEnd('\r', '\n'));
                                    twitch.SendReply(msgID, Translations.GetTranslation("addedToQueue", command[1]));
                                    mod.Logger.Log($"Added worshop level to queue with id {command[1]}");
                                }
                                else
                                {
                                    // AdofaiGG
                                    HttpResponseMessage levelDataResult = AdofaiGG.getLevelData(command[1]);
                                    if (!levelDataResult.IsSuccessStatusCode)
                                    {
                                        twitch.SendReply(msgID, Translations.GetTranslation("songNotFound", command[1]));
                                        mod.Logger.Warning($"Could not find song with id {command[1]}");
                                        break;
                                    }
                                    JsonObjects.LevelResult data = JsonConvert.DeserializeObject<JsonObjects.LevelResult>(levelDataResult.Content.ReadAsStringAsync().Result);

                                    if (data.workshop == null)
                                    {
                                        // Custom File
                                        if (Config.workshopOnlyMode)
                                        {
                                            twitch.SendReply(msgID, Translations.GetTranslation("workshopOnlyMode"));
                                            mod.Logger.Log($"Requested non-workshop level with id {data.id}, but WorkshopOnlyMode is enabled");
                                        }
                                        else
                                        {
                                            try
                                            {
                                                twitch.SendReply(msgID, Translations.GetTranslation("downloadingFile", data.title));
                                                mod.Logger.Log($"Downloading file from {data.download}");

                                                // Download File
                                                Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, $"CustomSongs{Path.DirectorySeparatorChar}{data.id}"));
                                                webClient.DownloadFile(new Uri(data.download), Path.Combine(AppContext.BaseDirectory, $"CustomSongs{Path.DirectorySeparatorChar}{data.id}{Path.DirectorySeparatorChar}{data.id}.zip"));

                                                //Scan file
                                                VirusScanner scanner = new VirusScanner("application/x-zip-compressed", $"{data.id}.zip", Convert.ToBase64String(File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, $"CustomSongs{Path.DirectorySeparatorChar}{data.id}{Path.DirectorySeparatorChar}{data.id}.zip"))));
                                                HttpResponseMessage file = scanner.UploadFile();
                                                JsonObjects.UploadResult fileAnalysis = JsonConvert.DeserializeObject<JsonObjects.UploadResult>(file.Content.ReadAsStringAsync().Result);
                                                string fileDecodedString = Encoding.UTF8.GetString(Convert.FromBase64String(fileAnalysis.data.id));
                                                await Task.Delay(30000);
                                                HttpResponseMessage fileAnalysisResult = scanner.getAnalysisResults(fileDecodedString.Split(':')[0]);
                                                if (!fileAnalysisResult.IsSuccessStatusCode)
                                                {
                                                    twitch.SendReply(msgID, Translations.GetTranslation("downloadError"));
                                                    mod.Logger.Error(fileAnalysisResult.ToString());
                                                    break;
                                                }
                                                JsonObjects.AnalysisResult fileReport = JsonConvert.DeserializeObject<JsonObjects.AnalysisResult>(fileAnalysisResult.Content.ReadAsStringAsync().Result);
                                                mod.Logger.Log($"Scanning done! malicious: {fileReport.data.attributes.last_analysis_stats.malicious} harmless: {fileReport.data.attributes.last_analysis_stats.undetected}");
                                                if (fileReport.data.attributes.last_analysis_stats.malicious > 0)
                                                {
                                                    twitch.SendReply(msgID, Translations.GetTranslation("virusFound", fileReport.data.attributes.last_analysis_stats.malicious.ToString()));
                                                    mod.Logger.Log($"Removing file...");
                                                    File.Delete(Path.Combine(AppContext.BaseDirectory, $"CustomSongs{Path.DirectorySeparatorChar}{data.id}{Path.DirectorySeparatorChar}{data.id}.zip"));
                                                    break;
                                                }

                                                // Unzip file
                                                ZipUtil.Unzip(Path.Combine(AppContext.BaseDirectory, $"CustomSongs{Path.DirectorySeparatorChar}{data.id}{Path.DirectorySeparatorChar}{data.id}.zip"), Path.Combine(AppContext.BaseDirectory, $"CustomSongs{Path.DirectorySeparatorChar}{data.id}{Path.DirectorySeparatorChar}"));

                                                // Add to queue
                                                string[] files = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, $"CustomSongs{Path.DirectorySeparatorChar}{data.id}"), "*.adofai");
                                                files.ToList().Remove(Path.Combine(AppContext.BaseDirectory, $"CustomSongs{Path.DirectorySeparatorChar}{data.id}{Path.DirectorySeparatorChar}backup.adofai"));
                                                Queue.AddToQueueFile(data.id.ToString(), data.title, files.Last());
                                                twitch.SendReply(msgID, Translations.GetTranslation("addedToQueue", data.title));
                                                mod.Logger.Log($"Added {data.title} to queue with id {data.id}");
                                            }
                                            catch (Exception err)
                                            {
                                                twitch.SendReply(msgID, Translations.GetTranslation("downloadError", data.title));
                                                mod.Logger.Error($"An error ocurred while download file with id {data.id}, {err.Message}");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // Add level to queue from workshop
                                        Queue.AddToQueueAdofai(data.workshop.Split('=').Last(), data.title);
                                        twitch.SendReply(msgID, Translations.GetTranslation("addedToQueue", data.title));
                                        mod.Logger.Log($"Added {data.title} to queue with id {data.id}");
                                    }
                                }
                                break;

                            //
                            // !queue <page>
                            //     page: page number, default 1
                            //  Displays current queue up to 5 per page
                            //
                            case "!queue":
                                int page = 0;
                                if (command.Length == 2)
                                {
                                    page = int.Parse(command[1]) - 1;
                                }
                                List<string> queueItems = Queue.GetQueueNames(5, page);
                                if (queueItems.Count == 0)
                                {
                                    twitch.SendReply(msgID, Translations.GetTranslation("queuePageOutOfRange"));
                                    mod.Logger.Warning($"requested page {page} which does not exist");
                                    break;
                                }
                                string items = "";
                                foreach (string name in queueItems)
                                {
                                    items += $"{name} ";
                                }
                                twitch.SendReply(msgID, Translations.GetTranslation("queueItems", items));
                                break;
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    }
}