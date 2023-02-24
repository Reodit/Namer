using System;
using System.Collections;
using UnityEngine;

public class FlameAdj : IAdjective
{
    private EAdjective adjectiveName = EAdjective.Flame;
    private EAdjectiveType adjectiveType = EAdjectiveType.Contradict;
    private int count = 0;
    
    private GameObject sprayObj;
    [SerializeField] private GameObject testObj;
    private GameObject fireBallEffect;
    private ParticleSystem[] fireBall;
    
    public EAdjective GetAdjectiveName()
    {
        return adjectiveName;
    }

    public EAdjectiveType GetAdjectiveType()
    {
        return adjectiveType;
    }

    public int GetCount()
    {
        return count;
    }

    public void SetCount(int addCount)
    {
        this.count += addCount;
    }
    
    public void Execute(InteractiveObject thisObject)
    {
        ParticleSetting(thisObject);
        OnFire();
    }

    public void Execute(InteractiveObject thisObject, GameObject player)
    {
        GameManager.GetInstance.isPlayerDoAction = false;
    }
    
    public void Execute(InteractiveObject thisObject, InteractiveObject otherObject)
    {
        // Debug.Log("Flame : this Object -> other Object");
             // thisObject.SubtractAdjective(EAdjective.Flame);

         if(sprayObj == null)
             GetSpray(otherObject.gameObject);
         if (thisObject.CheckAdjective(EAdjective.Flame))
         {
             // make some effect particle? 
             // excute Extinguish
             //방향을 해야됨
             //spray 는 z방향으로 나간다.
             //그러니까 서로의 위치를 확인해서 그위치로 쏘게해야함.
             SetSprayDirection(otherObject.gameObject, thisObject.gameObject);
             //
             InteractionSequencer.GetInstance.SequentialQueue.Enqueue(Extinquish(thisObject,otherObject));
         }
    }

    #region FlameOut
    IEnumerator Extinquish(InteractiveObject thisObject, InteractiveObject otherObject)
    {
        int extinguishIdx = (int)EAdjective.Extinguisher;
        if(otherObject.Adjectives[extinguishIdx] == null) yield break;
        if (sprayObj == null) yield break;
        SoundManager.GetInstance.Play(adjectiveName,sprayObj.GetComponent<ParticleSystem>().main.duration);
        var duraion = (double)SoundManager.GetInstance.sfxSound.clip.samples /
                      SoundManager.GetInstance.sfxSound.clip.frequency;
        var mainModule =sprayObj.GetComponent<ParticleSystem>().main;
        mainModule.duration = (float)duraion;
        sprayObj.GetComponent<ParticleSystem>().Play();
        // SoundManager.GetInstance.SetEndDSPTime(sprayObj.GetComponent<ParticleSystem>().main.duration);
        yield return new WaitUntil(() => sprayObj == null || !sprayObj.GetComponent<ParticleSystem>().isPlaying);
        EradicateFlame(thisObject);
    }

    
    void GetSpray(GameObject otherObject)
    {
        var waterSprayPrefabs = Resources.Load<GameObject>("Prefabs/Interaction/Effect/WaterSpray");
        sprayObj=GameObject.Instantiate(waterSprayPrefabs,otherObject.transform);

        sprayObj.transform.position = new Vector3(sprayObj.transform.position.x,
            otherObject.transform.position.y + otherObject.transform.lossyScale.y * .5f, sprayObj.transform.position.z);
        // sprayObj.GetComponent<ParticleSystem>().Play();
    }
    void EradicateFlame(InteractiveObject thisObject)
    {
        thisObject.SubtractAdjectiveCard(EAdjective.Flame);
    }
    void SetSprayDirection(GameObject otherObject, GameObject thisObject)
    {
        var direction = thisObject.transform.position- otherObject.transform.position;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z) && direction.x > 0)
        {
            sprayObj.transform.Rotate(new Vector3(0f,90f,0f));
            // sprayObj.transform.rotation = Quaternion.Euler(new Vector3(0f,90f,0f));
        }
        else if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z) && direction.x < 0)
        {
            sprayObj.transform.Rotate(new Vector3(0f, -90f, 0f));
            // sprayObj.transform.rotation = Quaternion.Euler(new Vector3(0f,-90f,0f));
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.z) && direction.z > 0)
        {
            sprayObj.transform.Rotate(new Vector3(0f,0f,0f));
            // sprayObj.transform.rotation = Quaternion.Euler(new Vector3(0f,0f,0f));
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.z) && direction.z < 0)
        {
            sprayObj.transform.Rotate(new Vector3(180f,0f,0f));
            // sprayObj.transform.rotation = Quaternion.Euler(new Vector3(0f,0f,180f));
        }
    }

    

        #endregion
    
    public void Abandon(InteractiveObject thisObject)
    {
        if(sprayObj != null)
            GameObject.Destroy(sprayObj);
        if (fireBallEffect != null)
            GameObject.Destroy(fireBallEffect);
    }
    
    public IAdjective DeepCopy()
    {
        return new FlameAdj();
    }

    GameObject FindEffect(String prefabName)
    {
        string path = "Prefabs/Interaction/Effect/" + prefabName;
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null)
        {
            Debug.LogError("Prefab not found: " + prefabName);
        }

        return prefab;
    }
    
    private void ParticleSetting(InteractiveObject thisObject)
    {
        if (thisObject.transform.Find("FlameEffect")) return;

        GameObject effect = GameObject.Instantiate( FindEffect("FlameEffect"), thisObject.transform);
        effect.name = "FlameEffect";
        fireBallEffect = effect;

        fireBall = thisObject.gameObject.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < fireBall.Length; i++)
        {
            fireBall[i].Stop();
        }
    }

    private void OnFire()
    {
        for (int i = 0; i < fireBall.Length; i++)
        {
            fireBall[i].Play();
        }
    }
}
