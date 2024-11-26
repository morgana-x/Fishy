using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fishy.Models;
using Fishy.Models.Packets;
using Fishy.Utils;
using Steamworks;

namespace Fishy.Extensions
{
    public abstract class FishyExtension
    {
        public static List<Player> Players { get => Fishy.Players; }
        public static List<string> BannedPlayers { get => Fishy.BannedUsers; }
        public static List<Actor> Actors { get => Fishy.Actors; }
        public static List<ChatMessage> ChatLog { get => ChatLogger.ChatLogs; }

        public virtual void OnChatMessage(ChatMessage message) { }
        public virtual void OnPlayerJoin(Player player) { }
        public virtual void OnPlayerLeave(Player player) { }
        public virtual void OnInit() { }


        public static Dictionary<string, object> GetConfigValue(string table)
            => Fishy.Config.GetConfigValue(table);

        public static void SendPacketToAll(FPacket packet)
            => packet.SendPacket("all", (int)CHANNELS.GAME_STATE);

        public static void SendPacketToPlayer(FPacket packet, SteamId player)
            => packet.SendPacket("single", (int)CHANNELS.GAME_STATE, player);

        public static void SpawnActor(string actor)
        {
            switch (actor)
            {
                case "fish_spawn":
                    Spawner.SpawnFish();
                    break;
                case "fish_spawn_alien":
                    Spawner.SpawnFish("fish_spawn_alien");
                    break;
                case "raincloud":
                    Spawner.SpawnRainCloud();
                    break;
                case "metalspot":
                    Spawner.SpawnMetalSpot();
                    break;
                case "void_portal":
                    Spawner.SpawnFish();
                    break;
            }        
        }

        public static void KickPlayer(Player player)
            => Punish.KickPlayer(player);
        public static void BanPlayer(Player player)
            => Punish.BanPlayer(player);
    }
}
