using System.Numerics;

namespace Fishy.Models.Packets
{
    public class ActorUpdatePacket(int id, Vector3 pos, Vector3 rot) : FPacket
    {
        public override string Type { get; set; } = "actor_update";
        public int Actor_ID { get; set; } = id;
        public Vector3 Pos { get; set; } = pos;
        public Vector3 Rot { get; set; } = rot;

    }
}
