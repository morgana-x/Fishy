using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fishy.Models;
using Fishy.Models.Packets;
using Steamworks;

namespace Fishy.Utils
{
    class CommandHandler
    {
        public static List<string> PublicCommands = ["help", "rules", "report", "issue"];
        public static List<string> AdminCommands = ["spawn", "kick", "ban", "visible", "codeonly", "issue"];

        public static void OnMessage(SteamId from, string message)
        {

            string[] parameters = message.Remove(0, 1).Split(" ");


            if ((!PublicCommands.Contains(parameters[0]) && !Fishy.Config.Admins.Contains(from.Value.ToString())))
            {
                new MessagePacket("Invalid command or no permission.", "b30000").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                return;
            }

            switch (parameters[0])
            {
                case "kick":
                    if (parameters.Length < 2) return;
                    Player? playerToKick = Fishy.Players.Find(p => p.Name.Equals(parameters[1])) 
                        ?? Fishy.Players.Find(p => p.SteamID.Value.ToString().Equals(parameters[1]));
                    if (playerToKick != null)
                        new KickPacket().SendPacket("single", (int)CHANNELS.GAME_STATE, playerToKick.SteamID);
                    break;
                case "ban":
                    if (parameters.Length < 2) return;
                    Player? playerToBan = Fishy.Players.Find(p => p.Name.Equals(parameters[1]))
                        ?? Fishy.Players.Find(p => p.SteamID.Value.ToString().Equals(parameters[1]));
                    if (playerToBan != null)
                    {
                        new BanPacket().SendPacket("single", (int)CHANNELS.GAME_STATE, playerToBan.SteamID);
                        new ForceDisconnectPacket(playerToBan.SteamID.Value.ToString()).SendPacket("all", (int)CHANNELS.GAME_STATE);
                        SteamHandler.UpdateSteamBanList(playerToBan.SteamID.Value.ToString(), Fishy.SteamHandler.Lobby);
                        using StreamWriter writer = new(Path.Combine(AppContext.BaseDirectory, "bans.txt"), true);
                        writer.WriteLine(playerToBan.SteamID.Value.ToString());
                        Fishy.BannedUsers.Add(playerToBan.SteamID.Value.ToString());
                    }
                    break;
                case "spawn":
                    if (parameters.Length < 2) return;
                    switch (parameters[1])
                    {
                        case "fish":
                            Spawner.SpawnFish();
                            new MessagePacket("A fish has been spawned!").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                            break;
                        case "meteor":
                            Spawner.SpawnFish("fish_spawn_alien");
                            new MessagePacket("A meteor has been spawned").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                            break;
                        case "rain":
                            Spawner.SpawnRainCloud();
                            new MessagePacket("A raincloud has been spawned!").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                            break;
                        case "metal":
                            Spawner.SpawnMetalSpot();
                            new MessagePacket("A metalspot has been spawned!").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                            break;
                        case "void_portal":
                            Spawner.SpawnVoidPortal();
                            new MessagePacket("A voidportal has been spawned!").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                            break;
                    }
                    break;
                case "codeonly":
                    if (parameters.Length < 2) return;
                    string type = parameters[1] == "true" ? "code_only" : "public";
                    Fishy.SteamHandler.Lobby.SetData("type", type);
                    new MessagePacket("The lobby type has been set to: " + type).SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                    break;
                case "help":
                    new MessagePacket("This is a Fishy dedicated server - https://github.com/ncrypted-dev/Fishy").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                    new MessagePacket("The following commands are available: ").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                    new MessagePacket("!help - Displays this info text").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                    new MessagePacket("!rules - Displays the rules").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                    new MessagePacket("!report PlayerName reason - Report a player").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                    new MessagePacket("!issue Description - Report an issue on the server").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                    break;
                case "rules":
                    foreach(string s in Fishy.Config.Rules)
                        new MessagePacket(s).SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                    break;
                case "report":
                    if (parameters.Length < 3) return;
                    string reportPath = Path.Combine(AppContext.BaseDirectory, Fishy.Config.ReportFolder, DateTime.Now.ToString("ddMMyyyyHHmmss") + parameters[1] + ".txt");
                    string report = "Report for user: " + parameters[1];
                    report += "\nReason: " + String.Join(" ",parameters[2..]);
                    report += "\nChat Log:\n\n";

                    string chatLog = String.Empty;
                    Player ? player = Fishy.Players.FirstOrDefault(p => p.Name.Equals(parameters[1]));

                    if (player != null)
                        chatLog = ChatLogger.GetLog(player.SteamID);
                    else
                        chatLog = ChatLogger.GetLog();

                    File.WriteAllText(reportPath, report + ChatLogger.GetLog());
                    new MessagePacket(Fishy.Config.ReportResponse, "b30000").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                    break;
                case "issue":
                    if (parameters.Length < 2) return;
                    string issuePath = Path.Combine(AppContext.BaseDirectory, Fishy.Config.ReportFolder, DateTime.Now.ToString("ddMMyyyyHHmmss") + "issueReport.txt");
                    string issueReport = "Issue Report\n" + String.Join(" ", parameters[1..]);
                    File.WriteAllText(issuePath, issueReport);
                    new MessagePacket("Your issues has been received and will be looked at as soon as possible.", "b30000").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                    break;

            }
        }
    }
}
