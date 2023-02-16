using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguisherRose : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject highLight;
    public GameObject waterEffect1;
    public GameObject waterEffect2;

    bool isCoroutine;

    private void Start()
    {
        waterEffect1.SetActive(false);
        waterEffect2.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (highLight.activeInHierarchy)
        {
            waterEffect1.SetActive(true);
            waterEffect2.SetActive(true);
        }

        else
        {
            waterEffect1.SetActive(false);
            waterEffect2.SetActive(false);
        }
        
    }

    //IEnumerator ExtinguishEffect()
    //{
        
    //    isCoroutine = true;
    //    Debug.Log("coroutine");
    //    fireEffect.SetActive(true);

    //    yield return new WaitForSeconds(1);

    //    waterEffect.SetActive(true);
        

    //    yield return new WaitForSeconds(1f);
    //    fireEffect.SetActive(false);
    //    //isCoroutine = false;
    //}

}
