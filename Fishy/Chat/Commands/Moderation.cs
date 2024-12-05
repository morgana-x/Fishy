using Fishy.Models;
using Fishy.Utils;
using Steamworks;

namespace Fishy.Chat.Commands
{
    internal class KickCommand : Command
    {
        public override string Name() =>"kick";
        public override string Description() =>"Kick a player";
        public override ushort PermissionLevel() => 1;
        public override string[] Aliases() => new string[0];

        public override string Help() => "!kick player";
        public override void OnUse(SteamId executor, string[] arguments)
        {
            if (arguments.Length == 0) { ChatUtils.SendChat(executor, Help()); return; }

            Player? playerToKick = ChatUtils.FindPlayer(arguments[0]);

            if (playerToKick == null)
                return;

            Punish.KickPlayer(playerToKick);

            ChatUtils.SendChat(executor, $"Kicked player {playerToKick.Name} {playerToKick.SteamID}");
        }
    }
    internal class BanCommand : Command
    {
        public override string Name() => "ban";
        public override string Description() =>"Ban a player";
        public override ushort PermissionLevel() => 1;
        public override string[] Aliases() => new string[0];

        public override string Help() => "!ban player";
        public override void OnUse(SteamId executor, string[] arguments)
        {
            if (arguments.Length == 0) { ChatUtils.SendChat(executor, Help()); return; }

            Player? playerToBan = ChatUtils.FindPlayer(arguments[0]);

            if (playerToBan == null)
                return;

            Punish.BanPlayer(playerToBan);

            ChatUtils.SendChat(executor, $"Banned player {playerToBan.Name} {playerToBan.SteamID}");
        }
    }
    internal class SpawnCommand : Command
    {
        public override string Name() => "spawn";
        public override string Description() =>"spawn an entity";
        public override ushort PermissionLevel() => 1;

        public override string[] Aliases() => new string[0];

        public override string Help() => "!spawn type\nAvaiable default types: fish, meteor, raincloud, metalspot, voidportal";
        public override void OnUse(SteamId executor, string[] arguments)
        {
            if (arguments.Length == 0) { ChatUtils.SendChat(executor, Help()); return; }
            var from = executor;

            switch (arguments[0])
            { 
                case "fish":
                    Spawner.SpawnFish();
                    ChatUtils.SendChat(executor, "A fish has been spawned!");
                    return;
                case "meteor":
                    Spawner.SpawnFish("fish_spawn_alien");
                    ChatUtils.SendChat(executor, "A meteor has been spawned!");
                    return;
                case "rain":
                    Spawner.SpawnRainCloud();
                    ChatUtils.SendChat(executor, "A raincloud has been spawned!");
                    return;
                case "metal":
                    Spawner.SpawnMetalSpot();
                    ChatUtils.SendChat(executor, "A metalspot has been spawned!");
                    return;
                case "void_portal":
                    Spawner.SpawnVoidPortal();
                    ChatUtils.SendChat(executor, "A voidportal has been spawned!");
                    return;
            }

            var player = ChatUtils.FindPlayer(executor);
            if (player == null)
                return;
            Spawner.SpawnActor(new Actor(Spawner.GetFreeId(), arguments[0], player.Position));
            ChatUtils.SendChat(executor, $"Spawned \"{arguments[0]}\".");
        }
    }
    internal class CodeOnlyCommand : Command
    {
        public override string Name() => "codeonly";
        public override string Description() =>"sets lobby type";
        public override ushort PermissionLevel() => 1;
        public override string[] Aliases() => new string[0];

        public override string Help() => "!codeonly true/false";
        public override void OnUse(SteamId executor, string[] arguments)
        {
            if (arguments.Length == 0) { ChatUtils.SendChat(executor, Help()); return; }
            var from = executor;
            string type = arguments[0] == "true" ? "code_only" : "public";
            Fishy.SteamHandler.Lobby.SetData("type", type);
            ChatUtils.SendChat(executor, "The lobby type has been set to: " + type);
        }
    }

    internal class ReportCommand : Command
    {
        public override string Name() => "report";
        public override string Description() => "Report a player";

        public override ushort PermissionLevel() => 0;

        public override string[] Aliases() => new string[0];

