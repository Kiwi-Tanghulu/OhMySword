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

        gameEventDictionary = new();
        foreach(GameEventType eventType in Enum.GetValues(typeof(GameEventType)))
        {
            if (eventType == GameEventType.None)
                continue;

            string typeName = eventType.ToString();

            try
            {
                Type type = Type.GetType($"{typeName}Event");
                Debug.Log(type);
                gameEventDictionary.Add(eventType, Activator.CreateInstance(type) as GameEvent);
                gameEventDictionary[eventType].InitEvent();
            }
            catch (Exception e)
            {

                Debug.LogError($"{eventType}Event is none : {e.ToString()}");
            }
        }

        CurrentEventType = GameEventType.None;
        currentGameEvent = null;
    }

    private void Update()
    {
        currentGameEvent?.UpdateEvent();
    }

    public void StartEvent(int eventType, int param)
    {
        Debug.Log("2");

        if ((GameEventType)eventType == GameEventType.None)
            return;
        if (currentGameEvent != null)
            return;

        CurrentEventType = (GameEventType)eventType;
        currentGameEvent = gameEventDictionary[CurrentEventType];
        currentGameEvent.StartEvent(param);
    }

    public void FinishEvent()
    {
        currentGameEvent?.FinishEvent();
        CurrentEventType = GameEventType.None;
        currentGameEvent = null;
    }
}
