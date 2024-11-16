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
        public static List<string> PublicCommands = ["report"];
        public static List<string> AdminCommands = ["spawn", "kick", "visible", "codeonly"];

        public static void OnMessage(SteamId from, string command)
        {
            string[] commandParams = command.Remove(0, 1).Split(" ");

            if (!PublicCommands.Contains(commandParams[0]) && !Fishy.Config.Admins.Contains(from.ToString()))
            {
                new MessagePacket("Invalid command or no permission.", "b30000").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                return;
            }

            if (commandParams.Length < 2) return;

            switch (commandParams[0])
            {
                case "kick":
                    Player? p = Fishy.Players.Find(p => p.Name.Equals(commandParams[1]));
                    if (p != null)
                        new KickPacket().SendPacket("single", (int)CHANNELS.GAME_STATE, p.SteamID);
                    break;
                case "spawn":
                    switch (commandParams[1])
                    {
                        case "fish":
                            Spawner.SpawnFish();
                            break;
                        case "fish_alien":
                            Spawner.SpawnFish("fish_spawn_alien");
                            break;
                        case "rain":
                            Spawner.SpawnRainCloud();
                            break;
                        case "metal":
                            Spawner.SpawnMetalSpot();
                            break;
                        case "void_portal":
                            Spawner.SpawnVoidPortal();
                            break;
                    }
                    break;
                case "visible":
                    Fishy.SteamHandler.Lobby.SetJoinable(commandParams[1] == "true");
                    break;
                case "codeonly":
                    Fishy.SteamHandler.Lobby.SetData("type", commandParams[1] == "true" ? "code_only" : "public");
                    break;
                case "report":
                    string reportPath = Path.Combine(AppContext.BaseDirectory, Fishy.Config.ReportFolder, DateTime.Now.ToString("ddMMyyyyHHmmss") + commandParams[1] + ".txt");
                    string report = "Report for user: " + commandParams[1];
                    report += "\nReason: " + commandParams[2];
                    report += "\nChat Log:\n\n";
                    File.WriteAllText(reportPath, report + ChatLogger.GetLog());
                    new MessagePacket(Fishy.Config.ReportResponse, "b30000").SendPacket("single", (int)CHANNELS.GAME_STATE, from);
                    break;

            }
        }
    }
}
