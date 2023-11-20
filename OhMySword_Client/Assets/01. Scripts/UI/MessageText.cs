using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageText : PoolableMono
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private float showTiem = 10f;

    private void Start()
    {
        text = GetComponent<TextMeshPro>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void SetText(string str)
    {
        text.text = str;
        Debug.Log(GetComponent<RectTransform>().rect.height);
    }

    public override void Init()
    {
        StartCoroutine(Hide());
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(showTiem);

        PoolManager.Instance.Push(this);
    }
}
