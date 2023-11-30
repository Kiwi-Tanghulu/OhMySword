using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUI
{
    public class FullUI : BaseUI
    {
        protected override void Awake()
        {
            base.Awake();

            Hide();
        }

        public void ShowAndRecord(Transform parent = null)
        {
            base.Show(parent);

            MyUIManager.Instance.RecordHistory(this);
        }
    }

}
