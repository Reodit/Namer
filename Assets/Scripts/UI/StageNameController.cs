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
    int stageNum = 1;

    private void OnEnable()
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
        nameAdjTxt.text = "Stage " + inputStageNum.ToString();
        stageText.text = inputStageNum.ToString();
    }
}
