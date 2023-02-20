using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowAdj : IAdjective
{
    private EAdjective adjectiveName = EAdjective.Flow;
    private EAdjectiveType adjectiveType = EAdjectiveType.Normal;
    private int count = 0;

    private GameObject iceShardEffect;
    private ParticleSystem[] iceShardEffects;

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
        //Debug.Log("this is Null");
        //thisObject.gameObject.layer = 4;
        if (thisObject.transform.Find("iceShardEffect"))
        {
            GameObject.Destroy(thisObject.transform.Find("iceShardEffect").gameObject);
        }

        InteractionSequencer.GetInstance.CoroutineQueue.Enqueue(FlowObj(thisObject));
    }

    public void Execute(InteractiveObject thisObject, GameObject player)
    {
        //Debug.Log("Null : this Object -> Player");
    }
    
    public void Execute(InteractiveObject thisObject, InteractiveObject otherObject)
    {
        //Debug.Log("Null : this Object -> other Object");
        FindEffect(thisObject.gameObject);
        InteractionSequencer.GetInstance.SequentialQueue.Enqueue(FreezeObj(thisObject));
        
    }
    
    public void Abandon(InteractiveObject thisObject)
    {
        InteractionSequencer.GetInstance.CoroutineQueue.Enqueue(AbandonFlow(thisObject));
        //thisObject.gameObject.layer = 0;
    }

    IEnumerator FlowObj(InteractiveObject obj)
    {
        yield return null;
        obj.gameObject.layer = 4;

    }

    IEnumerator AbandonFlow(InteractiveObject obj)
    {
        yield return null;
        obj.gameObject.layer = 0;
        //obj.SubtractAdjective(EAdjective.Flow);
    }
    
    public IAdjective DeepCopy()
    {
        return new FlowAdj();
    }

    void FindEffect(GameObject thisObject)
    {
        
        //if (thisObject.transform.Find("iceShardEffect")) return;
        //Debug.Log("find");
        var freezeEffect = Resources.Load<GameObject>("Prefabs/Interaction/Effect/IceShardEffect");
        iceShardEffect = GameObject.Instantiate(freezeEffect, thisObject.transform);
        iceShardEffect.name = "iceShardEffect";
        iceShardEffects = iceShardEffect.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < iceShardEffects.Length; i++)
        {
            iceShardEffects[i].Stop();
        }
    }

    IEnumerator FreezeObj(InteractiveObject obj)
    {
        obj.gameObject.layer = 0;
        

        for (int i = 0; i < iceShardEffects.Length; i++)
        {
            iceShardEffects[i].Play();
        }

        yield return new WaitForSeconds(.5f);

        

        for (int i = 1; i < iceShardEffects.Length; i++)
        {
            iceShardEffects[i].Stop();
        }

        iceShardEffects[0].Pause();

        obj.SubtractAdjective(EAdjective.Flow);
        obj.SubtractAdjective(EAdjective.Extinguisher);
    }
}
