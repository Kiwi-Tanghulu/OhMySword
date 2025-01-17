using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimation : MonoBehaviour
{
    [field : SerializeField] public bool Animationable { get; set; } = true;

    [SerializeField] private Transform followTrm;
    [SerializeField] private GameObject visualObj;
    private Animator anim;
    private Transform animTrm;
    private ActiveRagdoll ragdoll;
    private PlayerAttack attack;

    public UnityEvent<int> PlayEmotionEvent;
    public UnityEvent<int> StopEmotionEvent;

    private int currnetPlayEmotion;

    private readonly int animHash = Animator.StringToHash("animatorHash");
    [SerializeField] private bool isEmotionPlay = false;

    private void Start()
    {
        ragdoll = GetComponent<ActiveRagdoll>();
        attack = GetComponent<PlayerAttack>();
        anim = transform.Find("Animation").GetComponent<Animator>();
        animTrm = anim.transform;
    }

    public void SetAnimation(int hash)
    {
        if (!Animationable)
            return;

        Debug.Log(3);
        if (isEmotionPlay)
            StopEmtion();
        else
        {
            if(hash == 0)
            {
                attack.Attack();
            }
            else
            {
                PlayEmotion(hash);
            }
        }
    }

    private void PlayEmotion(int hash)
    {
        if (!ragdoll.IsGround)
            return;

        isEmotionPlay = true;
        animTrm.position = followTrm.position + Vector3.down * 0.55f;
        animTrm.rotation = Quaternion.Euler(0, followTrm.eulerAngles.y, 0);
        visualObj.SetActive(false);
        animTrm.gameObject.SetActive(true);
        anim.SetInteger(animHash, hash);
        PlayEmotionEvent?.Invoke(hash);
        currnetPlayEmotion = hash;
    }

    public void StopEmtion()
    {
        isEmotionPlay = false;
        visualObj.SetActive(true);
        animTrm.gameObject.SetActive(false);
        anim.SetInteger(animHash, 0);
        StopEmotionEvent?.Invoke(currnetPlayEmotion);
        currnetPlayEmotion = 0;
    }
}
