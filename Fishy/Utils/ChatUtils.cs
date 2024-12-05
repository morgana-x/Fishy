using Fishy.Models;
using Fishy.Models.Packets;
using Steamworks;
namespace Fishy.Utils
{
    public class ChatUtils
    {
        public static void SendChat(SteamId steamId, string message, string color="ffffff")
        {
            if (steamId == SteamClient.SteamId)
            {
                Console.WriteLine(message);
                return;
            }
            new MessagePacket(message, color).SendPacket("single", (int)CHANNELS.GAME_STATE, steamId);
        }
        public static void BroadcastChat(string message, string color="ffffff")
        {
            Console.WriteLine(message);
            new MessagePacket(message, color).SendPacket("all", (int)CHANNELS.GAME_STATE);
        }

        public static Player? FindPlayer(string name)
        {
            Player? player;

            // Get Player with exact name match
            player = Fishy.Players.FirstOrDefault(p => p.Name == name) ?? null;

            if (player != null)
                return player;

            // Try to get Player with SteamID
            player = Fishy.Players.FirstOrDefault(p => p.SteamID.ToString() == name) ?? null;

            if (player != null)
                return player;

            // Case insensitive search
            player = Fishy.Players.FirstOrDefault(p => p.Name.ToLower() == name.ToLower()) ?? null;

            if (player != null)
                return player;

            return null;
        }
        public static Player? FindPlayer(SteamId id)
            => Fishy.Players.FirstOrDefault(p => p.SteamID == id) ?? null;
        
    }
}
