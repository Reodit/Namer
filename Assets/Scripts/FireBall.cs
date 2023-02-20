using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed = 2.5f;
    public float rotationSpeed = 10;
    public float radius = 0.6f;
    public float amplitude = 0.6f;
    private float time = 0;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        // Calculate the x and y positions of the circle
        float x = Mathf.Cos(time * speed) * radius;
        //float y = Mathf.Sin(time * speed) * amplitude;
        float z = Mathf.Sin(time * speed) * amplitude;

        // Update the position of the circle
        transform.localPosition = new Vector3(x, 0, z);
        transform.parent.rotation = Quaternion.Euler(0, 0, time * rotationSpeed);
    }
}
