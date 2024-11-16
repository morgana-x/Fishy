using Steamworks;

namespace Fishy.Models.Packets
{
    public class HandshakePacket : FPacket
    {
        public override string Type { get; set; } = "handshake";
        public string User_ID { get; set; } = SteamClient.SteamId.Value.ToString();

    }
}
