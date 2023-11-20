using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChat : MonoBehaviour
{
    [SerializeField] private Transform messageSpawnTrm;

    public void CreateMessageText(string m)
    {
        MessageText message = PoolManager.Instance.Pop("MessageText", messageSpawnTrm.position) as MessageText; 
        RectTransform messageRect = message.GetComponent<RectTransform>();
        message.SetText(m);

        foreach (Transform legacyMessage in messageSpawnTrm)
        {
            legacyMessage.position += messageRect.rect.height * messageRect.localScale.y * Vector3.up;
        }

        message.transform.SetParent(transform);

        foreach (Transform legacyMessage in messageSpawnTrm)
        {
            legacyMessage.position += messageRect.rect.height * messageRect.localScale.y * Vector3.up * 0.5f;
        }
    }
}
