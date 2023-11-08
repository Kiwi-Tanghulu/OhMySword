using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyUI
{
    public class PauseUI : FullUI
    {
        private Button continueBtn;
        private Button settingBtn;
        private Button quitBtn;

        protected override void Awake()
        {
            base.Awake();

            Transform btnContainer = transform.Find("Contents/BtnContainer");

            continueBtn = btnContainer.Find("ContinueBtn").GetComponent<Button>();
            settingBtn = btnContainer.Find("SettingBtn").GetComponent<Button>();
            quitBtn = btnContainer.Find("QuitBtn").GetComponent<Button>();

            settingBtn.onClick.AddListener(() => UIManager.Instance.GetUI<FullUI>("SettingUI").ShowAndRecord());
        }
    }
}

