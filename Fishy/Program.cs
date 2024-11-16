using Fishy.Models.Packets;
using Fishy.Utils;
using Steamworks;

namespace Fishy
{
    class Program
    {

        static void Main(string[] args)
        {
            Fishy.Init();
        }

        static void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Application is closing...");

            new ServerClosePacket().SendPacket("all", (int)CHANNELS.GAME_STATE);

            Fishy.SteamHandler.Lobby.Leave();
            SteamClient.Shutdown();
        }
    }
}
