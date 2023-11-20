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

    public string Nickname {
        get => nicknameField.text;
        set {
            nicknameField.text = value;
        }
    }

    private void Start()
    {
        //Show();
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
}
