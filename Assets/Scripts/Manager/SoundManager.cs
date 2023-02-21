using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    public AudioMixer audioMixer; 
    public AudioSource bgmSound;
    public AudioSource sfxSound;

    public Slider masterSlider;
    public Slider BGMSlider;
    public Slider SFXSlider;
    public Toggle muteToggle;
    public Toggle bgToggle;

    // public List<AudioClip> effectClips = new List<AudioClip> ();
    private Dictionary<EAdjective, AudioClip> effectClips = new Dictionary<EAdjective, AudioClip>();
    private Dictionary<string, AudioClip> uiEffectClips = new Dictionary<string, AudioClip>();
    public List<AudioClip> bgmClips = new List<AudioClip> ();

    public bool isMuteToggleOn;
    public bool isBgToggleOn;
    bool isStop;

    SGameSetting sGameSetting = new SGameSetting();


    private void Awake()
    {
        BgmPlay();
        SetObjectSFXClips();
        SetUISFXClips();
    }

    private void Start()
    {
        if (GameDataManager.GetInstance.UserDataDic[GameManager.GetInstance.userId].gameSetting.isMute)
        {
            bgmSound.mute = !bgmSound.mute;
            sfxSound.mute = !sfxSound.mute;

        }
        isMuteToggleOn = bgmSound.mute;
        isBgToggleOn = bgmSound.mute;
    }

    [ContextMenu("SetObjSFXClips")]
    public void SetObjectSFXClips()
    {
        var audioClips =  Resources.LoadAll<AudioClip>("Prefabs/Interaction/ObjectSoundEffect");
        for (int i = 0; i < audioClips.Length; i++)
        {
            var idx = audioClips[i].name.IndexOf('_');
            var eadjNum = int.Parse(audioClips[i].name.Substring(0, idx));
            // Debug.Log(eadjNum);
            effectClips.Add((EAdjective)eadjNum,audioClips[i]);
            // Debug.Log(effectClips[(EAdjective)eadjNum].name);
        }
    }

    public void SetUISFXClips()
    {
        AudioClip[] uiAudioClips = Resources.LoadAll<AudioClip>("Prefabs/Interaction/UISoundEffect");

        for (int i = 0; i < uiAudioClips.Length; i++)
        {
            uiEffectClips.Add(uiAudioClips[i].name, uiAudioClips[i]);
        }
    }

    public void BgmPlay()
    {
        bgmSound.loop = true;
        bgmSound.volume = 1;
        bgmSound.Play(); 
    }

    public void Play(AudioClip clip)
    {
        if (sfxSound.isPlaying) return;
        //한번만 실행되면 그 다음은 실행 안되게 bool?

        sfxSound.PlayOneShot(clip);
    }

    public void Play(EAdjective eAdjective)
    {
        if(effectClips.ContainsKey(eAdjective))
            Play(effectClips[eAdjective]);
    }

    public void Play(string clipname)
    {

        if (uiEffectClips.ContainsKey(clipname))
        {
            Play(uiEffectClips[clipname]);
        }
    }

    public void SetMasterVolume()
    {
        FindSlider();
        audioMixer.SetFloat("Master", Mathf.Log10(masterSlider.value) * 20);
        sGameSetting = GameDataManager.GetInstance.UserDataDic[GameManager.GetInstance.userId].gameSetting;
        sGameSetting.volume = masterSlider.value;
        GameDataManager.GetInstance.SetGameSetting(sGameSetting);
    }

    public void SetBgmVolume()
    {
        FindSlider();
        audioMixer.SetFloat("BGM", Mathf.Log10(BGMSlider.value) * 20);
        sGameSetting = GameDataManager.GetInstance.UserDataDic[GameManager.GetInstance.userId].gameSetting;
        sGameSetting.backgroundVolume = BGMSlider.value;
        GameDataManager.GetInstance.SetGameSetting(sGameSetting);
    }   
    
    public void SetSfxVolume()
    {
        FindSlider();
        audioMixer.SetFloat("SFX", Mathf.Log10(SFXSlider.value) * 20);
        sGameSetting = GameDataManager.GetInstance.UserDataDic[GameManager.GetInstance.userId].gameSetting;
        sGameSetting.soundEffects = SFXSlider.value;
        GameDataManager.GetInstance.SetGameSetting(sGameSetting);
    }

    public void SetSound()
    {
        isMuteToggleOn = !isMuteToggleOn;
        bgmSound.mute = !bgmSound.mute;
        sfxSound.mute = !sfxSound.mute;
        sGameSetting = GameDataManager.GetInstance.UserDataDic[GameManager.GetInstance.userId].gameSetting;
        sGameSetting.isMute = bgmSound.mute;
        GameDataManager.GetInstance.SetGameSetting(sGameSetting);
    }

    public void SetBgMuteToggle()
    {
        isBgToggleOn = !isBgToggleOn;
        sGameSetting = GameDataManager.GetInstance.UserDataDic[GameManager.GetInstance.userId].gameSetting;
        sGameSetting.isMuteInBackground = isBgToggleOn;
        GameDataManager.GetInstance.SetGameSetting(sGameSetting);
    }

    public void FindToggle()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            muteToggle =
            GameObject.Find("MainCanvas").transform.
            Find("OptionPanel").transform.
            Find("SoundPanel").transform.
            Find("MutePanel").transform.
            GetChild(0).GetComponent<Toggle>();
            bgToggle =
            GameObject.Find("MainCanvas").transform.
            Find("OptionPanel").transform.
            Find("SoundPanel").transform.
            Find("MutePanel").transform.
            GetChild(1).GetComponent<Toggle>();
        } else
        {
            muteToggle =
            GameObject.Find("IngameCanvas").transform.
            Find("OptionPanel").transform.
            Find("SoundPanel").transform.
            Find("MutePanel").transform.
            GetChild(0).GetComponent<Toggle>();
            bgToggle = 
            GameObject.Find("IngameCanvas").transform.
            Find("OptionPanel").transform.
            Find("SoundPanel").transform.
            Find("MutePanel").transform.
            GetChild(0).GetComponent<Toggle>();
        }
            isBgToggleOn = bgToggle.isOn;
            muteToggle.onValueChanged.AddListener(delegate {
                SetSound(); });
            bgToggle.onValueChanged.AddListener(delegate {
                SetBgMuteToggle(); ;
            });
    }

    private void OnApplicationPause(bool pause)
    {
        if (!isBgToggleOn) return;
        if (pause)
        {
            if(!isMuteToggleOn)
            {
                bgmSound.mute = !bgmSound.mute;
                sfxSound.mute = !sfxSound.mute;
            }
            else
            {
                return;
            }
        }
        else
        {
            if (!isMuteToggleOn)
            {
                bgmSound.mute = !bgmSound.mute;
                sfxSound.mute = !sfxSound.mute;
            }
            else
            {
                return;
            }
        }
    }

    void FindSlider()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            masterSlider =
                GameObject.Find("MainCanvas").transform.
                Find("OptionPanel").transform.
                Find("SoundPanel").transform.
                Find("FullVolume Panel").transform.
                GetChild(1).GetComponent<Slider>();

            BGMSlider =
                GameObject.Find("MainCanvas").transform.
                Find("OptionPanel").transform.
                Find("SoundPanel").transform.
                Find("BGMVolume Panel").transform.
                GetChild(1).GetComponent<Slider>();

            SFXSlider =
                GameObject.Find("MainCanvas").transform.
                Find("OptionPanel").transform.
                Find("SoundPanel").transform.
                Find("SfxVolume Panel").transform.
                GetChild(1).GetComponent<Slider>();
        }
        else
        {
            masterSlider =
                 GameObject.Find("IngameCanvas").transform.
                 Find("OptionPanel").transform.
                 Find("SoundPanel").transform.
                 Find("FullVolume Panel").transform.
                 GetChild(1).GetComponent<Slider>();

            BGMSlider =
                GameObject.Find("IngameCanvas").transform.
                Find("OptionPanel").transform.
                Find("SoundPanel").transform.
                Find("BGMVolume Panel").transform.
                GetChild(1).GetComponent<Slider>();

            SFXSlider =
                GameObject.Find("IngameCanvas").transform.
                Find("OptionPanel").transform.
                Find("SoundPanel").transform.
                Find("SfxVolume Panel").transform.
                GetChild(1).GetComponent<Slider>();
        }
    }
}
