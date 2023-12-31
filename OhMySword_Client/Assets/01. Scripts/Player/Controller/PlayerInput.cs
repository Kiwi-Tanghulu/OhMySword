using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent<Vector3> Movement;
    public UnityEvent<Vector2> MouseMove;
    public UnityEvent LeftClick;
    public UnityEvent<int> Alpha1;
    public UnityEvent<int> Alpha2;
    public UnityEvent<int> Alpha3;
    public UnityEvent Space;

    [field : SerializeField] public bool Inputable { get; set; } = true;

    private void Start()
    {
        //LeftClick.AddListener(() => CameraManager.Instance.ShakeCam());
    }

    private void Update()
    {
        //if (!Inputable)
        //    return;

        //if (UIManager.Instance.ActiveUI)
        //{
        //    Debug.Log("active ui");
        //    return;
        //}

        if (UIManager.Instance.ChattingPanel.IsChat || UIManager.Instance.ActiveUI || !Inputable)
        {
            Movement?.Invoke(Vector3.zero);
            MouseMove?.Invoke(Vector3.zero);
            Debug.Log("Is Chat");
        }
        else
        {
            Movement?.Invoke(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
            MouseMove?.Invoke(new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")));

            if (Input.GetKeyDown(KeyCode.Space))
                Space?.Invoke();

            if (Input.GetMouseButtonDown(0))
                LeftClick?.Invoke();
            if (Input.GetKeyDown(KeyCode.Alpha1))
                Alpha1?.Invoke(1);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                Alpha2?.Invoke(2);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                Alpha3?.Invoke(3);
            if (Input.GetKeyDown(KeyCode.Space))
                Space?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (UIManager.Instance.ChattingPanel.IsChat)
            {
                //UIManager.Instance.ChattingPanel.SetFieldSelect(false);
                //UIManager.Instance.ChattingPanel.HideImmediediatly();
            }
            else
            {
                if(!UIManager.Instance.ActiveUI)
                {
                    UIManager.Instance.ChattingPanel.SetFieldSelect(true);
                    UIManager.Instance.ChattingPanel.Show();
                }
            }
        }
    }

    private void OnDestroy()
    {
        LeftClick = default;
    }
}
