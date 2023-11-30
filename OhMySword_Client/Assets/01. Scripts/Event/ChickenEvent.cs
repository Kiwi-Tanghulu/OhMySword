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
        //chickenResource = Resources.Load<GameObject>("Chicken");
    }

    public override void StartEvent()
    {
        Debug.Log("Start Chicken Event");
    }

    public override void UpdateEvent()
    {
        Debug.Log("Start Chicken Event");
    }

    public override void FinishEvent()
    {
        base.FinishEvent();
        
    }
}
