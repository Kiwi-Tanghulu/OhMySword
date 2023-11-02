
namespace Server
{
    public class Player : ObjectBase
    {
        public string name;
        public ushort score;
        public ushort hp;

        public Player(ClientSession session, string name)
        {
            this.session = session;
            this.name = name;
            this.hp = 1;
        }
    }
}
