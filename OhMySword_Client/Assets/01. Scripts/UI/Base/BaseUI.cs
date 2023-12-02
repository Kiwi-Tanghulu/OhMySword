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

        public virtual void Show(Transform parent = null, bool isAnimation = false)
        {
            if (IsOpen)
                return;

            if (parent != null)
                transform.SetParent(parent);

            if(!isAnimation)
                transform.localScale = Vector3.one;

            IsOpen = true;
        }

        public virtual void Hide(bool isAnimation = false)
        {
            if (!IsOpen)
                return;

            if(!isAnimation)
                transform.localScale = Vector3.zero;

            IsOpen = false;
        }
    }
}