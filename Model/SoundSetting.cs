using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundSetting
{
    public SoundType effectName;
    public AudioClip audioClip;
    [Range(0f, 1f)] public float soundVolume = 1f;
}

[CreateAssetMenu(menuName = "Audio/Sound Setting List")]
public class SoundSettingList : ScriptableObject
{
    public List<SoundSetting> soundSettings;
}
