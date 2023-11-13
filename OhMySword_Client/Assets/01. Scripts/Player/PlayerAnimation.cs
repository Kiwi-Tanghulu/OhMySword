using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerAttack attack;
    private Animator anim;

    private List<bool> emotionList = new List<bool>();

    private void Start()
    {
        
    }

    public void PlayAnim(int hash)
    {
        if (hash == 0)
        {
            PlayAttackAnim();
        }
        else
        {
            PlayEmotionAnim(hash);
        }
    }

    public void PlayAttackAnim()
    {
        attack.
    }

    public void PlayEmotionAnim(int hash)
    {

    }
}
