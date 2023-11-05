using Packets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : MonoBehaviour
{
    [SerializeField] int lengthLimit = 5;

	private TMP_InputField nicknameField = null;
    private Button enterButton = null;

    public string Nickname {
        get => nicknameField.text;
        set {
            nicknameField.text = value;
        }
    }

    private void Awake()
    {
        nicknameField = transform.Find("NicknameField")?.GetComponent<TMP_InputField>();
        enterButton = transform.Find("EnterButton")?.GetComponent<Button>();
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
