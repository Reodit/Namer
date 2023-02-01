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
    }
}
