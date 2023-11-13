using Base.Feedback;
using DG.Tweening;
using UnityEngine;

namespace OhMySword.Feedback
{
    public class PositionFloatingFeedback : FeedbackBase
    {
        [SerializeField] Transform target = null;
        [SerializeField] float duration = 1f;
        [SerializeField] float delay = 0f;
        
        [Space(10f)]
        [SerializeField] Ease positionUpEase = Ease.InExpo;
        [SerializeField] float positionUpOffset = 2f;

        [Space(10f)]
        [SerializeField] Ease positionDownEase = Ease.InOutQuad;
        [SerializeField] float positionDownOffset = -1f;

        [Space(10f)]
        [SerializeField] Ease finalEase = Ease.InQuint;
        [SerializeField] float finalOffset = 0f;

        private Sequence seq = null;
        private Vector3 origin = Vector3.zero;

        private void Awake()
        {
            origin = target.localPosition;            
        }

        public override void CreateFeedback()
        {
            origin = target.localPosition;
            Debug.Log($"Position Floating View From Origin : {target.localPosition}");
            target.localPosition += Vector3.down * 2f;

            seq = DOTween.Sequence();
            seq.AppendInterval(delay);
            seq.Append(target.DOLocalMoveY(origin.y + positionUpOffset, duration * 0.7f).SetEase(positionUpEase));
            seq.Append(target.DOLocalMoveY(origin.y + positionDownOffset, duration * 0.2f).SetEase(positionDownEase));
            seq.Append(target.DOLocalMoveY(origin.y + finalOffset, duration * 0.1f).SetEase(finalEase));
            seq.AppendCallback(() => target.localPosition = origin);
        }

        public override void FinishFeedback()
        {
            if (seq != null)
            {
                seq.Kill();
                target.localPosition = origin;
            }
        }
    }
}
