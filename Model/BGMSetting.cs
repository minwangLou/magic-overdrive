using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BGMSetting
{
    public BGMType bgmType;
    public AudioClip musicClip;
    [Range(0f, 1f)] public float musicVolume = 1f;
}

[CreateAssetMenu(menuName = "Audio/BGM Setting List")]
public class BGMSettingList: ScriptableObject
{
    public List<BGMSetting> bgmSettings;
}
