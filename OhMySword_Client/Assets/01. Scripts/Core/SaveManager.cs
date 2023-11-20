using UnityEngine;

public class SaveData
{
    public int BestScore;
    public float mouseSpeed;
    public float bgmSound;
    public float sfxSound;
    public float masterSound;
    public float systemSound;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public SaveData data;

    private void Awake()
    {
        Load();

        DontDestroyOnLoad(gameObject);
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("data", json);
    }

    public void Load()
    {
        string json = PlayerPrefs.GetString("data", "");

        if (json != "")
            data = JsonUtility.FromJson<SaveData>(json);
        else
            data = new();
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
