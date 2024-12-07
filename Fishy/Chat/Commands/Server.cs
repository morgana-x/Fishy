using Fishy.Models.Packets;
using Fishy.Utils;
using Steamworks;

namespace Fishy.Chat.Commands
{
    internal class CodeOnlyCommand : Command
    {
        public override string Name => "codeonly";
        public override string Description => "sets lobby type";
        public override PermissionLevel PermissionLevel => PermissionLevel.Admin;
        public override string[] Aliases => [];
        public override string Help => "!codeonly true/false";

        public override void OnUse(SteamId executor, string[] arguments)
        {
            if (arguments.Length == 0) { ChatUtils.SendChat(executor, Help); return; }

            string type = arguments[0] == "true" ? "code_only" : "public";

            Fishy.SteamHandler.Lobby.SetData("type", type);

            ChatUtils.SendChat(executor, "The lobby type has been set to: " + type);
        }
    }

    internal class StopCommand : Command
    {
        public override string Name => "stop";
        public override string Description => "Shuts down the server";
        public override PermissionLevel PermissionLevel => PermissionLevel.Admin;
        public override string[] Aliases => ["halt"];
        public override string Help => "!stop";

        public override void OnUse(SteamId executor, string[] arguments)
        {
            Console.WriteLine("Server was halted by " + executor);

            new ServerClosePacket().SendPacket("all", (int)CHANNELS.GAME_STATE);

            Fishy.SteamHandler.Lobby.Leave();
            Environment.Exit(0);
        }
    }
}
