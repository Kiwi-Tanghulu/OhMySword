using MyUI;
using Packets;
using TMPro;
using UnityEngine;

namespace OhMySword.UI
{
    public class ChattingPanel : FixedUI
    {
        [SerializeField] ChatFrame chatPrefab = null;
        private Transform contentTrm = null;
        private TMP_InputField textField = null;

        protected override void Awake()
        {
            base.Awake();
            
            contentTrm = transform.Find("ScrollRect/Content")?.transform;
            textField = transform.Find("TextField").GetComponent<TMP_InputField>();
        }

        public void DoChat(string sender, string chat)
        {
            ChatFrame chatBox = Instantiate(chatPrefab);
            chatBox.SetText($"{sender} : {chat}");
            chatBox.Show(contentTrm);
        }

        public void SendChat(string text)
        {
            if(Input.GetKeyDown(KeyCode.Return) == false)
                return;
            
            if(textField.text.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length == 0)
            {
                textField.text = "";
                return;
            }

            C_ChattingPacket packet = new C_ChattingPacket(text);
            NetworkManager.Instance.Send(packet);

            textField.text = "";
            textField.ActivateInputField();
        }

        public void IsChat(bool value)
        {
            UIManager.Instance.IsChatting = value;
        }
    }
}
