using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using OhMySword.Player;
using System;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> leaderBoardScoreTexts;
    [SerializeField] private List<TextMeshProUGUI> leaderBoardNameTexts;
    void Start()
    {
        RoomManager.Instance.OnRankingChangedEvent += ChangeRanking;
    }

    private void ChangeRanking(List<PlayerController> list)
    {
        for(int i = 0; i < leaderBoardScoreTexts.Count; i++)
        {
            leaderBoardScoreTexts[i].text = list[i].Score.ToString();
            leaderBoardNameTexts[i].text = list[i].name;
        }
    }
}
