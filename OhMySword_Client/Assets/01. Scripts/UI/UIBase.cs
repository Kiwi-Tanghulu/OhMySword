using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum UIType
{
    Chase = 0,
    Fixed,
    Panel,
    PopUp,
    Full,
}
public class UIBase : MonoBehaviour
{
    public UIType uiType;
    [field: SerializeField]
    public bool IsOpen { get; protected set; } = true;

    //private void Start()
    //{
    //    Hide();
    //}
    public virtual void Show()
    {
        if (IsOpen)
            return;

        transform.localScale = Vector3.one;

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
