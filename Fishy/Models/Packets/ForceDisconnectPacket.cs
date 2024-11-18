using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishy.Models.Packets
{
    class ForceDisconnectPacket (string id) : FPacket
    {
        public override string Type { get; set; } = "force_disconnect_player";
        public string User_ID { get; set; } = id;

    }
}
