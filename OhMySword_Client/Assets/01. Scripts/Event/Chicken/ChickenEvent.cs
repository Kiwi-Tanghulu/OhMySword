using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class ChickenEvent : GameEvent
{
    private GameObject chickenResource;
    private Chicken chicken;
    private TextMeshProUGUI chickenText;

    public override void InitEvent()
    {
        base.InitEvent();
        chickenResource = Resources.Load<GameObject>("Prefabs/Chicken");
        chickenText = GameObject.Find("MainCanvas/ChickenText").GetComponent<TextMeshProUGUI>();
    }

    public override void StartEvent()
    {
        Debug.Log("Start Chicken Event");
        chicken = GameObject.Instantiate(chickenResource).GetComponent<Chicken>();
        RoomManager.Instance.ObjectParent.gameObject.SetActive(false);
        chickenText.transform.DOScale(0.75f, 1.2f).SetEase(Ease.InOutBack);
    }

    public override void UpdateEvent()
    {
        
    }

    public override void FinishEvent()
    {
        Debug.Log("Finish Chicken Event");
        GameObject.Destroy(chicken.gameObject);
        chicken = null;
        RoomManager.Instance.ObjectParent.gameObject.SetActive(true);
        chickenText.transform.DOScale(0f, 1.2f).SetEase(Ease.InOutBack);
    }
}
