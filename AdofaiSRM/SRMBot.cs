using System;
using System.IO;
using System.Net.Sockets;
using RestSharp;

namespace AdofaiSRM
{
    class SRMBot
    {

        private TcpClient twitchClient;
        private StreamReader reader;
        private StreamWriter writer;
        private RestClient client = new RestClient("https://adofai.gg:9200/api/v1");

        private readonly string username = "AdofaiSRM";
        private readonly string password = "oauth:z4gfznzkoa28iu3pp5s03hcppsaqq7"; // https://twitchapps.com/tmi/
        private readonly string channelName = "thijnmens";

        public SRMBot()
        {
            Connect();
        }

        public bool IsConnected()
        {
            if (twitchClient.Connected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Connect()
        {
            // Connect to twitch
            twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
            reader = new StreamReader(twitchClient.GetStream());
            writer = new StreamWriter(twitchClient.GetStream());

            // Authenticate with twitch
            writer.WriteLine("PASS " + password);
            writer.WriteLine("NICK " + username);
            writer.WriteLine("USER " + username + " 8 * :" + username);
            writer.WriteLine("JOIN #" + channelName);
            writer.Flush();
        }

        private void SendMessage(string message)
        {
            writer.WriteLine($"PRIVMSG #{channelName} :{message}");
            writer.Flush();
        }

        public async void ReadChat()
        {
            if (twitchClient.Available > 0)
            {
                string message = reader.ReadLine();

                if (message.Contains("PRIVMSG"))
                {
                    // Get username
                    int splitpoint = message.IndexOf("!", 1);
                    string chatName = message.Substring(0, splitpoint);
                    chatName = chatName.Substring(1);

                    // Get message
                    splitpoint = message.IndexOf(":", 1);
                    message = message.Substring(splitpoint + 1);
                    Console.WriteLine(string.Format("{0}: {1}", chatName, message));

                    // Is it a SRM command?
                    if (message.StartsWith("!srm"))
                    {
                        string[] cmd = message.Split(' ');
                        switch (cmd[1])
                        {
                            // Help message
                            case "help":
                                SendMessage("!srm <code> | !srm help");
                                break;

                            // Song Request
                            default:
                                RestResponse response = await client.GetAsync(new RestRequest($"levels/{cmd[1]}"));
                                if (!response.IsSuccessful)
                                {
                                    SendMessage($"Something went wrong while request songID {cmd[1]}, does it exist?");
                                }
                                else
                                {
                                    Main.mod.Logger.Log(response.Content.ToString());
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}
