using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Range(0.001f,0.05f)]
    [SerializeField] private float scoreSize; // ���ھ�� ������� ����! ��) 0.01�̶�� 1���� 0.01�� Ŀ�� 100���̶�� 1�� Ŀ����

    [SerializeField] private Transform swordPivot;
    private int swordSize; // ���߿� �ʿ��� �� �־...
    public int SwordSize => swordSize;

    private int currentSwordLevel = 0;
    public int CurrentSwordLevel => currentSwordLevel;

    private ushort currentScore = 0;
    private ushort nextScore = 0;
    private bool isGrowing = false;

    private BoxCollider col;

    private void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Other_Hitbox"))
        {
            //attack process
            Debug.Log(other.name);
        }
    }

    public void SetCollision(bool value)
    {
        col.enabled = value;
    }

    public void SetSwordSize() //���߿� Ư�� �������� Į�� �ٲ�ٸ� ä���־����
    {

    }
    public void SetScore(ushort value)
    {
        nextScore = value;
        if(currentScore != nextScore && isGrowing == false)
            StartCoroutine(GrowUpSword());
    }

    private IEnumerator GrowUpSword()
    {
        float checkTime = 0f;
        isGrowing = true;
        while (true)
        {
            if(checkTime >= 1f)
            {
                currentScore++;
                checkTime = 0f;
            }
            if(currentScore == nextScore)
                break;

            swordPivot.localScale = new Vector3(1f, swordPivot.localScale.y + (scoreSize * Time.deltaTime), 1f);
            checkTime += Time.deltaTime;
            yield return null;
        }
        isGrowing = false;
    }
}
