using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class CollisionEffect : MonoBehaviour
{
    [field: SerializeField] public ParticleSystem WaterFootprint { get; private set; }
    public List<string> WaterFootprintClipsName { get; private set; }
    [field: SerializeField] public AudioClip[] WaterFootprintSoundClips { get; private set; }
    private PlayerEffetct pe;
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, 20f))
        {
            if (hit.transform.name.Contains("WaterObj"))
            {
                pe.currentEffectIndex = EffectIndex.WaterFootprint;
            }

            return;
        }
        
        if (Physics.Raycast(transform.position, Vector3.down, out hit,20f))
        {
            if (hit.transform.name.Contains("GlassTile") || hit.transform.name.Contains("GroundTile"))
            {
                pe.currentEffectIndex = EffectIndex.Footprint;
            }
        }
    }

    void Init()
    {
        // if (!waterFootprint)
        // {    
        //     GameManager.GetInstance.localPlayerMovement.gameObject.transform.Find("CollisionEffect").Find("WaterFootprint");
        // }
        pe = GameObject.FindWithTag("Player").GetComponent<PlayerEffetct>();
        
        WaterFootprintClipsName = new List<string>();

        foreach (var e in WaterFootprintSoundClips)
        {
            WaterFootprintClipsName.Add(e.name);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("WaterObj"))
        {
            pe.currentEffectIndex = EffectIndex.WaterFootprint;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("WaterObj"))
        {
            pe.currentEffectIndex = EffectIndex.Footprint;
        }    
    }
}
