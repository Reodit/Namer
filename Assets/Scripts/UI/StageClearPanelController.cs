using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StageClearPanelController : MonoBehaviour
{
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject namingPanel;
    [SerializeField] GameObject rewardPanel;
    [SerializeField] GameObject namingBtn;
    [SerializeField] GameObject rewardBtn;
    [SerializeField] GameObject nameFrame;
    [SerializeField] GameObject nameKit;
    [SerializeField] GameObject nameOKBtn;
    [SerializeField] GameObject nameCancleBtn;
    [SerializeField] GameObject stageClearOKBtn;
    [SerializeField] GameObject rewardInfoTxt;
    GameObject clearRig;
    GameObject namingRig;
    GameObject rewardRig;

    bool isNamingDone, isRewardDone;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        clearRig = Camera.main.transform.Find("ClearRig").gameObject;
        namingRig = clearRig.transform.Find("NamingRig").gameObject;
        rewardRig = clearRig.transform.Find("RewardRig").gameObject;

        clearRig.SetActive(true);
    }

    public void NamingProcess()
    {
        buttons.SetActive(false);
        namingBtn.SetActive(false);
        namingPanel.SetActive(true);
        namingRig.SetActive(true);
    }

    public void NameConfirmBtn()
    {
        StartCoroutine(NameConfirm());
    }

    IEnumerator NameConfirm()
    {
        nameFrame.transform.DOScale(new Vector3(0.622527f, 0.622527f, 0.622527f), 1f);
        yield return new WaitForSeconds(1f);
        nameKit.SetActive(false);
        namingRig.transform.GetChild(0).gameObject.SetActive(true);
        nameCancleBtn.SetActive(true);
    }

    public void NameCancleBtn()
    {
        nameCancleBtn.SetActive(false);
        namingRig.transform.GetChild(0).gameObject.SetActive(false);
        nameKit.SetActive(true);
        nameFrame.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 1f);
    }

    public void NamingDone()
    {
        nameOKBtn.SetActive(true);
        nameCancleBtn.SetActive(false);
    }

    public void NameOKBtn()
    {
        namingRig.SetActive(false);
        namingPanel.SetActive(false);
        isNamingDone = true;
        ButtonsCheck();
        buttons.SetActive(true);
    }

    public void RewardProcess()
    {
        CardManager.GetInstance.isEncyclopedia = true;
        buttons.SetActive(false);
        rewardBtn.SetActive(false);
        rewardPanel.SetActive(true);
        rewardRig.SetActive(true);
        if (rewardRig.transform.GetChild(0).transform.childCount == 0)
        {
            rewardInfoTxt.SetActive(true);
        };


    }

    public void RewardOKBtn()
    {
        CardManager.GetInstance.isEncyclopedia = false;
        buttons.SetActive(true);
        rewardPanel.SetActive(false);
        rewardRig.SetActive(false);
        isRewardDone = true;
        ButtonsCheck();
    }

    void ButtonsCheck()
    {
        if (isNamingDone && isRewardDone)
        {
            StageUIOKBtn();
        }
    }

    public void StageUIOKBtn()
    {
        GameManager.GetInstance.ChangeGameState(GameStates.LevelSelect);
        GameDataManager.GetInstance.UpdateUserData(true);
        this.gameObject.SetActive(false);
        SceneManager.LoadScene("MainScene");
    }
}
