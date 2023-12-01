using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenEvent : GameEvent
{
    private GameObject chickenResource;
    private Chicken chicken;
    private Vector3 chickenSpawnPoint = Vector3.zero;

    public override void InitEvent()
    {
        base.InitEvent();
        chickenResource = Resources.Load<GameObject>("Prefabs/Chicken");
    }

    public override void StartEvent()
    {
        Debug.Log("Start Chicken Event");
        chicken = GameObject.Instantiate(chickenResource).GetComponent<Chicken>();
        RoomManager.Instance.ObjectParent.gameObject.SetActive(false);
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
    }
}
