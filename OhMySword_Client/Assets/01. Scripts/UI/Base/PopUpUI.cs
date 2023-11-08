using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyUI
{
    public class PopUpUI : BaseUI
    {
        protected override void Awake()
        {
            Hide();
        }

        public virtual void Show(float time, Transform parent = null)
        {
            Show(parent);
            StartCoroutine(Hide(time));
        }

        private IEnumerator Hide(float time)
        {
            yield return new WaitForSeconds(time);

            Hide();
        }
    }
}

