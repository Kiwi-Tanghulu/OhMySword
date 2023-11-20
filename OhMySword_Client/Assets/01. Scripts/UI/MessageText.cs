using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageText : PoolableMono
{
    private TextMeshPro text;

    private void Start()
    {
        text = GetComponent<TextMeshPro>();
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }

    public void SetText(string str)
    {
        text.text = str;
    }

    public override void Init()
    {

    }
}
