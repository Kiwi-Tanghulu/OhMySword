using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private float f = 0;
    private void Update()
    {
        Debug.Log(easeInCubic(2));
    }
    private float easeInCubic(float x)
    {
        return x * x * x;
    }
}
