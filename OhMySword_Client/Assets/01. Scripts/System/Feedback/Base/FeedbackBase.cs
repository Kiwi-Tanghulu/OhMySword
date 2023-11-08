using UnityEngine;

namespace Base.Feedback
{
    public abstract class FeedbackBase : MonoBehaviour
    {
        public abstract void CreateFeedback();
        public abstract void FinishFeedback();

        protected virtual void OnDisable()
        {
            FinishFeedback();
        }
    }
}