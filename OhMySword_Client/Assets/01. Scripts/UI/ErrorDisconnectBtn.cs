using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorDisconnectBtn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.SetCursorActive(true);
        GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.ResetClient());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
