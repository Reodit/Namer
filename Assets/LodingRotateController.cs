using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LodingRotateController : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 110f;

    private void FixedUpdate()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);    
    }
}
