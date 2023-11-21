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
            if (playerView == null)
                playerView = FindObjectOfType<PlayerView>();
            playerView.RotateSpeedOffset = _value / 100f;
            SaveManager.Instance.data.mouseSpeed = _value;
        }
    }

    public void Init()
    {
        Value = SaveManager.Instance.data.mouseSpeed;
    }
    private PlayerView playerView;

    private void Awake()
    {
        mouseSpeedInputField.onEndEdit.AddListener(ChangeMouseSpeedSlider);
        mouseSpeedSlider.onValueChanged.AddListener((v) => Value = (int)(v * 100));

    }
    private void Start()
    {
        AudioManager.Instance.SetVolume(AudioType.BGM, SaveManager.Instance.data.bgmSound);
        AudioManager.Instance.SetVolume(AudioType.Master, SaveManager.Instance.data.masterSound);
        AudioManager.Instance.SetVolume(AudioType.SFX, SaveManager.Instance.data.sfxSound);
        AudioManager.Instance.SetVolume(AudioType.System, SaveManager.Instance.data.systemSound);

        bgmSlider.value = SaveManager.Instance.data.bgmSound;
        SystemSoundSlider.value = SaveManager.Instance.data.systemSound;
        sfxSlider.value = SaveManager.Instance.data.sfxSound;
        masterSoundSlider.value = SaveManager.Instance.data.masterSound;
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

    public void ChangeBgmSoundSlider()
    {
        AudioManager.Instance.SetVolume(AudioType.BGM, bgmSlider.value);
        SaveManager.Instance.data.bgmSound = bgmSlider.value;
    }
    public void ChangeSfxSoundSlider()
    {
        AudioManager.Instance.SetVolume(AudioType.SFX, sfxSlider.value);
        SaveManager.Instance.data.sfxSound = sfxSlider.value;
    }
    public void ChangeMasterSoundSlider()
    {
        AudioManager.Instance.SetVolume(AudioType.Master, masterSoundSlider.value);
        SaveManager.Instance.data.masterSound = masterSoundSlider.value;
    }
    public void ChangeSystemSoundSlider()
    {
        AudioManager.Instance.SetVolume(AudioType.System, SystemSoundSlider.value);
        SaveManager.Instance.data.systemSound = SystemSoundSlider.value;
    }

    private void ChangeMouseSpeedSlider(string _value)
    {
        if (int.TryParse(_value, out int v))
        {
            Value = v;
        }
    }
}
