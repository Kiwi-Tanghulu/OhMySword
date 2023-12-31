using System.Diagnostics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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
        NetworkManager.Instance.Connect("222.99.93.62", 2651); // SEH00N Desktop
    }

    private void Update()
    {
        NetworkManager.Instance.FlushPacketQueue();

        if (!NetworkManager.Instance.IsConnected && SceneManager.GetActiveScene().name != "ErrorScene")
            SceneManager.LoadScene("ErrorScene");
    }

    private void OnDestroy()
    {
        if(Instance != this)
            return;

        NetworkManager.Instance.Disconnect();
    }

    public void ResetClient()
    {
        NetworkManager.Instance.Disconnect();
        Process.Start(Application.dataPath + "/../OhMySword_Client.exe");
        Application.Quit();
    }
}