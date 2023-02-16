using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinRose : MonoBehaviour
{
    [SerializeField] GameObject fireWork;
    [SerializeField] GameObject highLight;
    float fadeInTime = 0.5f;

    bool isFadIn;

    private void Update()
    {

        if (highLight.activeInHierarchy && !isFadIn)
        {
            StartCoroutine(FillIn());
        }
        else if (!highLight.activeInHierarchy)
        {
            isFadIn = false;
            StopCoroutine(FillIn());
            fireWork.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    IEnumerator FillIn()
    {
        fireWork.transform.localScale = new Vector3(0, 0, 0);

        isFadIn = true;
        while (fireWork.transform.localScale.x < 1f)
        {
            fireWork.transform.localScale =
                new Vector3( fireWork.transform.localScale.x + (Time.deltaTime / fadeInTime),
                fireWork.transform.localScale.y + (Time.deltaTime / fadeInTime),
                fireWork.transform.localScale.z + (Time.deltaTime / fadeInTime));
            yield return null;
        }
    }
}
