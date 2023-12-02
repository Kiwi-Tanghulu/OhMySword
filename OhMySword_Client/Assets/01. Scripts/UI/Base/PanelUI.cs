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

            Hide(true);
        }

        public override void Show(Transform parent = null, bool isAnimation = false)
        {
            base.Show(parent, isAnimation);
            UIManager.Instance.RecordHistory(this);
        }

        public override void Hide(bool isAnimation = false)
        {
            base.Hide(isAnimation);
            UIManager.Instance.EraseHistory();
        }

    }
}
