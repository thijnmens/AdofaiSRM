using UnityModManagerNet;
using WebSocketSharp;

namespace AdofaiSRM.src
{
    internal class Twitch
    {
        private WebSocket ws;
        private UnityModManager.ModEntry.ModLogger logger;
        private string[] channels = { "sussybdrffityfty" };

        public Twitch(WebSocket ws, UnityModManager.ModEntry.ModLogger logger)
        {
            this.ws = ws;
            this.logger = logger;
        }

        public void Connect()
        {
            ws.Send($"PASS oauth:{Secrets.GetTwitchApiKey()}");
            ws.Send("CAP REQ :twitch.tv/membership twitch.tv/tags twitch.tv/commands");
            ws.Send("NICK AdofaiSRM");
            foreach (string channel in channels)
            {
                ws.Send($"JOIN #{channel}");
                logger.Log($"Connected to channel {channel}");
                Send($"Request songs with >srm <code>", channel);
            }
        }

        public void SendReply(string msgId, string msg)
        {
            ws.Send($"@reply-parent-msg-id={msgId} PRIVMSG #{channels[0]} :! {msg}");
            logger.Log($"Send reply with id {msgId} to #{channels[0]}: {msg}");
        }

        public void Send(string msg, string channel)
        {
            ws.Send($"PRIVMSG #{channel} :! {msg}");
            logger.Log($"Send msg to #{channel}: {msg}");
        }
    }
}