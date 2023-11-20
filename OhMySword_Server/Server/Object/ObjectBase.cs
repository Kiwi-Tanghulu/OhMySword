using Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public abstract class ObjectBase
    {
        public GameRoom room;
        public ushort objectID;
        public ushort objectType;
        public Vector3 position;
        public Vector3 rotation;

        public virtual void Hit(ushort damage, ushort attackerID) { }

        public void BroadcastDestroy()
        {
            room?.AddJob(() => {
                Room tempRoom = room;
                if(tempRoom != null)
                {
                    S_ObjectDestroyPacket broadcastPacket = new S_ObjectDestroyPacket(objectID);
                    tempRoom.ReleaseObject(this);
                    tempRoom.Broadcast(broadcastPacket);
                }
            });
        }

        public static implicit operator ObjectPacket(ObjectBase right)
        {
            return new ObjectPacket(right.objectID, right.objectType, right.position, right.rotation);
        }
    }
}
