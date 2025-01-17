using System;
using System.Collections.Generic;
using Base.Network;
using Packets;
using UnityEngine;
using UnityEngine.Events;

namespace OhMySword.Player
{
    public class PlayerController : SyncableObject, IDamageable, IHitable, IComparable<PlayerController>
    {
        private AudioSource audioPlayer;

        [field : SerializeField]
        public bool IsDie { get; private set; } = false;

        private PlayerMove movement;
        private PlayerView view;
        private PlayerWeapon playerWeapon;
        private PlayerChat playerChat;
        private PlayerInfo info;

        public ActiveRagdoll Ragdoll { get; private set; }

        public UnityEvent<SyncableObject> OnHitEvent;
        public UnityEvent<SyncableObject> OnDieEvent;

        public UnityEvent<int> SetAnimation;

        public TMPro.TextMeshPro NameTag;

        public string Nickname;
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
            Ragdoll = GetComponent<ActiveRagdoll>();
            playerChat = GetComponent<PlayerChat>();
            info = GetComponent<PlayerInfo>();

            audioPlayer = Ragdoll.Hip.GetComponent<AudioSource>();
        }

        public override void OnDeleted()
        {
        }

        //public void SetSkin(ushort skinID)
        //{
            
        //}

        public void SetNickname(string nickname)
        {
            this.Nickname = nickname;  
            NameTag.text = nickname;
        }

        public void OnDamage(int damage, GameObject performer, Vector3 point)
        {
            if (IsDie)
                return;

            SyncableObject attacker = performer.GetComponent<SyncableObject>();
            C_AttackPacket attackPacket = null;

            if(attacker != null)
                attackPacket = new C_AttackPacket((ushort)ObjectType.Player, ObjectID, attacker.ObjectID, (ushort)damage);
            else
                attackPacket = new C_AttackPacket((ushort)ObjectType.Player, ObjectID, ushort.MaxValue, (ushort)damage);

            NetworkManager.Instance.Send(attackPacket);
            Debug.Log("OnDamage");
        }

        public void Hit(SyncableObject attacker)
        {
            // 맵 콘파이너에 의해 죽을 경우 attacker가 null로 들어옴
            Debug.Log($"Hit: {transform.name}");
            OnHitEvent?.Invoke(attacker);
            AudioManager.Instance.PlayAudio("Hit", audioPlayer, true);
        }

        public void GetXP(ushort amount, bool immediately)
        {
            if (IsDie)
                return;

            info.GetXpCount++;
            playerWeapon.SetScore(amount, immediately);
            AudioManager.Instance.PlayAudio("GetXP", audioPlayer, true);

            if(this.ObjectID == RoomManager.Instance.PlayerID)
                UIManager.Instance.MainCanvas.Find("InGamePanel/Leaderboard").GetComponent<LeaderBoard>().ChangeScore(playerWeapon.GetScore());
        }

        public void Die(SyncableObject attacker, ushort destroyCount)
        {
            if (IsDie)
                return;

            //둘 다
            OnDieEvent?.Invoke(attacker);
            //UIManager.Instance.MainCanvas.Find("DiePanel").GetComponent<DiePanel>().Show();
            
            //둘 다
            AudioManager.Instance.PlayAudio("PlayerDie", audioPlayer, true);
            
            if(attacker != null && attacker.ObjectID == RoomManager.Instance.PlayerID)
            {
                Debug.Log($"{transform.name} : 킬 증가");
                attacker.GetComponent<PlayerInfo>().KillCount++;
            }

            if(this.ObjectID == RoomManager.Instance.PlayerID)
            {
                //자기
                IsDie = true;
                if(attacker != null)
                {
                    if (attacker.TryGetComponent<PlayerController>(out PlayerController p))
                    {
                        info.KilledPlayerName = p.Nickname;
                    }
                }
                else
                {
                    info.KilledPlayerName = "자연사";
                }

                //자기
                if (SaveManager.Instance.data.BestScore < Score)
                    SaveManager.Instance.data.BestScore = Score;

                SaveManager.Instance.Save();

                string[] infos = new string[6];
                infos[0] = info.KillCount.ToString();
                infos[1] = info.GetXpCount.ToString();
                infos[2] = playerWeapon.GetScore().ToString();
                infos[3] = info.KilledPlayerName;
                infos[4] = destroyCount.ToString();
                infos[5] = SaveManager.Instance.data.BestScore.ToString();
                DiePanel diePanel = UIManager.Instance.MainCanvas.Find("DiePanel").GetComponent<DiePanel>();
                diePanel.gameObject.SetActive(true);
                diePanel.Show(infos, RoomManager.Instance.GetCurrentRanking(ObjectID));
            }
        }

        public void DoChat(string chat)
        {
            if (IsDie)
                return;

            if(RoomManager.Instance.PlayerID != this.ObjectID)
            {
                if(!UIManager.Instance.ChattingPanel.IsChat)
                {
                    UIManager.Instance.ChattingPanel.Show();
                    UIManager.Instance.ChattingPanel.Hide();
                }
            }
            else
            {
                UIManager.Instance.ChattingPanel.Hide();
            }

            playerChat.CreateMessageText(chat);
        }

        public override void SetPosition(Vector3 position, bool immediately = false)
        {
            if (IsDie)
                return;

            base.SetPosition(position, immediately);
            Debug.Log($"SetPosition : {position}");
            movement?.SetTargetPosition(position);
        }

        public override void SetRotation(Vector3 rotation)
        {
            if (IsDie)
                return;

            // 여기에 로테이션
            view?.SetRotation(rotation);
        }

        public override void PlayAnimation(ushort animationType)
        {
            if (IsDie)
                return;

            SetAnimation?.Invoke(animationType);
        }

        public int CompareTo(PlayerController other)
        {
            if(this.Score < other.Score)
                return 1;
            if(this.Score > other.Score)
                return -1;
            return 0;
        }
    }
}
