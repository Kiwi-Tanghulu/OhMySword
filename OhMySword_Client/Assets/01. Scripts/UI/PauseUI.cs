using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace MyUI
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] private Button continueBtn;
        [SerializeField] private Button settingBtn;
        [SerializeField] private Button reStartBtn;

        [SerializeField] private RectTransform[] buttonsRects;
        [SerializeField] private Vector2[] btnStrPos;
        [SerializeField] private Vector2[] btnEndPos;
        [SerializeField] private float btnMoveTime;
        [SerializeField] private float btnWaitTime;
        //protected override void Awake()
        //{
        //    base.Awake();
        //}
        //public override void Show(Transform parent = null)
        //{
        //    base.Show(parent);
        //    StartCoroutine(BtnMove());
        //}
        private void Start()
        {
            StartCoroutine(BtnMove());
        }
        private IEnumerator BtnMove()
        {
            for(int i = 0; i < buttonsRects.Length; i++)
            {
                buttonsRects[i].anchoredPosition = btnStrPos[i];
            }
            for(int i = 0; i < buttonsRects.Length; i++)
            {
                buttonsRects[i].DOAnchorPos(btnEndPos[i], btnMoveTime).SetEase(Ease.InOutQuart);
                yield return new WaitForSeconds(btnWaitTime);
            }
        }
    }
}

