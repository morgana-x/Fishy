namespace Fishy.Models.Packets
{
    class ActorRequestPacket : FPacket
    {
        public override string Type { get; set; } = "actor_request_send";
        public ListPacket List {get; set; } = new();

        public override Dictionary<string, object> ToDictionary()
        {

            Dictionary<string, object> spawnPacket = GetType()
               .GetProperties()
               .ToDictionary(p => p.Name.ToLower(), p => p.GetValue(this) ?? "");

            spawnPacket["list"] = new Dictionary<int, object>();
            return spawnPacket;
        }
    }


    class ListPacket() {}
 }
