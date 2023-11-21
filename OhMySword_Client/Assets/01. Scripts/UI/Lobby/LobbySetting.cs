using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySetting : MonoBehaviour
{
    [SerializeField] private GameSettingUI settingUI;
    bool isOpen = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isOpen == false)
            {
                settingUI.transform.localScale = new Vector3(0.8f, 0.8f, 1);
                isOpen = true;
            }
            else
            {
                Hide();
            }
        }
    }
    public void Hide()
    {
        settingUI.transform.localScale = Vector3.zero;
        isOpen = false;
    }
}