        public override string Help() => "!report player reason";
        public override void OnUse(SteamId executor, string[] arguments)
        {
            if (arguments.Length < 2) { ChatUtils.SendChat(executor, Help()); return; }

            string reportPath = Path.Combine(AppContext.BaseDirectory, Fishy.Config.ReportFolder, DateTime.Now.ToString("ddMMyyyyHHmmss") + arguments[0] + ".txt");
            string report = "Report for user: " + arguments[0];

            report += "\nReason: " + String.Join(" ", arguments[1..]);
            report += "\nChat Log:\n\n";

            string chatLog = String.Empty;
            Player? player = Fishy.Players.FirstOrDefault(p => p.Name.Equals(arguments[0]));

            if (player != null)
                chatLog = ChatLogger.GetLog(player.SteamID);
            else
                chatLog = ChatLogger.GetLog();

            File.WriteAllText(reportPath, report + ChatLogger.GetLog());
            ChatUtils.SendChat(executor, Fishy.Config.ReportResponse, "b30000");
        }
    }
    internal class IssueCommand : Command
    {
        public override string Name() => "issue";
        public override string Description() =>"Report an issue";
        public override ushort PermissionLevel() => 0;

        public override string Help() => "!issue description";
        public override string[] Aliases() => new string[0];
        public override void OnUse(SteamId executor, string[] arguments)
        {
            if (arguments.Length == 0)  { ChatUtils.SendChat(executor, Help()); return; };

            string issuePath = Path.Combine(AppContext.BaseDirectory, Fishy.Config.ReportFolder, DateTime.Now.ToString("ddMMyyyyHHmmss") + "issueReport.txt");

            string issueReport = "Issue Report\n" + String.Join(" ", arguments[0..]);

            File.WriteAllText(issuePath, issueReport);

            ChatUtils.SendChat(executor, "Your issues has been received and will be looked at as soon as possible.", "b30000");
        }
    }
    internal class SetAdmin : Command
    {
        public override string Name() => "setadmin";
        public override string Description() => "set a player to admin temporally (CONSOLE ONLY)";
        public override ushort PermissionLevel() => 100;

        public override string Help() => "!setadmin name/steamid";
        public override string[] Aliases() => new string[0];
        public override void OnUse(SteamId executor, string[] arguments)
        {
            if (arguments.Length == 0) { ChatUtils.SendChat(executor, Help()); return; };
            Player? player = ChatUtils.FindPlayer(arguments[0]);
            if (player == null)
            {
                ChatUtils.SendChat(executor, $"Couldn't find player\"{arguments[0]}\"!");
                return;
            }
            if (Fishy.Config.Admins.Contains(player.SteamID.ToString()))
            {
                ChatUtils.SendChat(executor, $"{player.Name} ({player.SteamID.ToString()}) is already an admin!", "ffaaaa");
                return;
            }
            Fishy.Config.Admins.Add(player.SteamID.ToString());
            // Todo save config, might require some config rework too :(
            ChatUtils.SendChat(executor, $"Set {player.Name} ({player.SteamID.ToString()}) to admin!", "aaffaa");
            ChatUtils.BroadcastChat($"{player.Name} ({player.SteamID.ToString()}) was promoted to admin!", "aaffaa");
        }
    }
    internal class RevokeAdmin : Command
    {
        public override string Name() => "revokeadmin";
        public override string Description() => "revoke a player's admin access (CONSOLE ONLY)";
        public override ushort PermissionLevel() => 100;

        public override string Help() => "!revokeadmin steamid";
        public override string[] Aliases() => new string[0];
        public override void OnUse(SteamId executor, string[] arguments)
        {
            if (arguments.Length == 0) { ChatUtils.SendChat(executor, Help()); return; };
            Player? player = ChatUtils.FindPlayer(arguments[0]);
            if (player == null)
            {
                ChatUtils.SendChat(executor, $"Couldn't find player\"{arguments[0]}\"!");
                return;
            }
            if (!Fishy.Config.Admins.Contains(player.SteamID.ToString()))
            {
                ChatUtils.SendChat(executor, $"{player.Name} {player.SteamID} is not an admin!", "ffaaaa");
                return;
            }
            Fishy.Config.Admins.Remove(player.SteamID.ToString());
            // Todo save config, might require some config rework too :(
            ChatUtils.SendChat(executor, $"Revoked {player.Name} ({player.SteamID.ToString()})'s admin!", "aaffaa");
            ChatUtils.BroadcastChat($"{player.Name} ({player.SteamID.ToString()}) was demoted from admin!", "ffaaaa");
        }

      
    }
    
}
