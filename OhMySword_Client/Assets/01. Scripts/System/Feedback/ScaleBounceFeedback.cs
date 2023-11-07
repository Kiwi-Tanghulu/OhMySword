using UnityEngine;
using DG.Tweening;
using Base.Feedback;

namespace OhMySword.Feedback
{
    public class ScaleBounceFeedback : FeedbackBase
    {
        [SerializeField] Transform target = null;
        [SerializeField] float duration = 1f;
        [SerializeField] float delay = 0.8f;

        [Space(10f)]
        [SerializeField] Ease scaleUpEase = Ease.InExpo;
        [SerializeField] float scaleUpFactor = 1.4f;

        [Space(10f)]
        [SerializeField] Ease scaleDownEase = Ease.InOutQuad;
        [SerializeField] float scaleDownFactor = 0.8f;

        [Space(10f)]
        [SerializeField] Ease finalEase = Ease.InQuint;
        [SerializeField] float finalFactor = 1f;

        private Sequence seq = null;

        public override void CreateFeedback()
        {
            target.localScale = Vector3.zero;

            seq = DOTween.Sequence();
            seq.AppendInterval(delay);
            seq.Append(target.DOScale(Vector3.one * scaleUpFactor, duration * 0.7f).SetEase(scaleUpEase));
            seq.Append(target.DOScale(Vector3.one * scaleDownFactor, duration * 0.2f).SetEase(scaleDownEase));
            seq.Append(target.DOScale(Vector3.one * finalFactor, duration * 0.1f).SetEase(finalEase));
            seq.AppendCallback(() => target.localScale = Vector3.one);
        }

        public override void FinishFeedback()
        {
            if(seq != null)
                seq.Kill();
            target.localScale = Vector3.one;
        }
    }
}
