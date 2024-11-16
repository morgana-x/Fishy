using Steamworks;

namespace Fishy.Models.Packets
{
    class ServerClosePacket : FPacket
    {
        public override string Type { get; set; } = "server_close";
    }
}
