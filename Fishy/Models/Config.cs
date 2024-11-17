using Tomlyn;

namespace Fishy.Models
{
    class Config
    {
        public string ServerName { get; set; } = String.Empty;
        public string JoinMessage { get; set; } = String.Empty;
        public string[] Rules { get; set; } = [];
        public string GameVersion { get; set; } = "1.1";
        public int MaxPlayers { get; set; } = 12;
        public bool CodeOnly { get; set; } = false;
        public string LobbyCode { get; set; } = "ABCDEF";
        public bool AgeRestricted { get; set; } = false;
        public string World { get; set; } = "main_zone.tscn";
        public List<string> Admins { get; set; } = [];
        public string ReportFolder { get; set; } = "Reports";
        public string ReportResponse { get; set; } = "";

        public bool LoadConfig(string configPath)
        {
            string cfg = File.ReadAllText(configPath);
            bool valid = Toml.TryToModel(cfg, out Config? manager, out _);
            if (!valid || manager == null)
                return false;

            ServerName = manager.ServerName;
            JoinMessage = manager.JoinMessage;
            Rules = manager.Rules;
            GameVersion = manager.GameVersion;
            CodeOnly = manager.CodeOnly;
            LobbyCode = manager.LobbyCode;
            MaxPlayers = manager.MaxPlayers;
            AgeRestricted = manager.AgeRestricted;
            World = manager.World;
            Admins = manager.Admins;
            ReportFolder = manager.ReportFolder;
            ReportResponse = manager.ReportResponse;
            return true;
        }
    }
}
