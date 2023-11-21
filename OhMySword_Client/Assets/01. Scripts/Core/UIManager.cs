using OhMySword.UI;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    public Stack<UIBase> panels = new Stack<UIBase>();

    public bool ActiveUI => panels.Count > 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private Transform mainCanvas = null;
    public Transform MainCanvas {
        get {
            if(mainCanvas == null)
                mainCanvas = GameObject.Find("MainCanvas")?.transform;
            return mainCanvas;
        }
    }

    private RoomPanel roomPanel = null;
    public RoomPanel RoomPanel {
        get {
            if(roomPanel == null)
                roomPanel = MainCanvas?.Find("Panels/GameStartPanelPivot/RoomPanel")?.GetComponent<RoomPanel>();
            return roomPanel;
        }
    }

    private ChattingPanel chattingPanel = null;
    public ChattingPanel ChattingPanel {
        get {
            if(chattingPanel == null)
                chattingPanel = MainCanvas?.Find("ChattingPanel")?.GetComponent<ChattingPanel>();
            return chattingPanel;
        }
    }

    private AudioSource uiAudioPlayer = null;
    public AudioSource UIAudioPlayer {
        get {
            if(uiAudioPlayer == null)
                uiAudioPlayer = Camera.main?.transform.Find("UIAudioPlayer")?.GetComponent<AudioSource>();
            return uiAudioPlayer;
        }
    }

    public void PopUI()
    {
        panels.Pop();

        if(panels.Count <= 0)
            Cursor.lockState = CursorLockMode.Locked;
    }
}
