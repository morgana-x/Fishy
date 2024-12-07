using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fishy.Models;
using Fishy.Models.Packets;
using GenHTTP.Api.Protocol;
using GenHTTP.Modules.Controllers;
using GenHTTP.Modules.Reflection;


namespace Fishy.Webserver
{
    internal class ActionController
    {
        [ControllerAction(RequestMethod.Post)]
        public void Spawn([FromBody] string actor_type)
        {
            if (Enum.TryParse<ActorType>(actor_type, true, out ActorType actorType))
                WebserverExtension.SpawnActor(actorType);
        }

        [ControllerAction(RequestMethod.Post)]
        public void Chat([FromBody] string message)
            => WebserverExtension.SendPacketToAll(new MessagePacket("Server: " + message));

        [ControllerAction(RequestMethod.Post)]
        public void Kick([FromBody] string accountId)
        {
            Player? p = WebserverExtension.Players.First(p => p.SteamID.AccountId.ToString() == accountId);
            if (p != null)
                WebserverExtension.KickPlayer(p);
        }

        [ControllerAction(RequestMethod.Post)]
        public void Ban([FromBody] string accountId)
        {
            Player? p = WebserverExtension.Players.First(p => p.SteamID.AccountId.ToString() == accountId);
            if (p != null)
                WebserverExtension.BanPlayer(p);
        }

    }
}
