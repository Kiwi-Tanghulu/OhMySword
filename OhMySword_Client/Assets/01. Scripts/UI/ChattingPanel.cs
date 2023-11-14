using MyUI;
using Packets;
using System.Collections;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

namespace OhMySword.UI
{
    public class ChattingPanel : FixedUI
    {
        [SerializeField] ChatFrame chatPrefab = null;
        private Transform contentTrm = null;
        private TMP_InputField textField = null;
        private CanvasGroup canvasGroup = null;

        [SerializeField] private float fadeTime = 1f;

        [field:SerializeField]
        public bool IsChat { get; set; } = false;

        protected override void Awake()
        {
            base.Awake();
            
            contentTrm = transform.Find("ScrollRect/Content")?.transform;
            textField = transform.Find("TextField").GetComponent<TMP_InputField>();
            canvasGroup = GetComponent<CanvasGroup>();
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

        public void SetSelect()
        {
            textField.Select();
            textField.ActivateInputField();
        }

        public void Show()
        {
            StopCoroutine(Fade());
            canvasGroup.alpha = 1;
        }

        public override void Hide()
        {
            StartCoroutine(Fade());
        }

        private IEnumerator Fade()
        {
            float percent = 0;

            while(percent <= 1)
            {
                canvasGroup.alpha = Mathf.Lerp(1, 0, percent);

                percent += Time.deltaTime / fadeTime;

                yield return null;
            }
        }
    }
}
