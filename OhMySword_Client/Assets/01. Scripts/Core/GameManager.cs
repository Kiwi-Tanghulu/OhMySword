using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] AudioAssetsSO audioAssets;
    [SerializeField] AudioMixer masterMixer;
    public ushort UserID = 0;

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

        SceneLoader.Instance = gameObject.AddComponent<SceneLoader>();
        UIManager.Instance = gameObject.AddComponent<UIManager>();

        PacketManager.Instance = new PacketManager();
        NetworkManager.Instance = new NetworkManager();
        AudioManager.Instance = new AudioManager(audioAssets, masterMixer);
    }

    private void Start()
    {
        // NetworkManager.Instance.Connect("172.30.1.16", 2651);
        // NetworkManager.Instance.Connect("172.31.3.230", 2651);
        // NetworkManager.Instance.Connect("172.31.3.95", 2651);
        // NetworkManager.Instance.Connect("192.168.0.19", 2651);
        // NetworkManager.Instance.Connect("192.168.0.16", 2651);
        NetworkManager.Instance.Connect("222.99.93.62", 2651); // SEH00N Desktop
    }

    private void Update()
    {
        NetworkManager.Instance.FlushPacketQueue();
    }

    private void OnDestroy()
    {
        if(Instance != this)
            return;

        NetworkManager.Instance.Disconnect();
    }

    public void ResetClient()
    {
        // 클라이언트 초기화
        // 클라이언트 경고 UI
        // 게임 매니저도 새로 생겨야 하고 네트워크 매니저도 새로 생겨야 함
        // 정말 게임을 껏다 키는 거 처럼 되어야 함
    }
}
