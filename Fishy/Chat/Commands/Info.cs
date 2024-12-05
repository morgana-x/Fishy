using Fishy.Utils;
using Steamworks;
namespace Fishy.Chat.Commands
{
    internal class HelpCommand : Command
    {
        public override string Name => "help";
        public override string Description => "Displays this info text";
        public override PermissionLevel PermissionLevel => PermissionLevel.Player;
        public override string[] Aliases => [];
        public override string Help => "";

        public override void OnUse(SteamId executor, string[] arguments)
        {
            ChatUtils.SendChat(executor, "The following commands are available: ", "aaffa1");
            foreach (var item in CommandHandler.Commands)
            {
                if ((int)item.Value.PermissionLevel > CommandHandler.GetPermissionLevel(executor))
                    continue;
                ChatUtils.SendChat(executor, $"!{item.Key} - {item.Value.Description}");
            }
        }
    }

    internal class RulesCommand : Command
    {
        public override string Name => "rules";
        public override string Description => "Displays the rules";
        public override PermissionLevel PermissionLevel => PermissionLevel.Player;
        public override string[] Aliases => [];
        public override string Help => "";

        public override void OnUse(SteamId executor, string[] arguments)
        {
            ChatUtils.SendChat(executor, $"Rules:", "aabbff");
            for (int i = 0; i < Fishy.Config.Rules.Length; i++)
            {
                ChatUtils.SendChat(executor, $"{i+1}. {Fishy.Config.Rules[i]}");
            }
        }
    }

}
