using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRose : MonoBehaviour
{
    public float speed = 2f;
    public float radius = 0.5f;
    public float amplitude = 0.5f;

    public GameObject highLight;

    public GameObject freezeEffect;
    // Start is called before the first frame update
    void Start()
    {
        freezeEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (highLight.activeInHierarchy)
        {
            freezeEffect.SetActive(true);
        }
        else freezeEffect.SetActive(false);

    }
}
