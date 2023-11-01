using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] string ip = "172.31.3.230";
    [SerializeField] int port = 2651;

    private void Awake()
    {
        if(Instance != null) { Destroy(gameObject); return; }

        Instance = this;

        PacketManager.Instance = new PacketManager();
        NetworkManager.Instance = new NetworkManager();
    }
}
