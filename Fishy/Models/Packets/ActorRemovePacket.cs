namespace Fishy.Models.Packets
{
    public class ActorRemovePacket(int id) : FPacket
    {
        public override string Type { get; set; } = "actor_action";
        public int Actor_ID { get; set; } = id;
        public string Action { get; set; } = "queue_free";
        public Dictionary<int, object> Params { get; set; } = [];

        public override Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> spawnPacket = GetType()
               .GetProperties()
               .ToDictionary(p => p.Name.ToLower(), p => p.GetValue(this) ?? "");

            spawnPacket["params"] = new Dictionary<int, object>();
            return spawnPacket;
        }
    }
}
