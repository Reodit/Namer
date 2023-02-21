using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class PopUpNameController : MonoBehaviour
{
    [SerializeField] Text nameText;
    Transform cardHolder;
    InteractiveObject interactiveObject;
    int currentAdjCount;

    private void OnEnable()
    {
        cardHolder = Camera.main.transform;
        interactiveObject = GetComponentInParent<InteractiveObject>();
    }

    void Update()
    {
        setUpPopUpName();
    }

    private void setUpPopUpName()
    {
        gameObject.transform.rotation = cardHolder.rotation;
        var scene = SceneManager.GetActiveScene();
        if (scene.name == "MainScene") return;
        nameText.text = interactiveObject.GetCurrentName();

        currentAdjCount = interactiveObject.GetAddAdjectiveCount();

        gameObject.transform.localPosition = new Vector3(
             gameObject.transform.localPosition.x,
             0.5f * gameObject.transform.parent.transform.localScale.y,
             gameObject.transform.localPosition.z
            );


        gameObject.transform.localScale = new Vector3(
            1,1 / gameObject.transform.parent.transform.localScale.y,1);

        if (currentAdjCount == 1)
        {
            nameText.gameObject.transform.localScale = new Vector3(0.009f, 0.01f, 0.01f);
        }

        if(currentAdjCount == 2)
        {
            nameText.gameObject.transform.localScale = new Vector3(0.006f, 0.01f, 0.01f);
        }
        else
        {
            nameText.gameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }
    }
}
