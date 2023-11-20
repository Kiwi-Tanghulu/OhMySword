using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameSettingUI : UIBase
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider masterSoundSlider;
    [SerializeField] private Slider SystemSoundSlider;
    [SerializeField] private Slider mouseSpeedSlider;
    [SerializeField] private TMP_InputField mouseSpeedInputField;

    private int _value = 0;
    public int Value
    {
        get { return _value; }
        private set
        {
            _value = value;
            mouseSpeedInputField.text = _value.ToString();
            mouseSpeedSlider.value = _value / 100f;
        }
    }
    private PlayerView playerView;

    private void Awake()
    {
        mouseSpeedInputField.onEndEdit.AddListener(ChangeMouseSpeedSlider);
        mouseSpeedSlider.onValueChanged.AddListener((v) => Value = (int)(v * 100));
    }

    public override void Show()
    {
        base.Show();
        transform.localScale = new Vector3(0.8f, 0.8f, 1);
        UIManager.Instance.panels.Push(this);
    }

    public override void Hide()
    {
        base.Hide();
        UIManager.Instance.panels.Pop();
    }

    private void ChangeBgmSoundSlider()
    {
        AudioManager.Instance.SetVolume(AudioType.BGM, bgmSlider.value * 100f);
    }
    private void ChangeSfxSoundSlider()
    {
        AudioManager.Instance.SetVolume(AudioType.SFX, sfxSlider.value * 100f);
    }
    private void ChangeMasterSoundSlider()
    {
        AudioManager.Instance.SetVolume(AudioType.Master, masterSoundSlider.value * 100f);
    }
    private void ChangeSystemSoundSlider()
    {
        AudioManager.Instance.SetVolume(AudioType.System, SystemSoundSlider.value * 100f);
    }

    private void ChangeMouseSpeedSlider(string _value)
    {
        if (int.TryParse(_value, out int v))
        {
            Value = v;
            if (playerView == null)
                playerView = FindObjectOfType<PlayerView>();
            playerView.RotateSpeedOffset = v / 100f;
        }
    }
}
