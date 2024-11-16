using Fishy.Models;
using Fishy.Utils;

namespace Fishy
{
    class Fishy
    {
        public static Config Config = new();
        public static World World = new();
        public static List<Player> Players = [];
        public static List<Actor> Instances = [];
        public static SteamHandler SteamHandler = new();
        public static NetworkHandler NetworkHandler = new();
        static readonly string configPath = Path.Combine(AppContext.BaseDirectory, "config.toml");

        public static void Init()
        {

            Console.WriteLine("Fishy - Your Dedicated Webfishing Server");
            Console.WriteLine("Starting Server");
            Console.WriteLine("Reading config file...");
            LoadConfig();
            Console.WriteLine("Reading world file...");
            LoadWorld();
            Console.WriteLine("Initializing Steam Client...");
            InitSteam();
            Console.WriteLine("Starting NetworkHandler...");
            NetworkHandler.Start();
            Console.WriteLine("NetworkHandler was started successfully");
            Console.WriteLine("Creating Lobby...");
            SteamHandler.CreateLobby();
            Console.ReadKey();
        }

        static void LoadConfig()
        {

            if (!File.Exists(configPath))
            {
                Console.WriteLine("No config file found. (config.toml) Shutting down...");
                Environment.Exit(1);
            }

            if (!Config.LoadConfig(configPath))
            {
                Console.WriteLine("Error in config file. Shutting down...");
                Environment.Exit(1);
            }
            Console.WriteLine("Config was read successfully");
        }

        static void LoadWorld()
        {
            string worldPath = Path.Combine(AppContext.BaseDirectory, "Worlds", Config.World);
            if (!File.Exists(worldPath))
            {
                Console.WriteLine("No world file found. (main_zone.tscn) Shutting down...");
                Environment.Exit(1);
            }

            if (!World.LoadWorld(worldPath))
            {
                Console.WriteLine("Error in world file. Shutting down...");
                Environment.Exit(1);
            }
            Console.WriteLine("Worldfile was read successfully");
        }
   
        static void InitSteam()
        {
            string error = SteamHandler.Init();
            if (!String.IsNullOrEmpty(error))
            {
                Console.WriteLine("Error Initializing Steam Client. Shutting down...");
                Console.WriteLine("Error: " + error);
                Environment.Exit(1);
            }
            Console.WriteLine("Steam Client was initialized successfully");
        }

    }
}
