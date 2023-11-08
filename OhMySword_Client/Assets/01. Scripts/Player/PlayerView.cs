using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Vector3 forward { get; private set; }
    public Vector3 right { get; private set; }

    [SerializeField] private float rotateSpeed;
    [SerializeField] private float maxRotate;
    [SerializeField] private float minRotate;
    [SerializeField] private Transform target;
    [SerializeField] private Transform hip;

    private Vector2 rotation = Vector2.zero;

    public void RotateCamera(Vector2 vector)
    {
        rotation.x -= vector.y * rotateSpeed;
        rotation.y += vector.x * rotateSpeed;

        rotation.x = Mathf.Clamp(rotation.x, minRotate, maxRotate);

        target.rotation = Quaternion.Euler(rotation.x, rotation.y, 0);
        hip.rotation = Quaternion.Euler(0, rotation.y, 0);

        forward = new Vector3(target.forward.x, 0, target.forward.z).normalized;
        right = new Vector3(target.right.x, 0, target.right.z).normalized;
    }
}
