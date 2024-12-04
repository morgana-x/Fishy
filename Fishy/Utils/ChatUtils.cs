using Fishy.Models.Packets;
using Steamworks;
namespace Fishy.Utils
{
    internal class ChatUtils
    {
        public static void SendChat(SteamId steamId, string message, string color="ffffff")
        {
            if (steamId == Steamworks.SteamClient.SteamId)
            {
                Console.WriteLine(message);
                return;
            }
            new MessagePacket(message, color).SendPacket("single", (int)CHANNELS.GAME_STATE, steamId);
        }
        public static void BroadcastChat(string message, string color="ffffff")
        {
            new MessagePacket(message, color).SendPacket("all", (int)CHANNELS.GAME_STATE);
        }
    }
}
