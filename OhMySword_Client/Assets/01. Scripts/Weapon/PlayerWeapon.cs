using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Range(0.001f,0.05f)]
    [SerializeField] private float scoreSize; // 스코어당 길어지는 길이! 예) 0.01이라면 1점당 0.01이 커짐 100점이라면 1이 커지고

    [SerializeField] private Transform swordPivot;
    private int swordSize; // 나중에 필요할 수 있어서...
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

    public void SetSwordSize() //나중에 특정 구간에서 칼이 바뀐다면 채워넣어야함
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
