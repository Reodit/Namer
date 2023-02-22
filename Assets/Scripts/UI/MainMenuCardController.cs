using System.Collections;
using System.Collections.Generic;
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
        if (GameManager.GetInstance.CurrentState == GameStates.Pause) return;
        if (!CardManager.GetInstance.ableCardCtr) return;
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
    void CardSelectOff()
    {
        highlight.SetActive(false);
        cr.enabled = false;
        transform.DORotateQuaternion(cardHolder.transform.rotation, 0.5f);
        CardManager.GetInstance.isPickCard = false;
        CardManager.GetInstance.pickCard = null;
        UIManager.GetInstance.isShowNameKeyPressed = false;
    }
    public void TouchInteractObj()
    {
        StartCoroutine(CastCardDealing());
    }
    IEnumerator CastCardDealing()
    {
        originPos = gameObject.transform.position;
        originRot = gameObject.transform.localRotation.eulerAngles;
        highlight.SetActive(false);
        bc.enabled = false;
        cr.enabled = false;
        CardManager.GetInstance.isPickCard = false;
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
        MainCastCard(gameObject.name);
        CardManager.GetInstance.target = null;
        CardManager.GetInstance.pickCard = null;
        Destroy(particleObj);
        AllPopUpNameOff();
        yield return new WaitForSeconds(1f);
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
    public void MainCastCard(string cardName)
    {
        switch (cardName)
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
            case "1StageCard(Clone)":
                GameManager.GetInstance.SetLevelFromCard(cardName);
                LoadingSceneController.LoadScene("DemoPlay");
                break;
            case "2StageCard(Clone)":
                GameManager.GetInstance.SetLevelFromCard(cardName);
                LoadingSceneController.LoadScene("DemoPlay");
                break;
            case "3StageCard(Clone)":
                GameManager.GetInstance.SetLevelFromCard(cardName);
                LoadingSceneController.LoadScene("DemoPlay");
                break;
            case "4StageCard(Clone)":
                GameManager.GetInstance.SetLevelFromCard(cardName);
                LoadingSceneController.LoadScene("DemoPlay");
                break;
            case "5StageCard(Clone)":
                GameManager.GetInstance.SetLevelFromCard(cardName);
                LoadingSceneController.LoadScene("DemoPlay");
                break;
            case "6StageCard(Clone)":
                GameManager.GetInstance.SetLevelFromCard(cardName);
                LoadingSceneController.LoadScene("DemoPlay");
                break;
            // LoadingSceneController.LoadScene(“JSTESTER”);
            //이부분 살짝 수정함
            // GameManager.GetInstance.SetLevelFromCard(cardName);
            default:
                break;
        }
    }
}