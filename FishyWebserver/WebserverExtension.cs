using Fishy.Events;
using Fishy.Extensions;

namespace Fishy.Webserver
{
    class WebserverExtension : FishyExtension
    {
        readonly static Dashboard dashboard = new();

        public static string GetUsername()
            => GetConfigValue("webserver")["username"].ToString() ?? "";
        public static string GetPassword()
            => GetConfigValue("webserver")["password"].ToString() ?? "";
        public static void OnChatMessage(object? sender, EventManager.ChatMessageEventArgs evArgs)
            => dashboard.MessageToSync.Add(evArgs.Message);
        public static void OnPlayerJoin(object? sender, EventManager.PlayerJoinEventArgs evArgs)
            => dashboard.PlayersToSync.TryAdd(evArgs.Player, "join");
        public static void OnPlayerLeave(object? sender, EventManager.PlayerLeaveEventArgs evArgs)
            => dashboard.PlayersToSync.TryAdd(evArgs.Player, "leave");
        public override void OnInit()
        { 
            dashboard.Initalize();
            EventManager.OnPlayerJoin += OnPlayerJoin;
            EventManager.OnPlayerLeave += OnPlayerLeave;
            EventManager.OnChatMessage += OnChatMessage;
        }
    }
}
