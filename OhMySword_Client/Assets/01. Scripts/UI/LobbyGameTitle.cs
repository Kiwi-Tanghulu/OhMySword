using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class LobbyGameTitle : MonoBehaviour
{

    private void Awake()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        //DOTween.To(() => rectTransform.anchoredPosition, x => rectTransform.anchoredPosition = x, new Vector2(0, -300f), 1f).SetEase(Ease.InOutBounce);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
