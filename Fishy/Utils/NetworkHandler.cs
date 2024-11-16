using Fishy.Helper;
using Fishy.Models;
using Fishy.Models.Packets;
using Steamworks;
using Steamworks.Data;
using System.Numerics;

namespace Fishy.Utils
{
    enum CHANNELS
    {
        ACTOR_UPDATE,
        ACTOR_ACTION,
        GAME_STATE,
        CHALK,
        GUITAR,
        ACTOR_ANIMATION,
        SPEECH,
    }

    class NetworkHandler
    {
        public static Dictionary<int, Vector3> PreviousPositions = [];

        public void Start()
        {
            Thread cbThread = new(RunSteamworksUpdate)
            {
                IsBackground = true
            };
            cbThread.Start();

            Thread thread = new(Listen)
            {
                IsBackground = true
            };
            thread.Start();

            static void requestPlayerPings() => new PingPacket().SendPacket("all", (int)CHANNELS.GAME_STATE);
            ScheduledTask pingTask = new(requestPlayerPings, 5000);
            pingTask.Start();

            ScheduledTask spawnTask = new(Spawner.Spawn, 10000);
            spawnTask.Start();

            ScheduledTask updateTask = new(Update, 3000);
            updateTask.Start();
        }

        void Listen()
        {
            while (true)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (!SteamNetworking.IsP2PPacketAvailable(i))
                        continue;

                    P2Packet? packet = SteamNetworking.ReadP2PPacket(i);

                    if (packet != null)
                        OnPacketReceived(packet.Value);
                }
            }
        }

        void RunSteamworksUpdate()
        {
            while (true)
                SteamClient.RunCallbacks();
        }

        public static void OnPacketReceived(P2Packet packet)
        {
            byte[] packetData = GZip.Decompress(packet.Data);
            Dictionary<string, object> packetInfo = FPacket.FromBytes(packetData);
            if (!packetInfo.TryGetValue("type", out object? value))
                return;
            string packetType = (string)value;


            switch (packetType)
            {
                case "handshake":
                    new HandshakePacket().SendPacket("single", (int)CHANNELS.GAME_STATE, packet.SteamId);
                    break;
                case "request_ping":
                    new PongPacket().SendPacket("single", (int)CHANNELS.ACTOR_ACTION, packet.SteamId);
                    break;
                case "new_player_join":
                    new MessagePacket("Welcome to the server!").SendPacket("single", (int)CHANNELS.GAME_STATE, packet.SteamId);
                    new MessagePacket("This is a dedicated game lobby!").SendPacket("single", (int)CHANNELS.GAME_STATE, packet.SteamId);
                    new HostPacket().SendPacket("all", (int)CHANNELS.GAME_STATE);
                    break;
                case "instance_actor":
                    Dictionary<string, object> parameters = (Dictionary<string, object>)packetInfo["params"];
                    if (parameters["actor_type"].ToString() == "player")
                    {
                        int index = Fishy.Players.FindIndex(p => p.SteamID.Equals(packet.SteamId));
                        if (index == -1)
                            break;
                        Fishy.Players[index].InstanceID = (long)parameters["actor_id"];
                    }
                    break;
                case "actor_update":
                    int playerIndex = Fishy.Players.FindIndex(p => p.InstanceID.Equals(packetInfo["actor_id"]));
                    if (playerIndex == -1)
                        break;
                    Fishy.Players[playerIndex].Position = (Vector3)packetInfo["pos"];
                    break;
                
                case "actor_action":
                    string packetAction = (string)packetInfo["action"];
                    if (packetAction == "_sync_create_bubble")
                    {
                        string Message = (string)((Dictionary<int, object>)packetInfo["params"])[0];
                        OnChat(Message, packet.SteamId);
                    }
                    if ((string)packetInfo["action"] == "_wipe_actor")
                    {
                        long actorToWipe = (long)((Dictionary<int, object>)packetInfo["params"])[0];
                        Actor serverInst = Fishy.Instances.First(i => i.InstanceID == actorToWipe);
                        if (serverInst != null)
                        {
                            RemoveServerActor(serverInst);
                        }
                    }
                    break;
                case "request_actors":
                    List<Actor> instances = Fishy.Instances;
                    foreach (Actor actor in instances)
                    {
                        new ActorSpawnPacket(actor.Type, actor.Position, actor.InstanceID).SendPacket("single", (int)CHANNELS.GAME_STATE, packet.SteamId);
                    }

                    new ActorRequestPacket().SendPacket("single", (int)CHANNELS.GAME_STATE, packet.SteamId);
                    break;
                case "letter_recieved":
                    Dictionary<string, object> data = (Dictionary<string, object>)packetInfo["data"];
                    string body = data["body"].ToString() ?? "";
                    CommandHandler.OnMessage(packet.SteamId, body);
                    break;
                default: break;

            }
        }

        static void RemoveServerActor(Actor instance)
        {
            new ActorRemovePacket(instance.InstanceID).SendPacket("all", (int)CHANNELS.GAME_STATE);
            Fishy.Instances.Remove(instance);
        }



        static void OnChat(string message, SteamId id)
        {
            ChatLogger.Log(new ChatMessage(id, message));
            Player player = Fishy.Players.First(player => player.SteamID.Equals(id)) ?? new Player(0, "");
            if (player.Name == "" || !message.StartsWith('!')) return;
            CommandHandler.OnMessage(id, message);
        }


        public static void Update()
        {
            foreach(Actor instance in Fishy.Instances.ToList())
            {
                if (instance is RainCloud)
                    instance.OnUpdate();

                if (!PreviousPositions.ContainsKey(instance.InstanceID))
                    PreviousPositions[instance.InstanceID] = Vector3.Zero;

                if (instance.Position != PreviousPositions[instance.InstanceID])
                {
                    PreviousPositions[instance.InstanceID] = instance.Position;
                    new ActorUpdatePacket(instance.InstanceID, instance.Position, instance.Rotation).SendPacket("all", (int)CHANNELS.GAME_STATE);
                }
            }
        }
    }
}
