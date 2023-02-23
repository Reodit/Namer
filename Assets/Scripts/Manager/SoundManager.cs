using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SoundManager : Singleton<SoundManager>
{
    public AudioMixer audioMixer;
    public AudioSource bgmSound;
    [SerializeField] private AudioSource newAudioSource;
    // public AudioSource bGMSoundTrack02;
    // private Stack<AudioSource> bgmTracks = new Stack<AudioSource>();
    public AudioSource curBGMSoundTrack;

    public AudioSource sfxSound;
    private bool isBGMSOundTrack01Playing;

    private double dspStartTime;
    private double dspEndTime;

    public Slider masterSlider;
    public Slider BGMSlider;
    public Slider SFXSlider;
    public Toggle muteToggle;

    // public List<AudioClip> effectClips = new List<AudioClip> ();
    /*
     * 1. enum -> main, credit , ....
     * 2. seting
     * 3. when scene has changed  -> change bgmSound audioclip
     * 4. ........
     */
    private Dictionary<EAdjective, AudioClip> effectClips = new Dictionary<EAdjective, AudioClip>();

    private Dictionary<string, AudioClip> uiEffectClips = new Dictionary<string, AudioClip>();

    private AudioClip[] bgmClips;


    public bool isMuteToggleOn;
    public bool isBgToggleOn;
    bool isStop;

    SGameSetting sGameSetting = new SGameSetting();


    private void Awake()
    {
        SetObjectSFXClips();
        SetUISFXClips();
        SetBGM();
    }

    private void Start()
    {
        if (GameDataManager.GetInstance.UserDataDic[GameManager.GetInstance.userId].gameSetting.isMute)
        {
            bgmSound.mute = !bgmSound.mute;
            // MuteBgm();
            sfxSound.mute = !sfxSound.mute;

        }
        isMuteToggleOn = bgmSound.mute;
        isBgToggleOn = bgmSound.mute;
    }

    [ContextMenu("SetObjSFXClips")]
    public void SetObjectSFXClips()
    {
        var audioClips = Resources.LoadAll<AudioClip>("Prefabs/Interaction/ObjectSoundEffect");
        for (int i = 0; i < audioClips.Length; i++)
        {
            var idx = audioClips[i].name.IndexOf('_');
            var eadjNum = int.Parse(audioClips[i].name.Substring(0, idx));
            // Debug.Log(eadjNum);
            effectClips.Add((EAdjective)eadjNum, audioClips[i]);
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

    private void SetBGM()
    {
        bgmClips = Resources.LoadAll<AudioClip>("BGM");

        // Debug.Log(bgmClips.Length);
        foreach (var clip in bgmClips)
        {
            // Debug.Log(clip.name);
        }
    }

    AudioClip FindBgm(string BGMName)
    {
        foreach (var clip in bgmClips)
        {
            if (clip.name == BGMName)
            {
                return clip;
            }
        }
        return null;
    }

    void MuteBgm()
    {
        if (isBGMSOundTrack01Playing)
        {
            bgmSound.mute = !bgmSound.mute;
            isMuteToggleOn = bgmSound.mute;
            isBgToggleOn = bgmSound.mute;

        }
        else
        {
            bgmSound.mute = !bgmSound.mute;
            isMuteToggleOn = bgmSound.mute;
            isBgToggleOn = bgmSound.mute;
        }


    }
    public void ChangeMainBGM(MainMenuState _state)
    {
        switch (_state)
        {
            case MainMenuState.Main:
            case MainMenuState.Level:
            case MainMenuState.Title:
                BgmPlay(FindBgm("Main"));
                break;
            case MainMenuState.Credit:
                BgmPlay(FindBgm("Credit"));
                break;
            case MainMenuState.Encyclopedia:
                BgmPlay(FindBgm("Encyclopedia"));
                break;
            default:
                break;
        }
    }

    public void ChangeInGameLevelBGM()
    {
        int currentLevel = GameManager.GetInstance.Level + 1;
        string levelBGM = "Level" + currentLevel;
        var audioClip = FindBgm(levelBGM);
        bgmSound.clip = audioClip;
        bgmSound.loop = true;
        bgmSound.Play();
    }



    public void BgmPlay()
    {
        bgmSound.loop = true;
        bgmSound.volume = 1;
        bgmSound.Play();
    }

    public void BgmPlay(AudioClip clip)
    {
        // Debug.Log(clip);
        // Debug.Log(bgmSound.clip);
        if (bgmSound.clip == clip) return;

        // StartCoroutine(SmothelySwapAudio(clip));
        bgmSound.clip = clip;
        bgmSound.Play();
    }
    IEnumerator SmothelySwapAudio(AudioClip newClip)
    {
        float timeToFade = .25f;
        float timeElapsed = 0;
        newAudioSource.clip = newClip;
        // bgmTracks.Push(newAudioSource);
        newAudioSource.volume = 0f;
        newAudioSource.Play();
        while (timeElapsed < timeToFade)
        {
            newAudioSource.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
            bgmSound.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        bgmSound.Stop();
        // Debug.Log("--------------------");
        // Debug.Log(bgmSound.clip);
        bgmSound = newAudioSource;
        newAudioSource = bgmSound;
        // Debug.Log(bgmSound.clip);
        // if (isBGMSOundTrack01Playing)
        // {
        //     bGMSoundTrack02.clip = newClip;
        //     bGMSoundTrack02.volume = 0f;
        //     bGMSoundTrack02.Play();
        //     while (timeElapsed < timeToFade)
        //     {
        //         bGMSoundTrack02.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
        //         bGMSoundTrack01.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
        //         timeElapsed += Time.deltaTime;
        //         yield return null;
        //     }
        //     bGMSoundTrack01.Stop();
        // }
        // else
        // {
        //     bGMSoundTrack01.clip = newClip;
        //     bGMSoundTrack01.volume = 0f;
        //     bGMSoundTrack01.Play();
        //     while (timeElapsed < timeToFade)
        //     {
        //         bGMSoundTrack01.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
        //         bGMSoundTrack02.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
        //         timeElapsed += Time.deltaTime;
        //         yield return null;
        //     }
        //     bGMSoundTrack02.Stop();
        //     
        // }
    }

    void BGMChange(Scene scene)
    {
        if (scene.name == "LoadingScene") return;
        // bgmSound = 
    }

    public void Play(AudioClip clip, double time)
    {
        double duration = (double)clip.samples / clip.frequency; //  clips playTime need to get  
        sfxSound.Stop();
        sfxSound.clip = clip;
        sfxSound.volume = 1f;
        dspStartTime = AudioSettings.dspTime;
        sfxSound.PlayScheduled(dspStartTime);
        
        SetEndDSPTime(time);
        if (duration > dspEndTime-dspStartTime)
        {
            StartCoroutine(AudioFadeOut(sfxSound));
            // Debug.Log(duration);
            // Debug.Log(dspEndTime);
            // sfxSound.PlayScheduled(dspStartTime+duration);
        }


        // if(AudioSettings.dspTime < duration)
        //     sfxSound.PlayScheduled(AudioSettings.dspTime + duration);
        // sfxSound.PlayOneShot(clip);
    }

    IEnumerator AudioFadeOut(AudioSource audio)
    {
        double remainingTime = dspEndTime-AudioSettings.dspTime;
        float elapsedTime = 0;
        while (remainingTime > 0)
        {
            audio.volume = Mathf.Lerp(1, 0, (float)(dspEndTime-AudioSettings.dspTime/remainingTime));
            remainingTime -= Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    public void Play(AudioClip clip)
    {
        double duration = (double)clip.samples / clip.frequency; //  clips playTime need to get  
        sfxSound.Stop();
        sfxSound.clip = clip;
        dspStartTime = AudioSettings.dspTime;
        sfxSound.PlayScheduled(dspStartTime);
        // SetEndDSPTime(time);
        // if (dspEndTime > duration)
        // {
        //     sfxSound.PlayScheduled(dspStartTime+duration);
        // }
        // if(AudioSettings.dspTime < duration)
        //     sfxSound.PlayScheduled(AudioSettings.dspTime + duration);
        // sfxSound.PlayOneShot(clip);
    }

    public void Pause(AudioClip clip)
    {
        sfxSound.Pause();
    }

    void SetSFXEndTime(double despEndTime)
    {
        sfxSound.SetScheduledEndTime(despEndTime);
    }

    private void SetEndDSPTime(double time)
    {
        dspEndTime = dspStartTime + time;
        SetSFXEndTime(dspEndTime);
    }

    public void Play(EAdjective eAdjective)
    {
        if (effectClips.ContainsKey(eAdjective))
            Play(effectClips[eAdjective]);
    }
    public void Play(EAdjective eAdjective, double playTime)
    {
        if (effectClips.ContainsKey(eAdjective))
            Play(effectClips[eAdjective], playTime);
    }

    public void Play(string clipname)
    {
        if (uiEffectClips.ContainsKey(clipname))
        {
            Play(uiEffectClips[clipname]);
        }
    }


    public void Pause(string clipName)
    {
        if (uiEffectClips.ContainsKey(clipName))
        {
            Pause(uiEffectClips[clipName]);
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
        } else
        {
            muteToggle =
            GameObject.Find("IngameCanvas").transform.
            Find("OptionPanel").transform.
            Find("SoundPanel").transform.
            Find("MutePanel").transform.
            GetChild(0).GetComponent<Toggle>();
        }
        muteToggle.onValueChanged.AddListener(delegate {
            SetSound(); });

    }

    private void OnApplicationPause(bool pause)
    {
        if (!isBgToggleOn) return;
        if (pause)
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
    #region test
    public string testClipName;

    [ContextMenu("PauseTest")]
    public void TestPause()
    {
        Pause(testClipName);
    }
    #endregion


}
