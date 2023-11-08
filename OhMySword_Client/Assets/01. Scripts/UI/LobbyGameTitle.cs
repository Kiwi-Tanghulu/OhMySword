using UnityEngine;
using DG.Tweening;

public class LobbyGameTitle : MonoBehaviour
{
    [System.Serializable]
    public class JumpInfo
    {
        public Vector3 destination;
        public float power;
        public float duration;
    }

    [SerializeField] JumpInfo[] jumpInfos;
    [SerializeField] Ease ease = Ease.InOutQuad;

    private void Start()
    {
        DoJump();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            DoJump();
    }

    private void DoJump()
    {
        transform.localPosition = jumpInfos[0].destination;

        Sequence seq = DOTween.Sequence();
        for(int i = 1; i < jumpInfos.Length; i++)
            seq.Append(transform.DOLocalJump(jumpInfos[i].destination, jumpInfos[i].power, 1, jumpInfos[i].duration).SetEase(ease));
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}
