using UnityEngine;

namespace Base.Feedback
{
    public abstract class Feedback : MonoBehaviour
    {
        public abstract void CreateFeedback();
        public abstract void FinishFeedback();

        protected virtual void OnDisable()
        {
            FinishFeedback();
        }
    }
}