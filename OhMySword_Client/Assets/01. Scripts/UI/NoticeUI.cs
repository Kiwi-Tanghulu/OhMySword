using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace MyUI
{
    public class NoticeUI : PopUpUI
    {
        private TextMeshProUGUI text;
        private RectTransform rect;
        private Vector2 popupPos;
        private Vector2 hidePos;
        [SerializeField]
        private Color originColor;

        protected override void Awake()
        {
            Hide();

            text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
            rect = text.GetComponent<RectTransform>();
            popupPos = new Vector2(0, Screen.height * 0.25f);
            hidePos = new Vector2(0, Screen.height * 0.5f);
            rect.anchoredPosition = popupPos;
            text.color = originColor;
        }

        public void Show(float time, string message, Transform parent = null)
        {
            rect.anchoredPosition = popupPos;
            text.color = originColor;
            text.text = message;

            base.Show(time, parent);

            Sequence seq = DOTween.Sequence();
            seq.Insert(time * 0.5f, rect.DOAnchorPosY(hidePos.y, time * 0.5f));
            seq.Join(text.DOFade(0, time * 0.5f));
            seq.Play();
        }

        public override void Hide()
        {
            base.Hide();
            transform.SetParent(UIManager.Instance.ScreenCanvas);
        }
    }
}