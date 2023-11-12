using Base.Network;
using Packets;
using UnityEngine;
using UnityEngine.Events;

namespace OhMySword.Player
{
    public class PlayerController : SyncableObject, IDamageable, IHitable
    {
        private PlayerMove movement;

        public UnityEvent<SyncableObject> OnHitEvent;
        public UnityEvent OnDieEvent;

        public override void OnCreated()
        {
            movement = GetComponent<PlayerMove>();
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
            OnHitEvent?.Invoke(attacker);
        }

        public void GetXP()
        {

        }

        public void Die()
        {
            OnDieEvent?.Invoke();
        }

        public override void SetPosition(Vector3 position, bool immediately = false)
        {
            base.SetPosition(position, immediately);
            movement.SetTargetPosition(targetPosition);
        }
    }
}
