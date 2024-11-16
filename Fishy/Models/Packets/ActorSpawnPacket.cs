using Steamworks;
using System.Numerics;

namespace Fishy.Models.Packets
{
    public class ActorSpawnPacket : FPacket
    {
        public override string Type { get; set; } = "instance_actor";
        public Params Params { get; set; } = new Params();

        public ActorSpawnPacket(string type, Vector3 pos, int id)
        {
            this.Params = new Params() {  Actor_Type = type, At = pos, Actor_ID = id};
        }

        public override Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> data = Params.GetType()
               .GetProperties()
               .ToDictionary(p => p.Name.ToLower(), p => p.GetValue(this.Params) ?? "");

            Dictionary<string, object> spawnPacket = GetType()
               .GetProperties()
               .ToDictionary(p => p.Name.ToLower(), p => p.GetValue(this) ?? "");

            spawnPacket["params"] = data;
            return spawnPacket;
        }
    }

    public class Params
    {
        public string Actor_Type { get; set; } = "player";
        public Vector3 At { get; set; } = Vector3.Zero;
        public Vector3 Rot { get; set; } = Vector3.Zero;
        public string Zone { get; set; } = "main_zone";
        public int Zone_Owner { get; set; } = -1;
        public int Actor_ID { get; set; } = 255;
        public long Creator_ID { get; set; } = (long)SteamClient.SteamId.Value;
    }
}
