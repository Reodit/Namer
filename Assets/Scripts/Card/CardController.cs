using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using PlayerOwnedStates;
using UnityEngine.Serialization;
using System.Collections;

public class CardController : MonoBehaviour
{
    // [SerializeField] private CardData cardData;
    [SerializeField] private ECardType cardType;
    [SerializeField] private EName nameType;
    [SerializeField] private EAdjective adjectiveType;
    
    public PRS originPRS;
    GameObject cardHolder;
    [SerializeField] GameObject frontCover;
    [SerializeField] BoxCollider bc;
    [SerializeField] GameObject highlight;
    [SerializeField] private Text UIText;
    [SerializeField] private Text NameAdjUIText;
    GameObject Encyclopedia;
    CardRotate cr;

    public EAdjective GetAdjectiveTypeOfCard()
    {
        return adjectiveType;
    }

    private void Start()
    {
        cr = this.gameObject.GetComponent<CardRotate>();
        cardHolder = FindObjectOfType<CardManager>().gameObject;
        Encyclopedia = this.transform.GetChild(0).transform.GetChild(6).gameObject;
        SetUIText();
    }

    private void SetUIText()
    {
        if (UIText == null || (cardType == ECardType.Name && NameAdjUIText == null))
        {
            Debug.LogError("인스펙터창에서 UI 텍스트를 확인 후 넣어주세요!");
        }
        
        GameDataManager gameDataManager = GameDataManager.GetInstance;
        if (cardType == ECardType.Name)
        {
            UIText.text = gameDataManager.Names[nameType].uiText;
            NameAdjUIText.text = "";
            
            if (gameDataManager.Names[nameType].adjectives != null)
            {
                EAdjective[] adjectives = gameDataManager.Names[nameType].adjectives;
                string delimiter = ", ";
                for (int i = 0; i < adjectives.Length; i++)
                {
                    NameAdjUIText.text += gameDataManager.Adjectives[adjectives[i]].uiText + delimiter;
                }
                
                NameAdjUIText.text = NameAdjUIText.text.TrimEnd(',', ' ');
            }
            else
            {
                NameAdjUIText.text = "순수";
            }
        }
        else if (cardType == ECardType.Adjective)
        {
            UIText.text = gameDataManager.Adjectives[adjectiveType].uiText;
        }

        Text contentText = Encyclopedia.GetComponentInChildren<Text>();
        if (cardType == ECardType.Name)
        {
            contentText.text = gameDataManager.Names[nameType].contentText;
        }
        else if (cardType == ECardType.Adjective)
        {
            contentText.text = gameDataManager.Adjectives[adjectiveType].contentText;
        }
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.DOLocalMove(prs.pos, dotweenTime);
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
    //카드를 누르면 하이라이트 표시를하고 카트를 회전시킨다
    private void OnMouseDown()
    {
        if (GameManager.GetInstance.CurrentState == GameStates.Pause) return;
        if (!CardManager.GetInstance.ableCardCtr) return;

        //다른 카드가 골라져 있다면 그 카드 선택을 취소하고 이 카드로 변경 
        if (CardManager.GetInstance.isPickCard && CardManager.GetInstance.pickCard != this.gameObject)
        {
            CardManager.GetInstance.pickCard.GetComponent<CardController>().CardSelectOff();
            CardSelectOn();
        }
        else if (!CardManager.GetInstance.isPickCard)
        {
            CardSelectOn();
        }
        //카드를 선택후에 한번 더 누르면 하이라이트를 끄고 회전을 멈추고 처음 상태로 되돌
        else
        {
            CardSelectOff();
        }
    }

    void CardSelectOn()
    {
        if (CardManager.GetInstance.pickCard != null) return;

        highlight.SetActive(true);
        CardManager.GetInstance.isPickCard = true;
        CardManager.GetInstance.pickCard = this.gameObject;
        if (CardManager.GetInstance.isEncyclopedia || GameManager.GetInstance.CurrentState == GameStates.Encyclopedia)
        {
            Encyclopedia.SetActive(true);
            return;
        }
        if (GameManager.GetInstance.CurrentState == GameStates.Encyclopedia)
            return;
        cr.enabled = true;
        UIManager.GetInstance.isShowNameKeyPressed = true;
    }

    void CardSelectOff()
    {
        if (CardManager.GetInstance.pickCard != this.gameObject) return;
        highlight.SetActive(false);
        if (CardManager.GetInstance.isEncyclopedia || GameManager.GetInstance.CurrentState == GameStates.Encyclopedia)
        {
            Encyclopedia.SetActive(false);
            return;
        }
        cr.enabled = false;
        CardManager.GetInstance.isPickCard = false;
        transform.DORotateQuaternion(cardHolder.transform.rotation, 0.5f);
        CardManager.GetInstance.pickCard = null;
        UIManager.GetInstance.isShowNameKeyPressed = false;
    }

    //카드 선택 커서 상태에서 상호작용 오브젝트 위에서 마우스를 놓으면 속성 부여,
    //오브젝트 아닌곳에서는 기본 커서로 다시 변경하고 카드를 다시 보이게,
    public void TouchInteractObj()
    {
        StartCoroutine(CastCardDealing());

    }

    IEnumerator CastCardDealing()
    {
        highlight.SetActive(false);
        bc.enabled = false;
        cr.enabled = false;
        CardManager.GetInstance.isPickCard = false;
        gameObject.transform.DOLocalRotate(new Vector3(0, 180, 0), 0.3f);
        yield return new WaitForSeconds(0.3f);
        gameObject.transform.DOLocalRotate(new Vector3(300, 180, 0), 0.3f);
        if (name == "NamingCard")
        {
            gameObject.transform.DOScale(new Vector3(0.01f, 0.01f, 0.01f), 0.3f);
        }
        else
        {
            gameObject.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f);
        }
        gameObject.transform.DOMove(
            CardManager.GetInstance.target.transform.position + new Vector3(0, 0.5f, 0), 0.4f);
        yield return new WaitForSeconds(0.1f);
        CardManager.GetInstance.myCards.Remove(gameObject.GetComponent<CardController>());
        GameObject particleObj =
            Instantiate(Resources.Load<GameObject>("Prefabs/Interaction/Effect/CardCastEffect"),
            CardManager.GetInstance.target.transform.position + new Vector3(0, 1.1f, 0), Quaternion.identity);
        CardManager.GetInstance.CardAlignment();
        yield return new WaitForSeconds(0.3f);
        CastCard(CardManager.GetInstance.target);
        yield return new WaitForSeconds(2f);
        CardManager.GetInstance.target = null;
        Destroy(particleObj);
        AllPopUpNameOff();
        Destroy(gameObject);
    }


