using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyUI
{
    public class SettingUI : FullUI
    {
        [SerializeField] private Button inputBtn;
        [SerializeField] private Button audioBtn;
        [SerializeField] private Transform selectBar;
        [SerializeField] private GameObject inputOption;
        [SerializeField] private GameObject audioOption;

        private GameObject activeOption = null;

        protected override void Awake()
        {
            base.Awake();

            inputBtn.onClick.AddListener(() =>
            {
                selectBar.transform.position = new Vector3(inputBtn.transform.position.x, selectBar.transform.position.y);
                ActiveOption(inputOption);
            });
            audioBtn.onClick.AddListener(() =>
            {
                selectBar.transform.position = new Vector3(audioBtn.transform.position.x, selectBar.transform.position.y);
                ActiveOption(audioOption);
            });

            ActiveOption(inputOption);
            Hide();
        }

        private void ActiveOption(GameObject option)
        {
            if (activeOption != null)
                activeOption.SetActive(false);

            activeOption = option;
            activeOption.SetActive(true);
        }
    }
}


