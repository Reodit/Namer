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
    
    private void Start()
    {
        Init();
    }

    void Init()
    {
        if (!footprint)
        {
            GameManager.GetInstance.localPlayerMovement.gameObject.transform.Find("FootPrint").Find("PlayerFootprint");
        }

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
        footprint.Play();
    }
    
    public void FootprintSoundPlay()
    {
        int randomClipIdx;
        switch (currentEffectIndex)
        {
            case EffectIndex.Footprint:
                randomClipIdx = UnityEngine.Random.Range(0, footprintClipsName.Count);
                SoundManager.GetInstance.Play(footprintClipsName[randomClipIdx]);
                break;
            case EffectIndex.WaterFootprint:
                randomClipIdx = UnityEngine.Random.Range(0, ce.WaterFootprintClipsName.Count);
                SoundManager.GetInstance.Play(ce.WaterFootprintClipsName[randomClipIdx]);
                break;
        }
        
    }
    #endregion
}
