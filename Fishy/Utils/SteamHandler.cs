using Fishy.Models;
using Steamworks;
using Steamworks.Data;

namespace Fishy.Utils
{
    class SteamHandler
    {
        public Lobby Lobby { get; set; }

        public SteamHandler()
        {
            SteamMatchmaking.OnLobbyCreated += SteamMatchmaking_OnLobbyCreated;
            SteamMatchmaking.OnLobbyMemberJoined += SteamMatchmaking_OnLobbyMemberJoined;
            SteamMatchmaking.OnLobbyMemberLeave += SteamMatchmaking_OnLobbyMemberLeave;
            SteamNetworking.OnP2PSessionRequest += SteamMatchmaking_OnP2PSessionRequest;
        }

        public static string Init()
        {
            try
            {
                SteamClient.Init(3146520, false);
                return "";
            }
            catch (Exception e) { return e.Message; }
        }

        public static void CreateLobby()
        {
            SteamMatchmaking.CreateLobbyAsync(Fishy.Config.MaxPlayers);
        }


        void SteamMatchmaking_OnLobbyCreated(Result result, Lobby Lobby)
        {
            Lobby.SetJoinable(true); 
            Lobby.SetData("ref", "webfishing_gamelobby");
            Lobby.SetData("name", SteamClient.Name);
            Lobby.SetData("version", Fishy.Config.GameVersion);
            Lobby.SetData("code", Fishy.Config.LobbyCode);
            Lobby.SetData("type", Fishy.Config.CodeOnly ? "code_only" : "public");
            Lobby.SetData("public", "true");
            Lobby.SetData("banned_players", "");
            Lobby.SetData("age_limit", Fishy.Config.AgeRestricted.ToString().ToLower());
            Lobby.SetData("cap", Fishy.Config.MaxPlayers.ToString());
            Lobby.SetData("lurefilter", "dedicated");

            SteamNetworking.AllowP2PPacketRelay(true);

            Lobby.SetData("server_browser_value", "0");

            Console.WriteLine("Lobby Created!");
            Console.WriteLine($"Lobby Code: {Lobby.GetData("code")}");

            this.Lobby = Lobby;
            UpdatePlayerCount();
        }

        void SteamMatchmaking_OnLobbyMemberJoined(Lobby Lobby, Friend userJoining)
        {
            UpdatePlayerCount();
            Player player = new(userJoining.Id, userJoining.Name);
            Console.WriteLine($"A Player Joined: {userJoining.Name}");
            Fishy.Players.Add(player);
        }

        void SteamMatchmaking_OnLobbyMemberLeave(Lobby Lobby, Friend userLeaving)
        {
            UpdatePlayerCount();
            Fishy.Players.RemoveAll(player => player.SteamID.Equals(userLeaving.Id));
        }

        void SteamMatchmaking_OnP2PSessionRequest(SteamId id)
        {
            if (Lobby.Members.Any(player => player.Id.Value.Equals(id)))
                SteamNetworking.AcceptP2PSessionWithUser(id);
        }


        private static void UpdatePlayerCount()
        {
            //TODO: implement
        }
    }
}
