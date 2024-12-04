using Fishy.Models;
using Fishy.Models.Packets;
using Fishy.Utils;
using Steamworks;

namespace Fishy.Chat
{
    public abstract class Command
    {
        public abstract string Name();
        public abstract string[] Aliases();
        public abstract ushort PermissionLevel();
        public abstract string Description();

        public abstract string Help();
        public virtual void OnUse(SteamId player, string[] arguments)
        {

        }
    }

    class CommandHandler
    {
        public static Dictionary<string, Command> Commands = new Dictionary<string, Command>();

        public static void AddCommand(Command cmd)
        {
            Commands.Add(cmd.Name(), cmd);
        }
        public static void Init()
        {
            AddCommand(new Commands.HelpCommand());
            AddCommand(new Commands.RulesCommand());
            AddCommand(new Commands.IssueCommand());
            AddCommand(new Commands.ReportCommand());
            AddCommand(new Commands.SpawnCommand());
            AddCommand(new Commands.CodeOnlyCommand());
            AddCommand(new Commands.BanCommand());
            AddCommand(new Commands.KickCommand());
        }
        public static Player FindPlayer(string name)
        {
            foreach (var player in Fishy.Players)
            {
                if (player.Name == name)
                    return player;
            }
            foreach (var player in Fishy.Players)
            {
                if (player.Name.ToLower() == name.ToLower())
                    return player;
                if (player.Name.ToLower().StartsWith(name.ToLower()))
                    return player;
            }
            return null;
        }
        public static Player FindPlayer(SteamId id)
        {
            foreach (var player in Fishy.Players)
            {
                if (player.SteamID == id )
                    return player;
            }
            return null;
        }
        public static ushort GetPermissionLevel(SteamId player) // Temporary, ideally will have ranks
        {
            if (player == Steamworks.SteamClient.SteamId) // If player is server
                return 100;
            return Fishy.AdminUsers.Contains(player.ToString()) ? (ushort)1 : (ushort)0;
        }

        public static bool OnMessage(SteamId from, string message)
        {
            if (!message.StartsWith("!"))
                return false;
            message = message.Remove(0, 1);
            string commandName = message.Split(" ")[0];
            string[] arguments = message.Split(" ").Skip(1).ToArray();

            if (!Commands.ContainsKey(commandName))
            {
                ChatUtils.SendChat(from, $"Command \"{commandName}\" does not exist!", "bb1111");
                return true;
            }
            Command cmd = Commands[commandName];
            ushort userPermission = GetPermissionLevel(from);
            if (cmd.PermissionLevel() > userPermission)
            {
                ChatUtils.SendChat(from, $"You are not a high enough rank to use \"{commandName}\"!", "bb1111");
                return true;
            }
            ChatLogger.Log(new ChatMessage(default, $"Player {from} executed command {commandName} with arguments {string.Join(", ", arguments)}"));
            try
            {
                cmd.OnUse(from, arguments);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured while running command \"{commandName}\" {ex.ToString()}");
            }
            return true;

        }
    }
}
