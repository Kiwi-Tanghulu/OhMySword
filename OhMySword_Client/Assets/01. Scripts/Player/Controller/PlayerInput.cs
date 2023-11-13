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

    private void Update()
    {
        if(!UIManager.Instance.IsChatting)
        {
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
        
        Movement?.Invoke(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
        MouseMove?.Invoke(new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")));
    }
}
