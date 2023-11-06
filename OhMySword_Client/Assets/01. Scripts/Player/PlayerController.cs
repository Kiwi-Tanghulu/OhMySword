using Base.Network;
using Packets;
using UnityEngine;

namespace OhMySword.Player
{
    public class PlayerController : SyncableObject, IDamageable, IHitable
    {
        public override void OnCreated()
        {
            
        }

        public override void OnDeleted()
        {
        }

        public void SetNickname(string nickname)
        {
            
        }

        public void OnDamage(int damage, GameObject performer, Vector3 point)
        {
            SyncableObject attacker = performer.GetComponent<SyncableObject>();
            if(attacker == null)
                return;

            C_AttackPacket attackPacket = new C_AttackPacket((ushort)ObjectType.Player, ObjectID, attacker.ObjectID, (ushort)damage);
            NetworkManager.Instance.Send(attackPacket);
        }

        public void Hit(SyncableObject attacker)
        {
            
        }

        public void GetXP()
        {

        }

        public void Die()
        {
            
        }
    }
}
