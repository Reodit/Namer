using System.Collections.Generic;
using UnityEngine;

public class CollisionEffect : MonoBehaviour
{
    [field: SerializeField] public ParticleSystem WaterFootprint { get; private set; }
    public List<string> WaterFootprintClipsName { get; private set; }
    [field: SerializeField] public AudioClip[] WaterFootprintSoundClips { get; private set; }
    private PlayerEffetct pe;
    private PlayerMovement pm;
    private int layerMask;
    private void Start()
    {
        Init();

    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        InteractiveObject io;
        
        if (Physics.Raycast(pm.transform.position + Vector3.down * 0.2f, Vector3.up, out hit, 0.5f, layerMask))
        {
            if (hit.transform.TryGetComponent<InteractiveObject>(out io))
            {
                if (io.Adjectives[12] != null)
                {
                    pe.currentEffectIndex = EffectIndex.WaterFootprint;
                }
            }
        }
        
        else if (Physics.Raycast(pm.transform.position + Vector3.up * 0.2f, Vector3.down, out hit, 1f, layerMask))
        {
            if (hit.transform.name.Contains("GroundTile") || hit.transform.name.Contains("GlassTile"))
            {
                pe.currentEffectIndex = EffectIndex.DefaultFootprint;
            }

            else if (hit.transform.name.Contains("SnowTile"))
            {
                pe.currentEffectIndex = EffectIndex.SnowFootprint;
            }
            
            else if (hit.transform.name.Contains("SandTile"))
            {
                pe.currentEffectIndex = EffectIndex.DesertFootprint;
            }
            
            // TODO 오브젝트 밟았을 때 구름 나오게 하기
        }
    }

    void Init()
    {
        layerMask = ~(1 << LayerMask.NameToLayer("Player"));

        pm = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        pe = GameObject.FindWithTag("Player").GetComponent<PlayerEffetct>();

        WaterFootprintClipsName = new List<string>();

        foreach (var e in WaterFootprintSoundClips)
        {
            WaterFootprintClipsName.Add(e.name);
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        InteractiveObject io;

        if (other.TryGetComponent<InteractiveObject>(out io))
        {
            if (io.Adjectives[12] != null)
            {
                return;
            }
        }
        
        pm.rb.MovePosition(pm.rb.position - pm.pInputVector * pm.moveSpeed * Time.deltaTime);
        pm.rb.velocity = Vector3.zero;
    }
}
