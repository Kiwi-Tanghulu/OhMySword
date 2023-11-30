using System;
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

        foreach(GameEventType eventType in Enum.GetValues(typeof(GameEventType)))
        {
            if (eventType == GameEventType.None)
                continue;

            string typeName = eventType.ToString();

            try
            {
                Type type = Type.GetType($"{typeName}Event");
                gameEventDictionary.Add(eventType, Activator.CreateInstance(type) as GameEvent);
                gameEventDictionary[eventType].InitEvent();
            }
            catch
            {
                Debug.LogError($"{eventType}Event is none");
            }
        }

        CurrentEventType = GameEventType.None;
        currentGameEvent = null;
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
