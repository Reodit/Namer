using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum EffectIndex
{
    WaterFootprint = 0, 
    DefaultFootprint,
    SnowFootprint,
    DesertFootprint
}

public class PlayerEffetct : MonoBehaviour
{
    #region Footprints
    [SerializeField] private ParticleSystem glassFootprint;
    private List<string> glassFootprintClipsName;
    [SerializeField] private AudioClip[] glassFootprintSoundClips;
    
    [SerializeField] private ParticleSystem snowFootprint;
    private List<string> snowFootprintClipsName;
    [SerializeField] private AudioClip[] snowFootprintSoundClips;
    
    [SerializeField] private ParticleSystem desertFootprint;
    private List<string> desertFootprintClipsName;
    [SerializeField] private AudioClip[] desertFootprintSoundClips;
    #endregion
    
    public EffectIndex currentEffectIndex;
    private CollisionEffect ce;

    #region PlayerEffects
    [SerializeField] private ParticleSystem climbEffect;
    [SerializeField] private ParticleSystem pushEffect;
    #endregion

    #region Fade-out
    [SerializeField] private int fadeOutSpeed = 1;
    private Vector3 originalScale = Vector3.one;
    [SerializeField] private AudioClip fadeoutSound;
    #endregion
    
    private void Start()
    {
        Init();
        originalScale = transform.localScale;
    }
    
    public IEnumerator Fadeout()
    {
        climbEffect.Play();
        SoundManager.GetInstance.repeatPlay(fadeoutSound.name);
        float alpha = 0f;
        
        while (alpha < 0.5f)
        {
            alpha += Time.deltaTime * fadeOutSpeed;
            transform.localScale = originalScale * (1 - alpha);
            yield return null;
        }
        
        yield return new WaitForSeconds(0.2f);
    }

    private void Init()
    {
        ce = transform.Find("CollisionEffect").GetComponent<CollisionEffect>();
        currentEffectIndex = EffectIndex.DefaultFootprint;

        InitFootprintEffect(out glassFootprintClipsName, glassFootprintSoundClips);
        InitFootprintEffect(out snowFootprintClipsName, snowFootprintSoundClips);
        InitFootprintEffect(out desertFootprintClipsName, desertFootprintSoundClips);
    }

    void InitFootprintEffect(out List<string> footprintClipsName, AudioClip[] footparintClips)
    {
        footprintClipsName = new List<string>();

        if (footparintClips.Length < 1)
        {
            Debug.LogError("등록된 발소리 리소스가 없습니다!");
        }

        foreach (var e in footparintClips)
        {
            if (e)
            {
                footprintClipsName.Add(e.name);
            }
        }
    }

    #region Animation Event Function
    public void FootprintPlay()
    {
        switch (currentEffectIndex)
        { 
            case EffectIndex.DefaultFootprint:
                glassFootprint.Play();
                break;
            case EffectIndex.WaterFootprint:
                ce.WaterFootprint.Play();
                break;
            case EffectIndex.SnowFootprint:
                snowFootprint.Play();
                break;
            case EffectIndex.DesertFootprint:
                desertFootprint.Play();
                break;
        }
    }
    
    // 복수의 오디오 소스 필요 (근데 확인해 봐야함)
    public void FootprintSoundPlay()
    {
        int randomClipIdx;
        switch (currentEffectIndex)
        {
            case EffectIndex.DefaultFootprint:
                randomClipIdx = UnityEngine.Random.Range(0, glassFootprintClipsName.Count);
                SoundManager.GetInstance.repeatPlay(glassFootprintClipsName[randomClipIdx]);
                break;
            case EffectIndex.WaterFootprint:
                randomClipIdx = UnityEngine.Random.Range(0, ce.WaterFootprintClipsName.Count);
                SoundManager.GetInstance.repeatPlay(ce.WaterFootprintClipsName[randomClipIdx]);
                break;
            case EffectIndex.SnowFootprint:
                randomClipIdx = UnityEngine.Random.Range(0, snowFootprintClipsName.Count);
                SoundManager.GetInstance.repeatPlay(snowFootprintClipsName[randomClipIdx]);
                break;
            case EffectIndex.DesertFootprint:
                randomClipIdx = UnityEngine.Random.Range(0, desertFootprintClipsName.Count);
                SoundManager.GetInstance.repeatPlay(desertFootprintClipsName[randomClipIdx]);
                break;
        }
    }

    public void PushEffectPlay()
    {
        pushEffect.Play();
    }
    
    #endregion
}
