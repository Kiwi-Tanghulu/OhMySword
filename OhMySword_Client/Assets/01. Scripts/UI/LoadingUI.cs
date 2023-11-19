using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class LoadingUI : UIBase
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text helpText;
    [SerializeField] private string[] helpTexts;

    [SerializeField] private float helpTextTime;
    [SerializeField] private float titleTypeTime;

    private void Start()
    {
        Show();
    }
    public override void Show()
    {
        base.Show();
        StartCoroutine(HelpText());
    }

    public override void Hide()
    {
        base.Hide();
        StopAllCoroutines();
    }
    private IEnumerator HelpText()
    {
        float curhelpTextTime = 0;
        float curTypeTime = 0;
        int titleCnt = 0;
        while (true)
        {
            curhelpTextTime += Time.deltaTime;
            curTypeTime += Time.deltaTime;

            if (curhelpTextTime > helpTextTime)
            {
                helpText.DOFade(0f, 0.5f).OnComplete(() =>
                {
                    helpText.text = helpTexts[Random.Range(0, helpTexts.Length)];
                    helpText.DOFade(1, 0.5f);
                });
                curhelpTextTime = 0;
            }
            if (curTypeTime > titleTypeTime)
            {
                if (titleCnt > 2)
                {
                    titleText.text = "맵과 하나가 되는중";
                    titleCnt = 0;
                }
                else
                {
                    titleCnt++;
                    titleText.text += ".";
                }
                curTypeTime = 0;
            }
            yield return null;
        }
    }
}
