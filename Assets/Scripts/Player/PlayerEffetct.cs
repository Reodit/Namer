using System.Collections.Generic;
using UnityEngine;

public class PlayerEffetct : MonoBehaviour
{
    [SerializeField] private ParticleSystem footprint;
    private List<string> footprintClipsName;
    [SerializeField] private AudioClip[] footprintSoundClips;
    
    private void OnEnable()
    {
        Init();
    }

    void Init()
    {
        if (!footprint)
        {
            GameManager.GetInstance.localPlayerMovement.gameObject.transform.Find("FootPrint").Find("PlayerFootprint");
        }

        footprintClipsName = new List<string>();

        foreach (var e in footprintSoundClips)
        {
            footprintClipsName.Add(e.name);
        }
    }

    public void FootprintPlay()
    {
        footprint.Play();
    }

    public void FootprintSoundPlay()
    {
        int idx = UnityEngine.Random.Range(0, footprintClipsName.Count);
        SoundManager.GetInstance.Play(footprintClipsName[idx]);
    }
}
