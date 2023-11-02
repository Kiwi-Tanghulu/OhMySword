﻿
using Packets;

namespace Server
{
    public class Player : ObjectBase
    {
        public string nickname;
        public ushort score;
        public ushort hp;

        public Player(ClientSession session, string name)
        {
            this.session = session;
            this.nickname = name;
            this.hp = 1;
        }

        public static implicit operator PlayerPacket(Player right)
        {
            return new PlayerPacket(right.objectID, right.score, right.nickname, right.position, right.rotation);
        }
    }
}
