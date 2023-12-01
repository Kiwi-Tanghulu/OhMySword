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
        public bool IsClosed = false;
        public bool OnEvent = false;

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

            CreateEvent();
        }

        public override void Clear()
        {
            base.Clear();
            IsClosed = true;
        }

        public override void ReleasePlayer(Player player)
        {
            base.ReleasePlayer(player);

            if(Capacity >= maxPlayerCount)
                RoomManager.Instance.ReleaseRoom(this);
        }

        public void StartEvent(ushort eventType)
        {
            if (IsClosed)
                return;

            OnEvent = true;

            S_EventStartPacket packet = new S_EventStartPacket(eventType);
            Broadcast(packet);
            Console.WriteLine($"[Room] Event Started {{ Room ID : {RoomID}, Event ID : {eventType} }}");
        }

        public void CloseEvent()
        {
            if (IsClosed)
                return;

            OnEvent = false;

            S_EventEndPacket packet = new S_EventEndPacket();
            Broadcast(packet);

            Console.WriteLine($"[Room] Event Closed {{ Room ID : {RoomID} }}");
        }

        private void CreateEvent()
        {
            if (IsClosed)
                return;

            DelayCallback(60f * 1f, () => {
                AddJob(() => {
                    if (OnEvent)
                        return;

                    StartEvent(0);
                    DelayCallback(60f * 2f, () => {
                        AddJob(() => {
                            CloseEvent();
                            CreateEvent();
                        });
                    });
                });
            });
        }

        public async void DelayCallback(float delay, Action callback)
        {
            await Task.Delay((int)(delay * 1000));
            callback?.Invoke();
        }
    }
}
