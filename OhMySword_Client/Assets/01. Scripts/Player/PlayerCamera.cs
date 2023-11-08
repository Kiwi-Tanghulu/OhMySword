using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float cameraMoveSpeed = 5f;
    public Transform lookTarget;

    public void MoveCamera(Vector2 mouseMove)
    {
        Debug.Log(mouseMove);
        lookTarget.Rotate(mouseMove * cameraMoveSpeed * Time.deltaTime);
    }
}
