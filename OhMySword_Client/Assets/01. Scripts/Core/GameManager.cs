using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public ushort UserID = 0;
    public ushort PlayerID = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
            return;
        }

        PacketManager.Instance = new PacketManager();
        NetworkManager.Instance = new NetworkManager();
    }

    private void Start()
    {
        NetworkManager.Instance.Connect("172.30.1.16", 2651);
    }

    private void Update()
    {
        NetworkManager.Instance.FlushPacketQueue();
    }
}
