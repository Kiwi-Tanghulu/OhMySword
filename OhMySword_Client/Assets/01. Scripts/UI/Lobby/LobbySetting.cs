using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LobbySetting : MonoBehaviour
{
    [SerializeField] private GameSettingUI settingUI;

    public Stack<Transform> lobbyStack = new Stack<Transform>();

    private bool isOpen;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(lobbyStack.Count <= 0)
            {
                settingUI.transform.localScale = new Vector3(0.8f, 0.8f, 1);
                isOpen = true;
                lobbyStack.Push(settingUI.transform);
            }
            else
            {
                if(isOpen == true)
                {
                    Hide();
                }
                else
                {
                    lobbyStack.Peek().DOScale(0f, 0.25f).SetEase(Ease.InOutCubic);
                }
                lobbyStack.Pop();
            }
        }
    }
    public void Hide()
    {
        settingUI.transform.localScale = Vector3.zero;
        isOpen = false;
    }
}
