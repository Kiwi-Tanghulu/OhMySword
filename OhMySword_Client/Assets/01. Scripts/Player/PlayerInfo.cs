using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int KillCount { get; set; } = 0;
    public int GetXpCount { get; set; } = 0;
    public int BestScore { get; set; } = 0;
    public string KilledPlayerName { get; set; } = "";
}
