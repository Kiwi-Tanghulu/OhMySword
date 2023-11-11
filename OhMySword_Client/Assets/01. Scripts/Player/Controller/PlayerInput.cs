using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent<Vector3> Movement;
    public UnityEvent<Vector2> MouseMove;
    public UnityEvent LeftClick;

    private void Update()
    {
        LeftClick?.Invoke();
        Movement?.Invoke(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
        MouseMove?.Invoke(new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")));
    }
}
