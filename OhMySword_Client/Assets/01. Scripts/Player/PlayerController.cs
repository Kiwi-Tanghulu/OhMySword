using Base.Network;
using Packets;
using UnityEngine;
using UnityEngine.Events;

namespace OhMySword.Player
{
    public class PlayerController : SyncableObject, IDamageable, IHitable
    {
        private PlayerMove movement;
        private PlayerWeapon playerWeapon;

        public UnityEvent<SyncableObject> OnHitEvent;
        public UnityEvent<SyncableObject> OnDieEvent;

        public string nickname;

        public override void OnCreated()
        {
            movement = GetComponent<PlayerMove>();
        }

        public override void OnDeleted()
        {
        }

        public void SetNickname(string nickname)
        {
            this.nickname = nickname;   
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

        public void GetXP(ushort amount)
        {
            playerWeapon.SetScore(amount);
        }

        public void Die(SyncableObject attacker)
        {
            
        }

        public void DoChat(string chat)
        {

        }

        public override void SetPosition(Vector3 position, bool immediately = false)
        {
            base.SetPosition(position, immediately);
            movement.SetTargetPosition(targetPosition);
        }
    }
}
