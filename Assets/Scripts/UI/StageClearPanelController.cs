using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class StageClearPanelController : MonoBehaviour
{
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject namingPanel;
    [SerializeField] GameObject rewardPanel;
    [SerializeField] GameObject namingBtn;
    [SerializeField] GameObject rewardBtn;
    [SerializeField] GameObject nameFrame;
    [SerializeField] GameObject nameKit;
    [SerializeField] GameObject nameInfoText;
    [SerializeField] GameObject nameOKBtn;
    [SerializeField] GameObject nameCancleBtn;
    [SerializeField] GameObject stageClearOKBtn;
    [SerializeField] GameObject rewardInfoTxt;
    [SerializeField] GameObject topButtons;
    [SerializeField] GameObject bottomButtons;
    [SerializeField] GameObject joyButtons;
    GameObject clearRig;
    GameObject namingRig;
    GameObject rewardRig;
    GameObject namingCard;
    GameObject planetObj;

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
        SoundManager.GetInstance.Play("WinSound");       
        clearRig.SetActive(true);
        topButtons.SetActive(false);
        bottomButtons.SetActive(false);
        joyButtons.SetActive(false);

        CheckRewardExist();
    }

    private void CheckRewardExist()
    {
        if (GameDataManager.GetInstance.GetRewardCardCount() <= 0)
        {
            isRewardDone = true;
            rewardBtn.SetActive(false);
        }
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

        nameFrame.transform.DOLocalMove(new Vector3(0, -58.7999992f, 0), 1f);
        nameFrame.transform.DOScale(new Vector3(0.6720456f, 0.6720456f, 0.6720456f), 1f);
        nameKit.transform.GetChild(1).transform.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        nameKit.SetActive(false);
        namingCard = namingRig.transform.GetChild(0).gameObject;
        namingCard.SetActive(true);
        nameCancleBtn.SetActive(true);
        nameInfoText.SetActive(true);
    }

    public void NameCancleBtn()
    {
        nameCancleBtn.SetActive(false);
        nameInfoText.SetActive(false);
        namingRig.transform.GetChild(0).gameObject.SetActive(false);
        nameKit.transform.GetChild(1).transform.gameObject.SetActive(true);
        nameKit.SetActive(true);
        nameFrame.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 1f);
        nameFrame.transform.DOLocalMove(new Vector3(0, 100, 0), 1f);
    }

    public void NamingDone()
    {
        nameOKBtn.SetActive(true);
        nameCancleBtn.SetActive(false);
        nameInfoText.SetActive(false);
        planetObj = namingRig.transform.GetChild(1).gameObject;
        planetObj.transform.DOLocalMove(new Vector3(0, planetObj.transform.localPosition.y,
            planetObj.transform.localPosition.z), 1f);
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