    void AllPopUpNameOff()
    {
        UIManager.GetInstance.isShowNameKeyPressed = false;
    }

    public string currentLevelName = "";
    public void CastCard(GameObject target)
    {
        if(target && GameManager.GetInstance.CurrentState == GameStates.Victory)
        {
            PlanetObjController planetObjController;
            StageClearPanelController stageClearPanelController;
            GameDataManager.GetInstance.SetLevelName(currentLevelName);
            planetObjController =
                Camera.main.transform.
                Find("ClearRig").transform.
                Find("NamingRig").transform.
                Find("PlanetObj").transform.
                Find("PopUpName").GetComponent<PlanetObjController>();
            planetObjController.UpdateTxt();

            stageClearPanelController =
                GameObject.Find("IngameCanvas").transform.
                Find("StageClearPanel").GetComponent<StageClearPanelController>();

            stageClearPanelController.NamingDone();
        }

        if (target)
        {
            GameManager.GetInstance.localPlayerMovement.addCardTarget = target;
            GameManager.GetInstance.localPlayerEntity.ChangeState(PlayerStates.AddCard);
            
            if (cardType == ECardType.Name)
            {
                target.GetComponent<InteractiveObject>().AddName(nameType);
            }
            else if (cardType == ECardType.Adjective)
            {
                target.GetComponent<InteractiveObject>().AddAdjective(adjectiveType);
            }
        }
    }
}
