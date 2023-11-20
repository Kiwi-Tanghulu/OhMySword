﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class XPObject : ObjectBase
    {
        public ushort xp;

        public XPObject(GameRoom room, ushort xp)
        {
            this.room = room;
            this.xp = xp;
            this.objectType = (ushort)ObjectType.XPObject;
        }

        public override void Hit(ushort damage, ushort attackerID)
        {
            if (room.GetPlayer(attackerID, out Player player) == false)
                return;

            player.AddXP(xp);
            BroadcastDestroy();
        }

        public void ReleaseSelf(float delay)
        {
            Delay(delay, () => {
                if (this.room != null)
                    BroadcastDestroy();
            });
        }
    }
}
