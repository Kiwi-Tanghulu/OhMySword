using OhMySword.UI;
using System.Collections.Generic;
using UnityEngine;
using MyUI;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    public Stack<UIBase> panels = new Stack<UIBase>();

    
    private Stack<BaseUI> panelUIs = new Stack<BaseUI>();
    private Dictionary<string, BaseUI> uiContainer = new();
    private BaseUI escapeUI;
    public bool IsActivePanelUI => panelUIs.Count > 0;

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
    public T GetUI<T>(string name) where T : BaseUI
    {
        T ui = null;

        if (uiContainer.ContainsKey(name))
        {
            ui = uiContainer[name] as T;
        }
        else
        {
            ui = mainCanvas.Find(name).GetComponent<T>();
        }

        return ui;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapeUI();
        }
    }
    public void EscapeUI()
    {
        if (IsActivePanelUI)
        {
            panelUIs.Peek().Hide();
            panelUIs.Pop();
        }
        else
        {
            BaseUI escapeUI = GetUI<BaseUI>("Setting");
            escapeUI.Show();
            panelUIs.Push(escapeUI);
        }
    }
    public void RecordHistory(BaseUI ui)
    {
        panelUIs.Push(ui);
    }
    public void PopUI()
    {
        panels.Pop();

        if(panels.Count <= 0)
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void ClaerStack()
    {
        panels.Clear();
    }
}
