using H00N.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Packets
{
    public class UShortPacket : DataPacket
    {
        public ushort data;

        public UShortPacket() { }

        public UShortPacket(ushort data)
        {
            this.data = data;
        }

        public override ushort Deserialize(ArraySegment<byte> buffer, int offset)
        {
            ushort process = 0;
            process += PacketUtility.ReadUShortData(buffer, offset + process, out data);

            return process;
        }

        public override ushort Serialize(ArraySegment<byte> buffer, int offset)
        {
            ushort process = 0;
            process += PacketUtility.AppendUShortData(this.data, buffer, offset + process);

            return process;
        }
    }
}
