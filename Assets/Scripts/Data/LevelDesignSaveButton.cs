using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDesignSaveButton : MonoBehaviour
{
    private Button levelDesignButton;
    void Start()
    {
        levelDesignButton = GameObject.Find("LevelDesignSaveButton").GetComponent<Button>();
        levelDesignButton.onClick.AddListener(GameDataManager.GetInstance.CreateFile);
    }
}
