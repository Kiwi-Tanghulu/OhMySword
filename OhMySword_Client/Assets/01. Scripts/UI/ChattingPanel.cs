using MyUI;
using Packets;
using UnityEngine;

namespace OhMySword.UI
{
    public class ChattingPanel : FixedUI
    {
        [SerializeField] ChatFrame chatPrefab = null;
        private Transform contentTrm = null;

        protected override void Awake()
        {
            base.Awake();
            
            contentTrm = transform.Find("ScrollRect/Content")?.transform;
        }

        public void DoChat(string sender, string chat)
        {
            ChatFrame chatBox = Instantiate(chatPrefab);
            chatBox.SetText($"{sender} : {chat}");
            chatBox.Show(contentTrm);
        }

        public void SendChat(string text)
        {
            C_ChattingPacket packet = new C_ChattingPacket(text);
            NetworkManager.Instance.Send(packet);
        }
    }
}
