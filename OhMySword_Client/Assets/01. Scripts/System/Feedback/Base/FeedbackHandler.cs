using System.Collections.Generic;
using UnityEngine;

namespace Base.Feedback
{
    public class FeedbackHandler : MonoBehaviour
    {
        private List<FeedbackBase> feedbackList;

        private void Awake()
        {
            feedbackList = new List<FeedbackBase>();
            GetComponents<FeedbackBase>(feedbackList);
        }

        private void Update()
        {
            // if(Input.GetKeyDown(KeyCode.Space))
            //     PlayFeedback();
        }

        public void PlayFeedback()
        {
            FinishFeedback();

            foreach(FeedbackBase f in feedbackList)
                f.CreateFeedback();
        }

        public void FinishFeedback()
        {
            foreach(FeedbackBase f in feedbackList)
                f.FinishFeedback();
        }
    }
}