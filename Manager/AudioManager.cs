using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("AudioEffect")]
    public int audioSourcePoolSize = 10;
    public SoundSettingList soundSettingList;
    private List<SoundSetting> cachedSoundSettings;

    [Header("BGM")]
    public BGMSettingList bgmSettingList;
    private AudioSource backgroundMusicPlayer;
    private List<BGMSetting> cachedBgmSettings;

    private Dictionary<SoundType, SoundSetting> soundMap;
    private Dictionary<AudioSource, SoundSetting> activeSoundMap;

    private Dictionary<BGMType, BGMSetting> bgmMap;
    private BGMSetting currentBGMSetting;


    private List<AudioSource> audioPlayerPool;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float bgmVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;


    void Awake()
    {
        // ������ʼ��
        if (instance == null) instance = this;
        else Destroy(gameObject);

        LoadVolumeValue();

        backgroundMusicPlayer = gameObject.AddComponent<AudioSource>();

        activeSoundMap = new Dictionary<AudioSource, SoundSetting>();

        // ���� ScriptableObject ����
        cachedSoundSettings = soundSettingList != null ? soundSettingList.soundSettings : new List<SoundSetting>();
        cachedBgmSettings = bgmSettingList != null ? bgmSettingList.bgmSettings : new List<BGMSetting>();

        // ������Чӳ���
        soundMap = new Dictionary<SoundType, SoundSetting>();
        foreach (var sound in cachedSoundSettings)
        {
            if (!soundMap.ContainsKey(sound.effectName))
                soundMap.Add(sound.effectName, sound);
        }

        // ���� BGM ӳ���
        bgmMap = new Dictionary<BGMType, BGMSetting>();
        foreach (var bgm in cachedBgmSettings)
        {
            if (!bgmMap.ContainsKey(bgm.bgmType))
                bgmMap.Add(bgm.bgmType, bgm);
        }

        // ������Ч��������
        audioPlayerPool = new List<AudioSource>();
        for (int i = 0; i < audioSourcePoolSize; i++)
        {
            AudioSource player = gameObject.AddComponent<AudioSource>();
            audioPlayerPool.Add(player);
        }

        foreach (var kvp in soundMap)
        {
            //Debug.Log($"[AudioManager] Registered sound: {kvp.Key} => {kvp.Value.audioClip?.name}");
        }
    }

    private void Update()
    {
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        foreach (var entry in activeSoundMap)
        {
            var source = entry.Key;
            var setting = entry.Value;

            if (source.isPlaying)
            {
                source.volume = setting.soundVolume * sfxVolume;
            }
        }

        // ���� BGM ����
        if (backgroundMusicPlayer.isPlaying && bgmMap.TryGetValue(currentBGMSetting.bgmType, out var bgmSetting))
        {
            backgroundMusicPlayer.volume = bgmSetting.musicVolume * bgmVolume;
        }
    }

    private void LoadVolumeValue()
    {
        if (!PlayerPrefs.HasKey("BGMVolume") || !PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("BGMVolume", 1f);
            PlayerPrefs.SetFloat("SFXVolume", 1f);
            PlayerPrefs.Save();
        }

        bgmVolume = PlayerPrefs.GetFloat("BGMVolume");
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
    }

    public void SaveVolumeValue()
    {
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }



    private AudioSource GetFreeAudioPlayer()
    {
        foreach (var player in audioPlayerPool)
        {
            if (!player.isPlaying)
                return player;
        }

        AudioSource newPlayer = gameObject.AddComponent<AudioSource>();
        audioPlayerPool.Add(newPlayer);
        return newPlayer;
    }

    /// <summary>
    /// ������Ч
    /// </summary>
    public void PlaySound(SoundType soundType)
    {
        if (!soundMap.TryGetValue(soundType, out var setting))
        {
            return;
        }
        

        if (DetectPlayingGetHit(soundType, setting) == false)
        {
            AudioSource player = GetFreeAudioPlayer();
            player.clip = setting.audioClip;
            player.volume = setting.soundVolume;
            player.Play();

            if (activeSoundMap.ContainsKey(player)) 
            {
                activeSoundMap[player] = setting;
            }
            else
            {
                activeSoundMap.Add(player, setting);
            }
                
        }
    }

    private bool DetectPlayingGetHit(SoundType soundType, SoundSetting setting)
    {
        if (soundType == SoundType.GetHit)
        {
            foreach (var player in audioPlayerPool)
            {
                if (player.isPlaying && player.clip == setting.audioClip)
                {
                    // �Ѿ��� GetHit ��Ч�ڲ��ţ��Ͳ����ظ�����
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// ���� BGM����ѡ����������
    /// </summary>
    public void PlayBGM(BGMType bgmType, bool fade = false, float fadeDuration = 1.5f)
    {
        if (!bgmMap.TryGetValue(bgmType, out var bgmSetting)) return;

        currentBGMSetting = bgmSetting;

        if (fade && backgroundMusicPlayer.isPlaying)
        {
            StartCoroutine(FadeAndSwitchBGM(bgmSetting.musicClip, bgmSetting.musicVolume, fadeDuration));
        }
        else
        {
            backgroundMusicPlayer.clip = bgmSetting.musicClip;
            backgroundMusicPlayer.volume = bgmSetting.musicVolume;
            backgroundMusicPlayer.loop = true;
            backgroundMusicPlayer.Play();
        }
    }

    /// <summary>
    /// ����������ǰ BGM ���л����µ�����
    /// </summary>
    private IEnumerator FadeAndSwitchBGM(AudioClip newClip, float newVolume, float duration)
    {
        float startVolume = backgroundMusicPlayer.volume;
        float timePassed = 0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            backgroundMusicPlayer.volume = Mathf.Lerp(startVolume, 0f, timePassed / duration);
            yield return null;
        }

        backgroundMusicPlayer.Stop();
        backgroundMusicPlayer.clip = newClip;
        backgroundMusicPlayer.volume = newVolume;
        backgroundMusicPlayer.loop = true;
        backgroundMusicPlayer.Play();
    }

    /// <summary>
    /// ֹͣ���� BGM
    /// </summary>
    public void StopBGM()
    {
        if (backgroundMusicPlayer != null)
            backgroundMusicPlayer.Stop();
    }
}
