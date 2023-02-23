using System;
using System.Collections.Generic;
using UnityEngine;

public enum EffectIndex
{
    WaterFootprint = 0, 
    Footprint
}

public class PlayerEffetct : MonoBehaviour
{
    [SerializeField] private ParticleSystem footprint;
    private List<string> footprintClipsName;
    [SerializeField] private AudioClip[] footprintSoundClips;
    public EffectIndex currentEffectIndex;
    private CollisionEffect ce;
    [SerializeField] private ParticleSystem climbEffect;
    [SerializeField] private ParticleSystem pushEffect;
    
    private void Start()
    {
        Init();
    }

    void Init()
    {
        ce = transform.Find("CollisionEffect").GetComponent<CollisionEffect>();
        
        footprintClipsName = new List<string>();
        currentEffectIndex = EffectIndex.Footprint;
        foreach (var e in footprintSoundClips)
        {
            footprintClipsName.Add(e.name);
        }
    }

    #region Animation Event Function
    public void FootprintPlay()
    {
        switch (currentEffectIndex)
        { 
            case EffectIndex.Footprint:
                footprint.Play();
                break;
            case EffectIndex.WaterFootprint:
                ce.WaterFootprint.Play();
                break;
        }
    }
    
    // 복수의 오디오 소스 필요 (근데 확인해 봐야함)
    public void FootprintSoundPlay()
    {
        int randomClipIdx;
        switch (currentEffectIndex)
        {
            case EffectIndex.Footprint:
                randomClipIdx = UnityEngine.Random.Range(0, footprintClipsName.Count);
                SoundManager.GetInstance.repeatPlay(footprintClipsName[randomClipIdx]);
                break;
            case EffectIndex.WaterFootprint:
                randomClipIdx = UnityEngine.Random.Range(0, ce.WaterFootprintClipsName.Count);
                SoundManager.GetInstance.repeatPlay(ce.WaterFootprintClipsName[randomClipIdx]);
                break;
        }
        
    }

    public void ClimbEffectPlay()
    {
        climbEffect.Play();
    }

    public void PushEffectPlay()
    {
        pushEffect.Play();
    }
    #endregion
}
