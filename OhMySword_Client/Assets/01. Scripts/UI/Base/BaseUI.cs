using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUI
{
    public class BaseUI : MonoBehaviour
    {
        public UIType uiType;

        [field: SerializeField]
        public bool IsOpen { get; protected set; } = true;
        public bool isWorld = false;
        public bool isPooling = false;

        protected virtual void Awake()
        {
            IsOpen = true;
        }

        public virtual void Show(Transform parent = null)
        {
            if (IsOpen)
                return;

            transform.localScale = Vector3.one;
            if (parent != null)
                transform.SetParent(parent);

            IsOpen = true;
        }

        public virtual void Hide()
        {
            if (!IsOpen)
                return;

            transform.localScale = Vector3.zero;
            IsOpen = false;
        }
    }
}