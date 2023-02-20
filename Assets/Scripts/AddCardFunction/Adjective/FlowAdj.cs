using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlendMode
{
    Opaque = 0,
    Cutout,
    Fade,
    Transparent
}

public class FlowAdj : IAdjective
{
    private EAdjective adjectiveName = EAdjective.Flow;
    private EAdjectiveType adjectiveType = EAdjectiveType.Normal;
    private int count = 0;

    // FlowAdj value
    private Material originMat;

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
        InteractionSequencer.GetInstance.CoroutineQueue.Enqueue(FlowObj(thisObject));
    }

    public void Execute(InteractiveObject thisObject, GameObject player)
    {
        //Debug.Log("Null : this Object -> Player");
    }

    public void Execute(InteractiveObject thisObject, InteractiveObject otherObject)
    {
        //Debug.Log("Null : this Object -> other Object");
        InteractionSequencer.GetInstance.CoroutineQueue.Enqueue(AbandonFlow(thisObject));
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
        
        ChangeMat(obj);
    }

    void ChangeMat(InteractiveObject obj)
    {
        switch (obj.transform.name)
        {
            case ("WaterObj(Clone)"):
                MeshRenderer mesh = obj.transform.GetComponentInChildren<MeshRenderer>();
                originMat = new Material(mesh.materials[0]);
                Material newMat = new Material(Shader.Find("Standard"));
                changeRenderMode(newMat, BlendMode.Fade);
                Texture tex = originMat.GetTexture("_TopTexture0");
                newMat.mainTexture = tex;
                Color color = newMat.color;
                color.a = 0.5f;
                newMat.color = color;
                mesh.material = newMat;
                break;
        }
    }

    IEnumerator AbandonFlow(InteractiveObject obj)
    {
        yield return null;
        obj.gameObject.layer = 0;
        obj.SubtractAdjective(EAdjective.Flow);
        obj.transform.GetComponent<MeshRenderer>().material = originMat;
    }

    public IAdjective DeepCopy()
    {
        return new FlowAdj();
    }

    public static void changeRenderMode(Material standardShaderMaterial, BlendMode blendMode)
    {
        switch (blendMode)
        {
            case BlendMode.Opaque:
                standardShaderMaterial.SetFloat("_Mode", 0.0f);
                standardShaderMaterial.SetOverrideTag("RenderType", "Opaque");
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                standardShaderMaterial.SetInt("_ZWrite", 1);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = -1;
                break;
            case BlendMode.Cutout:
                standardShaderMaterial.SetFloat("_Mode", 1.0f);
                standardShaderMaterial.SetOverrideTag("RenderType", "Opaque");
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                standardShaderMaterial.SetInt("_ZWrite", 1);
                standardShaderMaterial.EnableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 2450;
                break;
            case BlendMode.Fade:
                standardShaderMaterial.SetFloat("_Mode", 2.0f);
                standardShaderMaterial.SetOverrideTag("RenderType", "Transparent");
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                standardShaderMaterial.SetInt("_ZWrite", 0);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.EnableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 3000;
                break;
            case BlendMode.Transparent:
                standardShaderMaterial.SetFloat("_Mode", 3.0f);
                standardShaderMaterial.SetOverrideTag("RenderType", "Transparent");
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                standardShaderMaterial.SetInt("_ZWrite", 0);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 3000;
                break;
        }
    }
}
