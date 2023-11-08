using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Range(0.001f,0.05f)]
    [SerializeField] private float scoreSize;

    [SerializeField] private Transform swordPivot;
    private int swordSize;
    private int currentScore = 0;
    private int nextScore = 0;
    private bool isGrowing = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestGetScore((int)Random.Range(1, 20));
        }
    }

    private void TestGetScore(int value)
    {
        nextScore += value;
        if(currentScore != nextScore && isGrowing == false)
        {
            StartCoroutine(GrowUpSword());
        }

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
