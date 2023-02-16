using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowRose : MonoBehaviour
{
    [SerializeField] Image arrow;
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
            arrow.fillAmount = 0;
        }
    }

    IEnumerator FillIn()
    {
        arrow.fillAmount = 0;

        isFadIn = true;
        while (arrow.fillAmount < 1f)
        {
            arrow.fillAmount = arrow.fillAmount + (Time.deltaTime / fadeInTime);
            yield return null;
        }
    }
}
