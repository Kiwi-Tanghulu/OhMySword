using UnityEngine;

public class BlockPanel : MonoBehaviour
{
	private void Start()
    {
        if(NetworkManager.Instance.IsConnected)
            gameObject.SetActive(false);
    }
}
