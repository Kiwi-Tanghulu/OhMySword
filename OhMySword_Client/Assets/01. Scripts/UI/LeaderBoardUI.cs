using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using OhMySword.Player;
using System;
using MyUI;
public class LeaderBoard : FixedUI
{
    [SerializeField] private List<TextMeshProUGUI> leaderBoardScoreTexts;
    [SerializeField] private List<TextMeshProUGUI> leaderBoardNameTexts;
    [SerializeField] private TextMeshProUGUI currentScoreText;

    void Start()
    {
        RoomManager.Instance.OnRankingChangedEvent += ChangeRanking;
        currentScoreText.text = "���� ���� : 0";

        for(int i = 0; i < leaderBoardScoreTexts.Count; i++)
        {
            leaderBoardScoreTexts[i].text = "";
            leaderBoardNameTexts[i].text = "";
        }
    }

    private void ChangeRanking(List<PlayerController> list)
    {
        for (int i = 0; i < leaderBoardScoreTexts.Count; i++)
        {
            leaderBoardScoreTexts[i].text = "";
            leaderBoardNameTexts[i].text = "";
        }
        for (int i = 0; i < list.Count; i++)
        {
            leaderBoardScoreTexts[i].text = list[i].Score.ToString();
            leaderBoardNameTexts[i].text = list[i].nickname;
        }
    }

    public void ChangeScore(int value)
    {
        currentScoreText.text = $"���� ���� : {value}";
    }
}
