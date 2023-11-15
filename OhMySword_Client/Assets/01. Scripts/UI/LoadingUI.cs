using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class LoadingUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text helpText;
    [SerializeField] private string[] helpTexts;

    [SerializeField] private float helpTextTime;
    [SerializeField] private float titleTypeTime;
    private void Start()
    {
        StartCoroutine(HelpText());
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
            
            if(curhelpTextTime > helpTextTime)
            {
                helpText.rectTransform.DOScale(Vector3.zero, 0.3f).OnComplete(() => {
                    helpText.text = helpTexts[Random.Range(0, helpTexts.Length)];
                    helpText.rectTransform.DOScale(Vector3.one, 0.3f); 
                });
                curhelpTextTime = 0;
            }
            if(curTypeTime > titleTypeTime)
            {
                if(titleCnt > 3)
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
