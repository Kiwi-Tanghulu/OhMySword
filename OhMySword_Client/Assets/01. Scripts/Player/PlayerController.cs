using System;
using Base.Network;
using Packets;
using UnityEngine;
using UnityEngine.Events;

namespace OhMySword.Player
{
    public class PlayerController : SyncableObject, IDamageable, IHitable, IComparable<PlayerController>
    {
        private AudioSource audioPlayer;

        private PlayerMove movement;
        private PlayerView view;
        private PlayerWeapon playerWeapon;

        public UnityEvent<SyncableObject> OnHitEvent;
        public UnityEvent<SyncableObject> OnDieEvent;

        public UnityEvent<int> SetAnimation;

        public TMPro.TextMeshPro nameTag;

        public string nickname;
        public ushort Score => playerWeapon.GetScore();

        protected override void Awake()
        {
            base.Awake();
        }

        public override void OnCreated()
        {
            movement = GetComponent<PlayerMove>();
            view = GetComponent<PlayerView>();
            playerWeapon = transform.Find("Hips/Rig/Sword/Sword").GetComponent<PlayerWeapon>();

            audioPlayer = GetComponent<AudioSource>();
        }

        public override void OnDeleted()
        {
        }

        public void SetNickname(string nickname)
        {
            this.nickname = nickname;  
            nameTag.text = nickname;
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
            Debug.Log($"Hit: {transform.name}");
            OnHitEvent?.Invoke(attacker);
            AudioManager.Instance.PlayAudio("Hit", audioPlayer, true);
        }

        public void GetXP(ushort amount)
        {
            playerWeapon.SetScore(amount);
            AudioManager.Instance.PlayAudio("GetXP", audioPlayer, true);
        }

        public void Die(SyncableObject attacker, ushort destroyCount)
        {
            Debug.Log($"die : {transform.name}");
            OnDieEvent?.Invoke(attacker);
            AudioManager.Instance.PlayAudio("PlayerDie", audioPlayer, true);
        }

        public void DoChat(string chat)
        {
            UIManager.Instance.ChattingPanel.Show();
            UIManager.Instance.ChattingPanel.Hide();
        }

        public override void SetPosition(Vector3 position, bool immediately = false)
        {
            base.SetPosition(position, immediately);
            Debug.Log($"SetPosition : {position}");
            movement?.SetTargetPosition(position);
        }

        public override void SetRotation(Vector3 rotation)
        {
            // 여기에 로테이션
            view?.SetRotation(rotation);
        }

        public override void PlayAnimation(ushort animationType)
        {
            Debug.Log(2);
            SetAnimation?.Invoke(animationType);
        }

        public int CompareTo(PlayerController other)
        {
            if(this.Score > other.Score)
                return 1;
            if(this.Score < other.Score)
                return -1;
            return 0;
        }
    }
}
