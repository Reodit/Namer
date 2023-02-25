using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRoss : MonoBehaviour
{
    public GameObject highLight;
    public GameObject LightEffect;
    void Update()
    {
        if (highLight.activeInHierarchy)
        {
            LightEffect.SetActive(true);
        }

        else
        {
            LightEffect.SetActive(false);
        }
        
    }
}
