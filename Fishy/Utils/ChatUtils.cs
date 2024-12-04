using Fishy.Models;
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
            Console.WriteLine(message);
            new MessagePacket(message, color).SendPacket("all", (int)CHANNELS.GAME_STATE);
        }

        public static Player FindPlayer(string name)
        {
            foreach (var player in Fishy.Players)
            {
                if (player.Name == name)
                    return player;
                if (player.SteamID.ToString() == name)
                    return player;
            }
            foreach (var player in Fishy.Players)
            {
                if (player.Name.ToLower() == name.ToLower())
                    return player;
            }
            foreach (var player in Fishy.Players)
            {
                if (player.Name.ToLower().StartsWith(name))
                    return player;
            }
            return null;
        }
        public static Player FindPlayer(SteamId id)
        {
            foreach (var player in Fishy.Players)
            {
                if (player.SteamID == id)
                    return player;
            }
            return null;
        }
    }
}
