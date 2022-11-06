using HarmonyLib;
using Newtonsoft.Json;
using Steamworks;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private static readonly TwitchHandler twitchHandler = new TwitchHandler();
        private static readonly HttpClient client = new HttpClient();

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                var harmony = new Harmony("com.thijnmens.adofaisrm");
                harmony.PatchAll();

                mod = modEntry;
                ws = new WebSocket("wss://irc-ws.chat.twitch.tv:443");

                ws.OnMessage += OnMessage;
                ws.OnOpen += TwitchConnect;

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
                    if (msg.StartsWith("!"))
                    {
                        string[] command = msg.Split(' ');
                        string msgID = e.Data.Split(';')[8].Split('=').Last();
                        switch (command[0].Trim())
                        {
                            case "!srm":
                                HttpResponseMessage result = client.GetAsync($"https://adofai.gg:9200/api/v1/levels/{command[1]}").Result;
                                if (!result.IsSuccessStatusCode)
                                {
                                    twitchHandler.SendReply(msgID, $"Sorry, we could not find a song with id {command[1]}. Did you make a typo?");
                                    break;
                                }
                                JsonObjects.Levels data = JsonConvert.DeserializeObject<JsonObjects.Levels>(result.Content.ReadAsStringAsync().Result);
                                AddToQueue(data);
                                twitchHandler.SendReply(msgID, $"I've sucessfully added {data.title} to the queue");
                                break;

                            case "!scan":
                                twitchHandler.SendReply(msgID, "Scanning... WARNING: This will take around 30 seconds");
                                var fileUploadRequest = new HttpRequestMessage
                                {
                                    Method = HttpMethod.Post,
                                    RequestUri = new Uri("https://www.virustotal.com/api/v3/files"),
                                    Headers =
                                    {
                                        { "accept", "application/json" },
                                        { "x-apikey", "8cefe816c41ffbd51ec1334f25dda645b6fc7fe88f64a6eebb0bae56e7ed200b" },
                                    },
                                    Content = new MultipartFormDataContent {
                                        new StringContent(TestFile.GetTestFile2())
                                        {
                                            Headers = {
                                                ContentType = new MediaTypeHeaderValue("application/octet-stream"),
                                                ContentDisposition = new ContentDispositionHeaderValue("form-data")
                                                {
                                                    Name = "file",
                                                    FileName = "eicar.com"
                                                }
                                            }
                                        }
                                    }
                                };
                                await Task.Delay(30000);
                                var fileUploadResult = client.SendAsync(fileUploadRequest).Result;
                                if (!fileUploadResult.IsSuccessStatusCode)
                                {
                                    twitchHandler.SendReply(msgID, $"Error! {fileUploadResult.Content.ReadAsStringAsync().Result}");
                                    mod.Logger.Error($"Error! {fileUploadResult.Content.ReadAsStringAsync().Result}");
                                    break;
                                }
                                JsonObjects.Analysis analysis = JsonConvert.DeserializeObject<JsonObjects.Analysis>(fileUploadResult.Content.ReadAsStringAsync().Result);
                                string decodestring = Encoding.UTF8.GetString(Convert.FromBase64String(analysis.data.id));
                                
                                var analysisRequest = new HttpRequestMessage
                                {
                                    Method = HttpMethod.Get,
                                    RequestUri = new Uri($"https://www.virustotal.com/api/v3/files/{decodestring.Split(':')[0]}"),
                                    Headers =
                                    {
                                        { "accept", "application/json" },
                                        { "x-apikey", "8cefe816c41ffbd51ec1334f25dda645b6fc7fe88f64a6eebb0bae56e7ed200b" },
                                    }
                                };
                                var analysisResult = client.SendAsync(analysisRequest).Result;
                                if (!analysisResult.IsSuccessStatusCode)
                                {
                                    twitchHandler.SendReply(msgID, $"Error! {analysisResult.Content.ReadAsStringAsync().Result}");
                                    mod.Logger.Error($"Error! {fileUploadResult.Content.ReadAsStringAsync().Result}");
                                    break;
                                }
                                JsonObjects.FileReport fileReport = JsonConvert.DeserializeObject<JsonObjects.FileReport>(analysisResult.Content.ReadAsStringAsync().Result);
                                twitchHandler.SendReply(msgID, $"Scanning done! malicious: {fileReport.data.attributes.last_analysis_stats.malicious} harmless: {fileReport.data.attributes.last_analysis_stats.undetected}");
                                break;

                            default:
                                mod.Logger.Log(command[0]);
                                twitchHandler.SendReply(msgID, $"Unkown command {command[0]}");
                                break;
                        }
                    }
                    break;

                default: break;
            }
        }

        private static void AddToQueue(JsonObjects.Levels data)
        {
            SteamUGC.SubscribeItem(new PublishedFileId_t(Convert.ToUInt64(data.workshop.Split('=').Last())));
        }

        private static void TwitchConnect(object sender, EventArgs e)
        {
            ws.Send("PASS oauth:z0be56q8k2aonc4ggdvm3jp9zufshj");
            ws.Send("CAP REQ :twitch.tv/membership twitch.tv/tags twitch.tv/commands");
            ws.Send("NICK AdofaiSRM");
            ws.Send("JOIN #sussybdrffityfty");
            ws.Send("PRIVMSG #sussybdrffityfty :Request songs with !srm <code>");
        }

        private class TwitchHandler
        {
            public void SendReply(string id, string msg)
            {
                ws.Send($"@reply-parent-msg-id={id} PRIVMSG #sussybdrffityfty :! {msg}");
                mod.Logger.Log($"Send reply with id {id} to #sussybdrffityfty with content: {msg}");
            }

            public void Send(string msg)
            {
                ws.Send($"PRIVMSG #sussybdrffityfty :! {msg}");
                mod.Logger.Log($"Send msg to #sussybdrffityfty with content: {msg}");
            }
        }
    }
}