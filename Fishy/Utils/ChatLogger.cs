using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;

namespace Fishy.Utils
{
    class ChatLogger
    {
        public static List<ChatMessage> ChatLogs = new List<ChatMessage>();

        public static void Log(ChatMessage message)
        {
            ChatLogs.Add(message);
            if (ChatLogs.Count > 100)
                ChatLogs.RemoveAt(0);
            Console.WriteLine(message.ToString());
        }

        public static string GetLog(SteamId? user = null)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (ChatMessage message in ChatLogs)
            {
                if (user == null)
                {
                    stringBuilder.AppendLine(message.ToString());
                }
                else
                {
                    if (message.UserID.Equals(user.ToString()))
                        stringBuilder.AppendLine(message.ToString());
                }
            }
            return stringBuilder.ToString();
        }
    }

    public class ChatMessage (SteamId user, string message)
    {
        public DateTime SentAt { get; set; } = DateTime.Now;
        public string UserID { get; set; } = user.ToString();
        public string UserName { get; set; } = Fishy.Players.FirstOrDefault(p => p.SteamID == user, new Models.Player(new SteamId(), "")).Name ?? user.ToString();
        public string Message { get; set; } = message;

        public override string ToString()
        {
            return $"{SentAt.ToString("dd.MM HH:mm:ss")} {UserName}({UserID}): {Message}";
        }
    }
}
