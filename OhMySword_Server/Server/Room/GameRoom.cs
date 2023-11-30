using Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GameRoom : Room
    {
        public int maxPlayerCount = 20;
        public ushort RoomID;
        public bool OnEvent;

        public int Capacity => (maxPlayerCount - players.Count);

        public GameRoom()
        {
            for(int i = 0; i < 3; i++)
            {
                ScoreBox scoreBox1 = new ScoreBox(this, 3, ObjectType.StoneScoreBox);
                scoreBox1.ResetPosition();
                PublishObject(scoreBox1);

                ScoreBox scoreBox2 = new ScoreBox(this, 4, ObjectType.GemScoreBox);
                scoreBox2.ResetPosition();
                PublishObject(scoreBox2);
            }

            for (int i = 0; i < 2; i++)
            {
                ScoreBox scoreBox1 = new ScoreBox(this, 6, ObjectType.WoodenScoreBox);
                scoreBox1.ResetPosition();
                PublishObject(scoreBox1);

                ScoreBox scoreBox2 = new ScoreBox(this, 8, ObjectType.DesertTreeScoreBox);
                scoreBox2.ResetPosition();
                PublishObject(scoreBox2);
            }

            for (int i = 0; i < 1; i++)
            {
                ScoreBox scoreBox1 = new ScoreBox(this, 9, ObjectType.EggScoreBox);
                scoreBox1.ResetPosition();
                PublishObject(scoreBox1);

                ScoreBox scoreBox2 = new ScoreBox(this, 11, ObjectType.CoreScoreBox);
                scoreBox2.ResetPosition();
                PublishObject(scoreBox2);
            }

            DelayCallback(60f * 5, () => {
                AddJob(() => {
                    if (OnEvent)
                        return;

                    StartEvent(0);
                    DelayCallback(60f * 2, () => {
                        AddJob(() => {
                            CloseEvent();
                        });
                    });
                });
            });
        }

        public override void ReleasePlayer(Player player)
        {
            base.ReleasePlayer(player);

            if(Capacity >= maxPlayerCount)
                RoomManager.Instance.ReleaseRoom(this);
        }

        public void StartEvent(ushort eventType)
        {
            OnEvent = true;

            S_EventStartPacket packet = new S_EventStartPacket(eventType);
            Broadcast(packet);
        }

        public void CloseEvent()
        {
            OnEvent = false;

            S_EventEndPacket packet = new S_EventEndPacket();
            Broadcast(packet);
        }

        public async void DelayCallback(float delay, Action callback)
        {
            await Task.Delay((int)(delay * 1000));
            callback?.Invoke();
        }
    }
}
