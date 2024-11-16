using Steamworks;

namespace Fishy.Models.Packets
{
    public class HostPacket : FPacket
    {
        public override string Type { get; set; } = "recieve_host";
        public string Host_ID { get; set; } = SteamClient.SteamId.Value.ToString();

    }
}
