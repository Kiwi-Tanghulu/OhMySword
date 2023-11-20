using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChat : MonoBehaviour
{
    [SerializeField] private Transform messageSpawnTrm;
    private List<MessageText> messages = new();

    public void CreateMessageText(string m)
    {
        MessageText message = PoolManager.Instance.Pop("MessageText", messageSpawnTrm.position) as MessageText; 
    }
}
