using DG.Tweening;
using MyUI;
using Packets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : PanelUI
{
    [Space(10f)]
    [SerializeField] int lengthLimit = 5;

    [Space(10f)]
	[SerializeField] TMP_InputField nicknameField = null;
    [SerializeField] Button enterButton = null;

    [SerializeField] private float panelChangeDuration;
    private Transform panelPivot;
    public string Nickname {
        get => nicknameField.text;
        set {
            nicknameField.text = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        panelPivot = transform.parent;
    }
    private void Start()
    {
        //Show();
        enterButton.onClick.AddListener(() => UIManager.Instance.ClaerStack());
    }

    public void CreateEnterRequest()
    {
        if(Nickname.Length >= lengthLimit)
        {
            Nickname = $"Nickname should be under the {lengthLimit}";
            return;
        }

        C_RoomEnterPacket reqPacket = new C_RoomEnterPacket();
        reqPacket.nickname = Nickname;

        NetworkManager.Instance.Send(reqPacket);
        enterButton.interactable = false;
    }

    public override void Show(Transform parent = null, bool isAnimation = false)
    {
        base.Show(parent, true);
        panelPivot.DOScale(1f, panelChangeDuration).SetEase(Ease.InOutCubic);
    }

    public override void Hide(bool isAnimation = false)
    {
        base.Hide(true);
        panelPivot.DOScale(0f, panelChangeDuration).SetEase(Ease.InOutCubic);
    }
}
