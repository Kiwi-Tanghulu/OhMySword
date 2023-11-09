using OhMySword.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

	private Transform mainCanvas = null;
    public Transform MainCanvas {
        get {
            if(mainCanvas == null)
                mainCanvas = GameObject.Find("MainCanvas")?.transform;
            return mainCanvas;
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
}
