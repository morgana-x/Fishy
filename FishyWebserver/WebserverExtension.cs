using Fishy.Extensions;
using Fishy.Models;
using Fishy.Utils;

namespace Fishy.Webserver
{
    class WebserverExtension : FishyExtension
    {
        readonly Dashboard dashboard = new();

        public static string GetUsername()
            => GetConfigValue("webserver")["username"].ToString() ?? "";
        public static string GetPassword()
            => GetConfigValue("webserver")["password"].ToString() ?? "";
        public override void OnChatMessage(ChatMessage message)
            => dashboard.MessageToSync.Add(message);
        public override void OnPlayerJoin(Player player)
            => dashboard.PlayersToSync.Add(player, "join");
        public override void OnPlayerLeave(Player player)
            => dashboard.PlayersToSync.Add(player, "leave");
        public override void OnInit()
            => dashboard.Initalize();
    }
}
