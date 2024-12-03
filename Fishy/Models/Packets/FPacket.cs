using Fishy.Helper;
using Fishy.Utils;
using Steamworks;
using System.Reflection;

namespace Fishy.Models.Packets
{
    public class FPacket
    {
        public virtual string Type { get; set; } = "";
        public virtual Dictionary<string, object> ToDictionary()
        {
            return GetType()
               .GetProperties(BindingFlags.Instance | BindingFlags.Public)
               .ToDictionary(p => p.Name.ToLower(), p => p.GetValue(this, null) ?? "");
        }

        public static Dictionary<string, object> FromBytes(byte[] packet)
            => (new GodotUtilReader(packet)).ReadPacket();

        public static byte[] ToBytes(FPacket packet)
           => GZip.Compress(new GodotUtilWriter().ConvertToGodotBytePacket(packet.ToDictionary()));

        public virtual void SendPacket(string target = "all", int channel = 0, SteamId steamId = default)
        {
            GodotUtilWriter writer = new();
            var data = GZip.Compress(writer.ConvertToGodotBytePacket(this.ToDictionary()));

            if (target == "all")
            {
                foreach (Friend member in Fishy.SteamHandler.Lobby.Members)
                {
                    if (member.Id != SteamClient.SteamId.Value)
                        SteamNetworking.SendP2PPacket(member.Id, data, nChannel: channel);
                }                
            }
            else
            {
                SteamNetworking.SendP2PPacket(steamId, data, nChannel: channel);
            }
        }   
    }
}
