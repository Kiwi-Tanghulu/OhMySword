using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using MyUI;
public class PauseUI : PanelUI
{
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button reStartBtn;

    [SerializeField] private RectTransform[] buttonsRects;
    [SerializeField] private Vector2[] btnStrPos;
    [SerializeField] private Vector2[] btnEndPos;
    [SerializeField] private float btnMoveTime;
    [SerializeField] private float btnWaitTime;

    private Tween[] btnTween = new Tween[3];

    //protected override void Awake()
    //{
    //    base.Awake();
    //}
    //public override void Show(Transform parent = null)
    //{
    //    base.Show(parent);
    //    StartCoroutine(BtnMove());
    //}

    private void Start()
    {
        reStartBtn.onClick.AddListener(() => UIManager.Instance.ChattingPanel.HideImmediediatly());
        reStartBtn.onClick.AddListener(() => Cursor.lockState = CursorLockMode.Locked);
        reStartBtn.onClick.AddListener(() => UIManager.Instance.panels.Peek().Hide());
        reStartBtn.onClick.AddListener(() => UIManager.Instance.PopUI());
    }

    public override void Show(Transform parent = null,bool isAnimation = false)
    {
        base.Show(parent,false);
        StartCoroutine(BtnMove());
    }
    public override void Hide(bool isAnimation = false)
    {
        base.Hide(false);
        StopAllCoroutines();
        for (int i = 0; i < buttonsRects.Length; i++)
        {
            btnTween[i].Kill();
            buttonsRects[i].anchoredPosition = btnStrPos[i];
        }
<<<<<<< HEAD
=======
        
        //UIManager.Instance.panels.Pop();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(UIManager.Instance.panels.Count <= 0)
            {
                Show();
                UIManager.Instance.ChattingPanel.HideImmediediatly();
                UIManager.Instance.SetCursorActive(true);
            }
            else
            {
                UIManager.Instance.panels.Peek().Hide();
                UIManager.Instance.PopUI();
            }
        }
>>>>>>> main
    }

    private IEnumerator BtnMove()
    {

        for (int i = 0; i < buttonsRects.Length; i++)
        {
            btnTween[i] = buttonsRects[i].DOAnchorPos(btnEndPos[i], btnMoveTime).SetEase(Ease.InOutQuart);
            yield return new WaitForSeconds(btnWaitTime);
        }
    }

    public void GotoLobby()
    {
        RoomManager.Instance.ExitRoom();
        SceneManager.LoadScene("LobbyScene");
    }
}

