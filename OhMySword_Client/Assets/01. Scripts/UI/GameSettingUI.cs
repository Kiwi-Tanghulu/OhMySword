using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameSettingUI : UIBase
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider masterSoundSlider;
    [SerializeField] private Slider SystemSoundSlider;
    [SerializeField] private Slider mouseSpeedSlider;

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
}
