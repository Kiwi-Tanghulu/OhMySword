using System.Collections.Generic;
using UnityEngine;

namespace Base.Feedback
{
    public class FeedbackHandler : MonoBehaviour
    {
        private List<Feedback> feedbackList;

        private void Awake()
        {
            feedbackList = new List<Feedback>();
            GetComponents<Feedback>(feedbackList);
        }

        public void PlayFeedback()
        {
            FinishFeedback();

            foreach(Feedback f in feedbackList)
                f.CreateFeedback();
        }

        public void FinishFeedback()
        {
            foreach(Feedback f in feedbackList)
                f.FinishFeedback();
        }
    }
}