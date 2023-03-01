using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageNameController : MonoBehaviour
{
    [SerializeField] Text nameTxt;
    [SerializeField] Text nameAdjTxt;
    [SerializeField] TextMeshProUGUI stageText;
    [SerializeField] GameObject namePlate;
    MainUIController mainUIController;
    int stageNum = 1;

    private void Awake()
    {
        mainUIController = GameObject.Find("MainCanvas").GetComponent<MainUIController>();
    }

    private void Start()
    {
        nameTxt.text = NameText();
        NamePlateOnOff();
    }

    string NameText()
    {
        bool isCustomLevel = mainUIController.state == MainMenuState.Level ? false : true;
        
        if (GameDataManager.GetInstance.GetLevelName(stageNum) == "")
        {
            return "???";
        }
        else
        {
            return GameDataManager.GetInstance.GetLevelName(stageNum);
        }
    }

    void NamePlateOnOff()
    {
        if (mainUIController.state == MainMenuState.Level)
        {
            if (stageNum <= GameDataManager.GetInstance.UserDataDic[GameManager.GetInstance.userId].clearLevel)
            {
                namePlate.SetActive(true);
            }
            else
            {
                namePlate.SetActive(false);
            }
        }
        else
        {
            if (GameDataManager.GetInstance.GetLevelName(stageNum) == "")
            {
                namePlate.SetActive(false);
            }
            else
            {
                namePlate.SetActive(false);
            }
        }
    }

    public void StageNumSetUp(int inputStageNum)
    {
        stageNum = inputStageNum;

        if (mainUIController.state == MainMenuState.Level)
        {
            nameAdjTxt.text = "Stage " + inputStageNum.ToString();
        }
        else
        {
            nameAdjTxt.text = "Star " + inputStageNum.ToString();
        }

        stageText.text = inputStageNum.ToString();
    }
}
