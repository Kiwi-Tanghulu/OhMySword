using Base.Network;
using Packets;
using UnityEngine;
using UnityEngine.Events;

namespace OhMySword.Player
{
    public class PlayerController : SyncableObject, IDamageable, IHitable
    {
        private PlayerMove movement;
        private PlayerView view;
        private PlayerWeapon playerWeapon;

        public UnityEvent<SyncableObject> OnHitEvent;
        public UnityEvent OnDieEvent;

        public string nickname;

        public override void OnCreated()
        {
            movement = GetComponent<PlayerMove>();
            view = GetComponent<PlayerView>();
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
            OnHitEvent?.Invoke(attacker);
        }

        public void GetXP(ushort amount)
        {
            playerWeapon.SetScore(amount);
        }

        public void Die(SyncableObject attacker, ushort destroyCount)
        {
            OnDieEvent?.Invoke();
        }

        public void DoChat(string chat)
        {

        }

        public override void SetPosition(Vector3 position, bool immediately = false)
        {
            base.SetPosition(position, immediately);
            Debug.Log($"SetPosition : {position}");
            movement?.SyncMove(targetPosition);
        }

        public override void SetRotation(Vector3 rotation)
        {
            // 여기에 로테이션
            view?.SetRotation(rotation);
        }
    }
}
