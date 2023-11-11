using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerView : MonoBehaviour
{
    [SerializeField] UnityEvent<Vector3> onRotatedEvent;

    [SerializeField] private float rotateSpeed;
    [SerializeField] private float maxRotate;
    [SerializeField] private float minRotate;
    [SerializeField] private Transform target;
    [SerializeField] private Transform hip;

    private Vector2 rotation = Vector2.zero;
    private Vector3 targetRotation = Vector3.zero;
    private Vector3 prevRotation = Vector3.zero;

    public void RotateCamera(Vector2 vector)
    {
        rotation.x -= vector.y * rotateSpeed;
        rotation.y += vector.x * rotateSpeed;

        rotation.x = Mathf.Clamp(rotation.x, minRotate, maxRotate);

        target.rotation = Quaternion.Euler(rotation.x, rotation.y, 0);
        hip.rotation = Quaternion.Euler(0, rotation.y, 0);

        onRotatedEvent?.Invoke(hip.eulerAngles);
    }

    public void SetRotation(Vector3 euler)
    {
        prevRotation = targetRotation;
        targetRotation = euler;

        StartCoroutine(RotateCoroutine(target, Quaternion.Euler(prevRotation.x, prevRotation.y, 0), Quaternion.Euler(targetRotation.x, targetRotation.y, 0)));
        StartCoroutine(RotateCoroutine(hip, Quaternion.Euler(0, prevRotation.y, 0), Quaternion.Euler(0, targetRotation.y, 0)));
    }

    private IEnumerator RotateCoroutine(Transform trm, Quaternion from, Quaternion target, float duration = 0.09f)
    {
        float timer = 0f;

        while (timer < duration)
        {
            trm.rotation = Quaternion.Lerp(from, target, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        trm.rotation = target;
    }
}
