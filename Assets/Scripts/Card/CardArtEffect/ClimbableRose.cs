using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ClimbableRose : MonoBehaviour
{
    [SerializeField] SpriteRenderer stairSprite;
    [SerializeField] GameObject highLight;
    float fadeInTime = 1f;

    bool isFadIn;

    private void Update()
    {

        if (highLight.activeInHierarchy && !isFadIn)
        {
            StartCoroutine(FadeIn());
        }
        else if (!highLight.activeInHierarchy)
        {
            isFadIn = false;
            StopCoroutine(FadeIn());
            stairSprite.color = new Color(0.4980392f, 0.9882353f, 0.9882353f, 0f);
        }
    }

    IEnumerator FadeIn()
    {
        stairSprite.color = new Color(0.4980392f, 0.9882353f, 0.9882353f, 0f);

        isFadIn = true;
        while (stairSprite.color.a < 1f)
        {
            stairSprite.color = new Color(0.4980392f, 0.9882353f, 0.9882353f, stairSprite.color.a + Time.deltaTime / fadeInTime);
            yield return null;
        }
    }
}
