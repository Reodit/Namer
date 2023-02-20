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
    private bool isFreezing;
    
    public float speed = 2.5f;
    public float rotationSpeed = 10;
    public float radius = 0.6f;
    public float amplitude = 0.6f;
    private float time = 0;

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
        isFreezing = true;
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
        // var freezeEffect = Resources.Load<GameObject>("Prefabs/Interaction/Effect/IceEffects");
        // var freezeEffect = Resources.Load<GameObject>("Prefabs/Interaction/Effect/IceEffect2");
        var freezeEffect = Resources.Load<GameObject>("Prefabs/Interaction/Effect/FreezeEffect");
        // var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //
        // sphere.transform.SetParent(thisObject.transform);
        // sphere.transform.localPosition = new Vector3(0f, .5f, 0f);
        // sphere.GetComponent<Collider>().isTrigger = true;
        // sphere.GetComponent<MeshRenderer>().enabled = false;
        //
        
        // iceEffect=GameObject.Instantiate(freezeEffect, sphere.transform);
        iceEffect = GameObject.Instantiate(freezeEffect, thisObject.transform);
        iceEffect.transform.localPosition = Vector3.zero;
        // iceEffect.transform.localPosition = new Vector3(.5f, 0f,.5f );   //요넘 
        iceEffect.GetComponentInChildren<ParticleSystem>().Play();
        // InteractionSequencer.GetInstance.CoroutineQueue.Enqueue(RotateAroundEffect(thisObject,sphere));
        
    }

    IEnumerator RotateAroundEffect(GameObject thisObject, GameObject sphere)
    {
        while (isFreezing)
        {
            //  time += Time.deltaTime;
            // // sphere.transform.Rotate(Quaternion.Euler(new Vector3(0f,0f,1f)).eulerAngles,1f);
            // sphere.transform.rotation = Quaternion.Euler(0,0,time );
            //
            // iceEffect.transform.RotateAround(thisObject.transform.position,Vector3.up, 90f*Time.deltaTime); //요건데
            time += Time.deltaTime;
            // // Calculate the x and y positions of the circle
            float x = Mathf.Cos(time * speed) * radius;
            float y = Mathf.Sin(time * speed) * amplitude;
            float z = Mathf.Sin(time * speed) * amplitude;
            // // Update the position of the circle
            iceEffect.transform.localPosition = new Vector3(x, 0, z);
            sphere.transform.rotation = Quaternion.Euler(0, 0, time * rotationSpeed);
            
            yield return null;
        }

        yield return null;
    }
}
