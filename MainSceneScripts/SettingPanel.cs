using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public static SettingPanel instance;

    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;
    [HideInInspector]public CanvasGroup canvasGroup;
    private AudioManager audioManager;

    private void Awake()
    {
        instance = this;
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        audioManager = AudioManager.instance;

        bgmVolumeSlider.value = audioManager.bgmVolume;
        sfxVolumeSlider.value = audioManager.sfxVolume;
    }

    private void Update()
    {
        if(canvasGroup.alpha == 1)
        {
            if (bgmVolumeSlider != null) audioManager.bgmVolume = bgmVolumeSlider.value;
            if (sfxVolumeSlider != null) audioManager.sfxVolume = sfxVolumeSlider.value;
        }
    }

}
