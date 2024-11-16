using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishy.Models.Packets
{
    class KickPacket : FPacket
    {
        public override string Type { get; set; } = "kick";
    }
}
