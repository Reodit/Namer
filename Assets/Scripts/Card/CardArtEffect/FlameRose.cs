using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameRose : MonoBehaviour
{
    public float speed = 2f;
    public float radius = 0.5f;
    public float amplitude = 0.5f;
    private float time = 0;

    public GameObject highLight;
    
    public GameObject flameEffect;
    // Start is called before the first frame update
    void Start()
    {
        flameEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (highLight.activeInHierarchy)
        {
            flameEffect.SetActive(true);
            time += Time.deltaTime;

            // Calculate the x and y positions of the circle
            float x = Mathf.Cos(time * speed) * radius;
            float y = Mathf.Sin(time * speed) * amplitude;
            float z = Mathf.Sin(time * speed) * radius;

            // Update the position of the circle
            flameEffect.transform.localPosition = new Vector3(x, y, z);
        }
        else flameEffect.SetActive(false);
        
    }
}
