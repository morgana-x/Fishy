using Fishy.Models;
using Fishy.Models.Packets;
using Fishy.Utils;
using Steamworks;

namespace Fishy.Chat.Commands
{
    internal class KickCommand : Command
    {
        public string Name = "kick";
        public string Description = "Kick a player";
        public ushort PermissionLevel = 1;
        public override void OnUse(SteamId executor, string[] arguments)
        {
            if (arguments.Length == 0) return;

            Player? playerToKick = CommandHandler.FindPlayer(arguments[0]);

            if (playerToKick == null)
                return;

            Punish.KickPlayer(playerToKick);

            ChatUtils.SendChat(executor, $"Kicked player {playerToKick.Name} {playerToKick.SteamID}");
        }
    }
    internal class BanCommand : Command
    {
        public string Name = "ban";
        public string Description = "Ban a player";
        public ushort PermissionLevel = 1;
        public override void OnUse(SteamId executor, string[] arguments)
        {
            if (arguments.Length == 0) return;

            Player? playerToBan = CommandHandler.FindPlayer(arguments[0]);

            if (playerToBan == null)
                return;

            Punish.BanPlayer(playerToBan);

            ChatUtils.SendChat(executor, $"Banned player {playerToBan.Name} {playerToBan.SteamID}");
        }
    }
    public class SpawnCommand : Command
    {
        public string Name = "spawn";
        public string Description = "spawn an entity";
        public ushort PermissionLevel = 1;
        public override void OnUse(SteamId executor, string[] arguments)
        {
            if (arguments.Length == 0) return;
            var from = executor;
            switch (arguments[0])
            { 
            case "fish":
                Spawner.SpawnFish();
                new MessagePacket("A fish has been spawned!").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                    return;
            case "meteor":
                Spawner.SpawnFish("fish_spawn_alien");
                new MessagePacket("A meteor has been spawned").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                    return;
            case "rain":
                Spawner.SpawnRainCloud();
                new MessagePacket("A raincloud has been spawned!").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                    return;
            case "metal":
                Spawner.SpawnMetalSpot();
                new MessagePacket("A metalspot has been spawned!").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                    return;
            case "void_portal":
                Spawner.SpawnVoidPortal();
                new MessagePacket("A voidportal has been spawned!").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                return;
            }
            var player = CommandHandler.FindPlayer(executor);
            if (player == null)
                return;
            Spawner.SpawnActor(new Actor(Spawner.GetFreeId(), arguments[0], player.Position));
        }
    }
    public class CodeOnlyCommand : Command
    {
        public string Name = "codeonly";
        public string Description = "sets lobby type";
        public ushort PermissionLevel = 1;
        public override void OnUse(SteamId executor, string[] arguments)
        {
            if (arguments.Length == 0) return;
            var from = executor;
            string type = arguments[0] == "true" ? "code_only" : "public";
            Fishy.SteamHandler.Lobby.SetData("type", type);
            new MessagePacket("The lobby type has been set to: " + type).SendPacket("single", (int)CHANNELS.GAME_STATE, from);
        }
    }

    internal class ReportCommand : Command
    {
        public string Name = "report";
        public string Description = "Report a player";
        public override void OnUse(SteamId executor, string[] arguments)
        {
            if (arguments.Length < 2) return;
            var from = executor;
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
            new MessagePacket(Fishy.Config.ReportResponse, "b30000").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
        }
    }
    internal class IssueCommand : Command
    {
        public string Name = "issue";
        public string Description = "Report an issue";
        public override void OnUse(SteamId executor, string[] arguments)
        {
            if (arguments.Length < 2) return;
            var from = executor;
            string issuePath = Path.Combine(AppContext.BaseDirectory, Fishy.Config.ReportFolder, DateTime.Now.ToString("ddMMyyyyHHmmss") + "issueReport.txt");
            string issueReport = "Issue Report\n" + String.Join(" ", arguments[1..]);
            File.WriteAllText(issuePath, issueReport);
            new MessagePacket("Your issues has been received and will be looked at as soon as possible.", "b30000").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
            
        }
    }

}
