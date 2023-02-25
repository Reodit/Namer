using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlendMode
{
    Back = 0,
    Custom
}

public class FlowAdj : IAdjective
{
    private EAdjective adjectiveName = EAdjective.Flow;
    private EAdjectiveType adjectiveType = EAdjectiveType.Normal;
    private int count = 0;

    private GameObject iceShardEffect;
    private ParticleSystem[] iceShardEffects;

    private Material originMat;
    private float flowAlpha = 0.4f;

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
        FindEffect(thisObject.gameObject);
        InteractionSequencer.GetInstance.SequentialQueue.Enqueue(FreezeObj(thisObject));
    }

    public void Abandon(InteractiveObject thisObject)
    {
        InteractionSequencer.GetInstance.CoroutineQueue.Enqueue(AbandonFlow(thisObject));
    }

    IEnumerator FlowObj(InteractiveObject obj)
    {
        yield return null;
        obj.gameObject.layer = 4;

        ChangeMat(obj);
    }

    void ChangeStandardMatAlpha(Material standardMaterial, float percent)
    {
        changeRenderMode(standardMaterial, percent == 1f ? BlendMode.Back : BlendMode.Custom);
        Color color = standardMaterial.color;
        color.a = percent;
        standardMaterial.color = color;
    }

    void ChangeMat(InteractiveObject obj)
    {
        switch (obj.transform.name)
        {
            case ("WaterObj(Clone)"):
                MeshRenderer mesh = obj.transform.GetComponentInChildren<MeshRenderer>();
                originMat = new Material(mesh.materials[0]);
                Material newMat = new Material(GameManager.GetInstance.flowShader);
                Texture tex = originMat.GetTexture("_TopTexture0");
                newMat.mainTexture = tex;
                Color color = newMat.color;
                color = Color.white;
                color.a = flowAlpha;
                newMat.color = color;
                mesh.material = newMat;
                break;
            case ("RoseObj(Clone)"):
                MeshRenderer[] roseMeshes = obj.transform.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer curMesh in roseMeshes)
                {
                    foreach (Material mat in curMesh.materials)
                    {
                        ChangeStandardMatAlpha(mat, flowAlpha);
                    }
                }
                break;
            default:
                MeshRenderer[] meshes = obj.transform.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer curMesh in meshes)
                {
                    ChangeStandardMatAlpha(curMesh.material, flowAlpha);
                }
                break;
        }
    }

    void RepairMat(InteractiveObject obj)
    {
        switch (obj.transform.name)
        {
            case ("WaterObj(Clone)"):
                obj.transform.GetComponentInChildren<MeshRenderer>().material = originMat;
                break;
            case ("RoseObj(Clone)"):
                MeshRenderer[] roseMeshes = obj.transform.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer curMesh in roseMeshes)
                {
                    foreach (Material mat in curMesh.materials)
                    {
                        ChangeStandardMatAlpha(mat, 1f);
                    }
                }
                break;
            default:
                MeshRenderer[] meshes = obj.transform.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer curMesh in meshes)
                {
                    ChangeStandardMatAlpha(curMesh.material, 1f);
                }
                break;
        }
    }

    IEnumerator AbandonFlow(InteractiveObject obj)
    {
        SoundManager.GetInstance.Play(adjectiveName);
        yield return null;
        obj.gameObject.layer = 0;
        obj.SubtractAdjectiveCard(EAdjective.Flow);
        RepairMat(obj);
    }

    public IAdjective DeepCopy()
    {
        return new FlowAdj();
    }

    void FindEffect(GameObject thisObject)
    {
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

        obj.SubtractAdjectiveCard(EAdjective.Flow);
        obj.SubtractAdjectiveCard(EAdjective.Extinguisher);
    }

    public static void changeRenderMode(Material standardShaderMaterial, BlendMode mode)
    {
        switch (mode)
        {
            case (BlendMode.Custom):
                standardShaderMaterial.shader = GameManager.GetInstance.flowShader; ;
                break;
            case (BlendMode.Back):
            default:
                standardShaderMaterial.shader = GameManager.GetInstance.flowShader;
                break;
        }
    }
}
