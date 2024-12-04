using Fishy.Models;
using Fishy.Models.Packets;
using Fishy.Utils;
using Steamworks;
using Steamworks.Ugc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishy.Chat.Commands
{
    internal class HelpCommand : Command
    {
        public string Name = "help";
        public string Description = "Displays this info text";
        public override void OnUse(SteamId executor, string[] arguments)
        {
            ChatUtils.SendChat(executor, "The following commands are available: ", "aaffa1");
            foreach (var item in CommandHandler.Commands)
            {
                if (item.Value.PermissionLevel > CommandHandler.GetPermissionLevel(executor))
                    continue;
                ChatUtils.SendChat(executor, $"!{item.Key} - {item.Value.Description}");
            }
        }
    }
    internal class RulesCommand : Command
    {
        public string Name = "rules";
        public string Description = "Displays the rules";
        public override void OnUse(SteamId executor, string[] arguments)
        {
            ChatUtils.SendChat(executor, $"Rules:", "aabbff");
            for (int i = 0; i < Fishy.Config.Rules.Length; i++)
            {
                ChatUtils.SendChat(executor, $"{i}. {Fishy.Config.Rules[i]}");
            }
        }
    }

}
