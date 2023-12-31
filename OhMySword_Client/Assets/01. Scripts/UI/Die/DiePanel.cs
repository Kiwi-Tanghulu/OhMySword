using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class DiePanel : MonoBehaviour
{

    [SerializeField] private RectTransform[] partyEffects;
    [SerializeField] private float partyEffectMoveSpeed;
    [SerializeField] private RectTransform partyEffectEndTrm;
    [SerializeField] private RectTransform partyEffectStartTrm;

    [SerializeField] private string[] testString;
    [SerializeField] private TextMeshProUGUI[] dieInfoTexts;


    [SerializeField] private RectTransform rank;
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private RectTransform rankMoveTrm;

    public void Show(string[] infoDatas, int currentRank)
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(TypingInfoText(infoDatas, currentRank+1));
        UIManager.Instance.MainCanvas.Find("PauseUI").gameObject.SetActive(false);
        UIManager.Instance.MainCanvas.Find("InGamePanel").gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void GoToLobby()
    {
        RoomManager.Instance.ExitRoom();
        SceneManager.LoadScene("LobbyScene");
    }

    private IEnumerator TypingInfoText(string[] infoDatas, int currentRank) // 0 = killCnt, 1 = pieceCnt, 2 = currentScore, 3 = killer, 4 = structureCnt, 5 = topScroe
    {
        yield return new WaitForSeconds(2f);
        transform.localScale = Vector3.one;
        rankText.text = currentRank.ToString() + "등";
        rank.DOAnchorPos(rankMoveTrm.anchoredPosition, 2.4f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(1.8f);
        int index = 0;
        int setIndex = 0;
        float checkNum = 0;
        while (setIndex < dieInfoTexts.Length)
        {
            for (int i = setIndex; i <= index; i++)
            {
                if (i == 3)
                {
                    dieInfoTexts[i].text = "";
                    for (int j = 0; j < 7; j++)
                    {
                        dieInfoTexts[i].text += (char)Random.Range(65, 123);
                    }
                }
                else
                {
                    dieInfoTexts[i].text = ((int)Random.Range(1, 100000)).ToString();
                }
            }
            checkNum += 0.05f;
            if (checkNum > 0.15f)
            {
                checkNum = 0;
                if (index < dieInfoTexts.Length - 1)
                    index++;
                else
                {
                    dieInfoTexts[setIndex].text = infoDatas[setIndex];
                    setIndex++;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }

        //for(int i = 0; i < dieInfoTexts.Length; i++)
        //{
        //    for(int j = 0; j < infoDatas[i].Length; j++)
        //    {
        //        dieInfoTexts[i].text += infoDatas[i][j];
        //        yield return new WaitForSeconds(0.25f);
        //    }
        //}
        while (true)
        {
            for (int i = 0; i < partyEffects.Length; i++)
            {
                RectTransform partyEffect = partyEffects[i];
                partyEffect.anchoredPosition = new Vector3(partyEffect.anchoredPosition.x, partyEffect.anchoredPosition.y - Time.deltaTime * partyEffectMoveSpeed, 0f);
                if (partyEffect.anchoredPosition.y < partyEffectEndTrm.anchoredPosition.y)
                    partyEffect.anchoredPosition = partyEffectStartTrm.anchoredPosition;
            }
            yield return null;
        }
    }
}
