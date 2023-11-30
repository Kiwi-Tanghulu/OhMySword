using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameEventType
{
    Chicken = 0,
    None,
}

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    private Dictionary<GameEventType, GameEvent> gameEventDictionary;
    [field: SerializeField]
    public GameEventType CurrentEventType { get; private set; }
    private GameEvent currentGameEvent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        currentGameEvent?.UpdateEvent();
    }

    public void StartEvent(int eventType)
    {
        if ((GameEventType)eventType == GameEventType.None)
            return;
        if (currentGameEvent != null)
            return;

        CurrentEventType = (GameEventType)eventType;
        currentGameEvent = gameEventDictionary[CurrentEventType];
        currentGameEvent.StartEvent();
    }

    public void FinishEvent()
    {
        currentGameEvent?.FinishEvent();
        CurrentEventType = GameEventType.None;
        currentGameEvent = null;
    }
}
