using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainableRose : MonoBehaviour
{
    [SerializeField] GameObject highLight;
    [SerializeField] GameObject card;
    Animator cardAnim;
    Vector3 startPos;
    Color alpha;

    // Start is called before the first frame update
    void Start()
    {
        cardAnim = this.gameObject.GetComponent<Animator>();
        startPos = new Vector3(0f, 0f, -0.1f);
        alpha = card.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (highLight.activeInHierarchy)
        {
            cardAnim.SetBool("animStart", true);
            alpha.a += 0.05f;
            card.GetComponent<SpriteRenderer>().color = alpha;

        }

        else if (!highLight.activeInHierarchy)
        {
            cardAnim.SetBool("animStart", false);
            this.transform.localPosition = startPos;
            alpha.a = 0;
            card.GetComponent<SpriteRenderer>().color = alpha;
        }
    }

    IEnumerator alphaControl()
    {

        


        yield return null;
    }

    //IEnumerator ObtainCoroutine()
    //{
    //    while (transform.localPosition.x < 0.2f && transform.localPosition.y < 0.2f && transform.localScale.y > 0.005f)
    //    {
    //        this.transform.localPosition += new Vector3(0.01f, 0.01f, 0);
    //        this.transform.localScale += new Vector3(0, -0.01f, 0);
    //        yield return null;
    //    }
    //}
}
