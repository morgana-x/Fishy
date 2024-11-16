using Steamworks;

namespace Fishy.Models.Packets
{
    class PingPacket : FPacket
    {
        public override string Type { get; set; } = "request_ping";
        public string Time { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        public string Sender { get; set; } = SteamClient.SteamId.Value.ToString();
    }

    class PongPacket : FPacket
    {
        public override string Type { get; set; } = "send_ping";
        public string Time { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        public string From { get; set; } = SteamClient.SteamId.Value.ToString();

    }
}
