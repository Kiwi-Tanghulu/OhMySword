using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class PlayerChat : MonoBehaviour
{
    [SerializeField] private Transform messageSpawnTrm;
    private MessageText beforeMsg;

    public void CreateMessageText(string m)
    {
        MessageText message = PoolManager.Instance.Pop("MessageText", messageSpawnTrm.position) as MessageText;
        message.transform.SetParent(messageSpawnTrm);
        message.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        message.transform.localPosition = Vector3.zero;
        message.SetText(m);
        beforeMsg = message;

        StartCoroutine(AdjustChatPos());
    }

    private IEnumerator AdjustChatPos()
    {
        yield return new WaitForSeconds(0.05f);

        float height = beforeMsg.GetComponent<RectTransform>().rect.height;
        Debug.Log(height);

        for (int i = 0; i < messageSpawnTrm.childCount - 1; i++)
        {
            messageSpawnTrm.GetChild(i).localPosition += height * Vector3.up;
        }
    }
}
