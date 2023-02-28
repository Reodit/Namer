using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuCardController : MonoBehaviour
{
    public PRS originPRS;
    GameObject cardHolder;
    [SerializeField] GameObject frontCover;
    [SerializeField] BoxCollider bc;
    [SerializeField] GameObject highlight;
    MainUIController mainUIController;
    CardRotate cr;
    GameObject levelSelectCardHolder;
    Vector3 originPos;
    Vector3 originRot;

    bool isTouching;
    private void Start()
    {
        cr = this.gameObject.GetComponent<CardRotate>();
        mainUIController = FindObjectOfType<MainUIController>();
        levelSelectCardHolder = GameObject.Find("CardHolders").transform.Find("LevelSelectCardHolder").gameObject;
        cardHolderPicker();
    }
    void cardHolderPicker()
    {
        if (this.gameObject.transform.parent?.name == "MainMenuCards")
        {
            cardHolder = FindObjectOfType<CardManager>().gameObject;
        }
        else
        {
            cardHolder = FindObjectOfType<LevelSelectCardController>().gameObject;
        }
    }
    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
    //카드 영역에서 마우스 누르면 카드 선택 커서로 변경, 카드를 숨김
    private void OnMouseDown()
    {
        // 메인메뉴 카드 선택하면 바로 실행
        if (GameManager.GetInstance.CurrentState == GameStates.Pause
            || CardManager.GetInstance.isCasting) return;
        if (!CardManager.GetInstance.ableCardCtr || !CardManager.GetInstance.isCardDealingDone) return;
        if (mainUIController.state == MainMenuState.Main)
        {
            CardManager.GetInstance.target = mainUIController.mainRose;
        }
        else if (mainUIController.state == MainMenuState.Level)
        {
            CardManager.GetInstance.target = mainUIController.levelRose;
        }
        else if (mainUIController.state == MainMenuState.Edit)
        {
            CardManager.GetInstance.target = mainUIController.editRose;
        }

        TouchInteractObj();

        /* 메인메뉴 카드 선택후 장미 선택 실
        if (GameManager.GetInstance.CurrentState == GameStates.Pause
            || CardManager.GetInstance.isCasting) return;
        if (!CardManager.GetInstance.ableCardCtr || !CardManager.GetInstance.isCardDealingDone) return;
        //다른 카드가 골라져 있다면 그 카드 선택을 취소하고 이 카드로 변경
        if (CardManager.GetInstance.isPickCard && CardManager.GetInstance.pickCard != this.gameObject)
        {
            CardManager.GetInstance.pickCard.GetComponent<MainMenuCardController>().CardSelectOff();
            CardSelectOn();
        }
        else if (!CardManager.GetInstance.isPickCard)
        {
            CardSelectOn();
        }
        //카드를 선택후에 한번 더 누르면 하이라이트를 끄고 회전을 멈추고 처음 상태로 되돌
        else
        {
            if (CardManager.GetInstance.pickCard != this.gameObject) return;
            CardSelectOff();
        }
        */
    }
    void CardSelectOn()
    {
        if (CardManager.GetInstance.pickCard != null) return;
        highlight.SetActive(true);
        CardManager.GetInstance.isPickCard = true;
        CardManager.GetInstance.pickCard = this.gameObject;
        cr.enabled = true;
        UIManager.GetInstance.isShowNameKeyPressed = true;
        SoundManager.GetInstance.Play("CardHover");
    }
    public void CardSelectOff()
    {
        highlight.SetActive(false);
        cr.enabled = false;
        transform.DORotateQuaternion(cardHolder.transform.rotation, 0.5f);

        if(CardManager.GetInstance.pickCard == gameObject)
        {
            CardManager.GetInstance.isPickCard = false;
            CardManager.GetInstance.pickCard = null;
            UIManager.GetInstance.isShowNameKeyPressed = false;
        }
    }
    public void TouchInteractObj()
    {
        if (isTouching) return;
        StartCoroutine(CastCardDealing());
    }

    IEnumerator CastCardDealing()
    {
        CardManager.GetInstance.isCasting = true;
        isTouching = true;
        originPos = gameObject.transform.position;
        originRot = gameObject.transform.localRotation.eulerAngles;
        highlight.SetActive(false);
        bc.enabled = false;
        cr.enabled = false;
        gameObject.transform.DOLocalRotate(new Vector3(originRot.x * -1, originRot.y + 180, 0), 0.3f);
        yield return new WaitForSeconds(0.3f);
        gameObject.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f);
        gameObject.transform.DOMove(
            CardManager.GetInstance.target.transform.position + new Vector3(0, 0.5f, 0), 0.4f);
        SoundManager.GetInstance.Play("CardFly");
        yield return new WaitForSeconds(0.1f);
        GameObject particleObj =
            Instantiate(Resources.Load<GameObject>("Prefabs/Interaction/Effect/CardCastEffect"),
            CardManager.GetInstance.target.transform.position + new Vector3(0, 1.1f, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        CardManager.GetInstance.myCards.Remove(gameObject.GetComponent<CardController>());
        CardManager.GetInstance.CardAlignment();
        yield return new WaitForSeconds(0.6f);
        CardManager.GetInstance.target = null;
        CardManager.GetInstance.pickCard = null;
        Destroy(particleObj);
        AllPopUpNameOff();
        CardManager.GetInstance.isPickCard = false;
        CardManager.GetInstance.isCasting = false;
        MainCastCard(gameObject.name);
        isTouching = false;
        yield return new WaitForSeconds(1.5f);
        if (this.name != "OptionCard(Clone)" && name != "EncyclopediaCard(Clone)")
        {
            CardReturn();
        }
    }
    public void CardReturn()
    {
        gameObject.transform.DOLocalRotate(Camera.main.transform.localRotation.eulerAngles
            , 0.5f);
        gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
        gameObject.transform.DOMove(originPos, 0.5f);
        bc.enabled = true;
        CardSelectOff();
    }
    void AllPopUpNameOff()
    {
        UIManager.GetInstance.isShowNameKeyPressed = false;
    }

    string CheckCardName(string cardName)
    {
        string stageCard = "StageCard";
        if (cardName.Contains(stageCard))
        {
            return stageCard;
        }
        else
        {
            return cardName;
        }
    }

    public void MainCastCard(string cardName)
    {
        string inputString;

        inputString = CheckCardName(cardName);

        switch (inputString)
        {
            case "StartCard(Clone)":
                mainUIController.LevelSelectScene();
                break;
            case "EncyclopediaCard(Clone)":
                mainUIController.EncyclopediaScene();
                break;
            case "OptionCard(Clone)":
                mainUIController.OptionPanelOpen();
                break;
            case "CreditCard(Clone)":
                mainUIController.CreditScene();
                break;
            case "MainCard(Clone)":
                mainUIController.MainMenuScene();
                break;
            case "EditCard(Clone)":
                mainUIController.LevelEditScene();
                break;
            case "MapEditCard(Clone)":
                LoadingSceneController.LoadScene("LevelEditor");
                break;
            case "GameOffCard(Clone)":
                Application.Quit();
                break;
            case "ComingSoonCard":
                mainUIController.GameUpdateInfo();
                break;
            case "StageCard":
                GameManager.GetInstance.SetLevelFromCard(cardName);
                CheckClearLevel(cardName);
                //LoadingSceneController.LoadScene("DemoPlay");
                // 레벨 디자인 시 활성화
                // DetectManager.GetInstance.InitTilesObjects();
                // GameManager.GetInstance.SetLevelFromCard("LevelDesign");
                // LoadingSceneController.LoadScene("LevelDesign");
                break;
            // LoadingSceneController.LoadScene("JSTESTER");
            //이부분 살짝 수정함
            // GameManager.GetInstance.SetLevelFromCard(cardName);              
            default:
                break;
        }
    }

    private void CheckClearLevel(string cardName)
    {

        StringBuilder sb = new StringBuilder();
        foreach (var letter in cardName)
        {
            if (letter >= '0' && letter <= '9')
            {
                sb.Append(letter);
            }
        }
        int level = int.Parse(sb.ToString());
        if (GameDataManager.GetInstance.UserDataDic[GameManager.GetInstance.userId].clearLevel == -1)
        {
            if (cardName == "1StageCard")
            {
                LoadingSceneController.LoadScene("DemoPlay");
            }
            else
            {
                CardReturn();
                mainUIController.InfoStagePopUp();
            }
        }
        else if (level <= GameDataManager.GetInstance.UserDataDic[GameManager.GetInstance.userId].clearLevel + 1)
        {
            LoadingSceneController.LoadScene("DemoPlay");
        }
        else
        {
            CardReturn();
            mainUIController.InfoStagePopUp();
        }
    }
}