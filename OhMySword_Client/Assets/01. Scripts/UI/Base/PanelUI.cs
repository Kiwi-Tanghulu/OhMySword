using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUI
{
    public class PanelUI : BaseUI
    {
        protected override void Awake()
        {
            base.Awake();

            Hide();
        }

        public void Show(Vector2 pos, Transform parent = null)
        {
            Show(parent);
            UIManager.Instance.RecordHistory(this);
            transform.position = pos;
        }
    }
}
