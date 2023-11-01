
namespace Server
{
    public class Player : ObjectBase
    {
        public ushort hp;

        public Player(ushort playerID, ClientSession session)
        {
            this.objectID = playerID;
            this.session = session;
            this.hp = 1;
        }
    }
}
