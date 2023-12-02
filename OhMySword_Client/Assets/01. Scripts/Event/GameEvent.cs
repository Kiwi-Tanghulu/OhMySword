using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent
{
    public virtual void InitEvent() { }
    public abstract void StartEvent(int param);
    public abstract void UpdateEvent();
    public abstract void FinishEvent();
}
