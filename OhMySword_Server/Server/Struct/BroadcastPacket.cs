using H00N.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public struct BroadcastPacket
    {
        public Packet packet;
        public ushort except;

        public BroadcastPacket(Packet packet, ushort except)
        {
            this.packet = packet;
            this.except = except;
        }
    }
}
