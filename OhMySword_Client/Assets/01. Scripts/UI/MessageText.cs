using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageText : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
