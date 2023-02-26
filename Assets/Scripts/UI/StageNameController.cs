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
        if (stageNum <= GameDataManager.GetInstance.UserDataDic[GameManager.GetInstance.userId].clearLevel)
        {
            namePlate.SetActive(true);
        }
        else
        {
            namePlate.SetActive(false);
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
            nameAdjTxt.text = "Custom " + inputStageNum.ToString();
        }

        stageText.text = inputStageNum.ToString();
    }
}
