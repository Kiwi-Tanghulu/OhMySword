using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenEvent : GameEvent
{
    private GameObject chickenResource;
    private GameObject chickenObject;
    private Vector3 chickenSpawnPoint = Vector3.zero;

    public override void InitEvent()
    {
        base.InitEvent();
        chickenResource = Resources.Load<GameObject>("Prefabs/Chicken");
    }

    public override void StartEvent()
    {
        Debug.Log("Start Chicken Event");
        chickenObject = GameObject.Instantiate(chickenResource);
    }

    public override void UpdateEvent()
    {
        
    }

    public override void FinishEvent()
    {
        Debug.Log("Finish Chicken Event");
        GameObject.Destroy(chickenObject);
        chickenObject = null;
    }
}
