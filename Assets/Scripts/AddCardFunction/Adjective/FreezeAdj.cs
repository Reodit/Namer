using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeAdj : IAdjective
{
    private EAdjective adjectiveName = EAdjective.Freeze;
    private EAdjectiveType adjectiveType = EAdjectiveType.Normal;
    private int count = 0;

    #region Freeze변수

    private GameObject iceEffect;
    

    #endregion
    
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
        FindEffect(thisObject.gameObject);
        //Debug.Log("this is Null");
    }

    public void Execute(InteractiveObject thisObject, GameObject player)
    {
        //Debug.Log("Null : this Object -> Player");
    }
    
    public void Execute(InteractiveObject thisObject, InteractiveObject otherObject)
    {
        //Debug.Log("Null : this Object -> other Object");
    }
    
    public void Abandon(InteractiveObject thisObject)
    {
        iceEffect.GetComponentInChildren<ParticleSystem>().Stop();        
    }
    
    public IAdjective DeepCopy() 
    { 
           return new FreezeAdj();
    }

    void FindEffect(GameObject thisObject)
    {
        var freezeEffect = Resources.Load<GameObject>("Prefabs/Interaction/Effect/IceEffects");
        iceEffect=GameObject.Instantiate(freezeEffect, thisObject.transform);
        iceEffect.GetComponentInChildren<ParticleSystem>().Play();
        
    }
}
