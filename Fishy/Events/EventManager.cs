using Fishy.Models;
using Fishy.Utils;
using Steamworks;

namespace Fishy.Events
{

    public class EventManager
    {
        public static event EventHandler<ActorActionEventArgs> OnActorAction;
        public static event EventHandler<PlayerJoinEventArgs > OnPlayerJoin;
        public static event EventHandler<PlayerLeaveEventArgs> OnPlayerLeave;
        public static event EventHandler<ChatMessageEventArgs> OnChatMessage;
        public class ActorActionEventArgs : EventArgs
        {
            public SteamId SteamId { get; set; }
            public string PacketAction { get; set; }
        }

        public class ChatMessageEventArgs : EventArgs
        {
            public ChatMessage Message { get; set; }

            public SteamId SteamId { get; set; }
        }

        public class PlayerJoinEventArgs : EventArgs
        {
            public Player Player { get; set; }
        }
        public class PlayerLeaveEventArgs : EventArgs
        {
            public Player Player { get; set; }
        }

        internal static void TriggerOnChatMessage( ChatMessage msg)
        {
            OnChatMessage.Invoke(null, new ChatMessageEventArgs() {Message = msg });
        }
        internal static void TriggerOnPlayerJoin(Player player)
        {
            OnPlayerJoin.Invoke(null, new PlayerJoinEventArgs() { Player = player });
        }
        internal static void TriggerOnPlayerLeave(Player player)
        {
            OnPlayerLeave.Invoke(null, new PlayerLeaveEventArgs() { Player = player });
        }

        internal static void TriggerOnActorAction(SteamId steamId, string packetAction)
        {
            OnActorAction.Invoke(null, new ActorActionEventArgs() {SteamId = steamId, PacketAction = packetAction });
        }
    }
}
