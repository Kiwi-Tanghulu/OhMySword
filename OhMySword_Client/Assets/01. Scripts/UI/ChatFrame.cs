using MyUI;
using TMPro;

public class ChatFrame : PanelUI
{
    private TMP_Text textUI = null;

    protected override void Awake()
    {
        base.Awake();
        textUI = transform.Find("Text")?.GetComponent<TMP_Text>();
    } 
    
	private void Start()
    {
        textUI.text = "";
    }

    public void SetText(string text)
    {
        textUI.text = text;
    }
}
