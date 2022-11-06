using AdofaiSRM.src;
using HarmonyLib;
using Newtonsoft.Json;
using Steamworks;
using System;
using System.Linq;
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
        private static WebSocket ws;
        private static Twitch twitch;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                var harmony = new Harmony("com.thijnmens.adofaisrm");
                harmony.PatchAll();

                mod = modEntry;
                ws = new WebSocket("wss://irc-ws.chat.twitch.tv:443");

                twitch = new Twitch(ws, modEntry.Logger);

                ws.OnMessage += OnMessage;
                ws.OnOpen += (object sender, EventArgs e) => { twitch.Connect(); };

                ws.Connect();

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
                    if (msg.StartsWith(">"))
                    {
                        string[] command = msg.Split(' ');
                        string msgID = e.Data.Split(';')[8].Split('=').Last();
                        switch (command[0].Trim())
                        {
                            //
                            // !srm <code>
                            //     code: platform:id [gg:3480] [steam:2830407654]
                            // Add song to queue
                            //
                            case ">srm":

                                HttpResponseMessage levelDataResult = AdofaiGG.getLevelData(command[1]);
                                if (!levelDataResult.IsSuccessStatusCode)
                                {
                                    twitch.SendReply(msgID, $"Sorry, we could not find a song with id {command[1]}. Did you make a typo?");
                                    mod.Logger.Warning($"Could not find song with id {command[1]}");
                                    break;
                                }

                                JsonObjects.LevelResult data = JsonConvert.DeserializeObject<JsonObjects.LevelResult>(levelDataResult.Content.ReadAsStringAsync().Result);

                                // Add level to queue
                                AddToQueue(data);
                                twitch.SendReply(msgID, $"I've sucessfully added {data.title} to the queue");
                                mod.Logger.Log($"Added {data.title} to queue with id {data.id}");
                                break;

                            //
                            // !scan
                            // Test if virustotal is configured correctly
                            //
                            case ">scan":

                                twitch.SendReply(msgID, "Scanning... WARNING: This will take around 30 seconds");
                                VirusScanner fileScan = new VirusScanner("", "", "");

                                // Upload file to virustotal
                                HttpResponseMessage fileUploadResult = fileScan.UploadFile();
                                if (!fileUploadResult.IsSuccessStatusCode)
                                {
                                    twitch.SendReply(msgID, "Something went wrong while uploading the file, please check the console.");
                                    mod.Logger.Error(fileUploadResult.ToString());
                                    break;
                                }
                                JsonObjects.UploadResult analysis = JsonConvert.DeserializeObject<JsonObjects.UploadResult>(fileUploadResult.Content.ReadAsStringAsync().Result);
                                string decodestring = Encoding.UTF8.GetString(Convert.FromBase64String(analysis.data.id));

                                await Task.Delay(30000); // Wait 30 seconds for analysis to complete

                                // Get analysis results back from virustotal
                                HttpResponseMessage analysisResult = fileScan.getAnalysisResults(decodestring.Split(':')[0]);
                                if (!analysisResult.IsSuccessStatusCode)
                                {
                                    twitch.SendReply(msgID, "Something went wrong while getting the analysis of the file, please check the console.");
                                    mod.Logger.Error(analysisResult.ToString());
                                    break;
                                }
                                JsonObjects.AnalysisResult fileReport = JsonConvert.DeserializeObject<JsonObjects.AnalysisResult>(analysisResult.Content.ReadAsStringAsync().Result);
                                twitch.SendReply(msgID, $"Scanning done! malicious: {fileReport.data.attributes.last_analysis_stats.malicious} harmless: {fileReport.data.attributes.last_analysis_stats.undetected}");

                                break;

                            //
                            // Unkown command
                            // Runs if the types command is unknown
                            //
                            default:

                                mod.Logger.Log(command[0]);
                                twitch.SendReply(msgID, $"Unkown command {command[0]}");

                                break;
                        }
                    }
                    break;

                default: break;
            }
        }

        private static void AddToQueue(JsonObjects.LevelResult data)
        {
            SteamUGC.SubscribeItem(new PublishedFileId_t(Convert.ToUInt64(data.workshop.Split('=').Last())));
        }
    }
}