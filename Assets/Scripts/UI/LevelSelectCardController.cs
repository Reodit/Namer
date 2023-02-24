using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelSelectCardController : MonoBehaviour
{
    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] Transform cardHolderPoint;
    [SerializeField] Transform cardHolderLeft;
    [SerializeField] Transform cardHolderRight;
    [SerializeField] GameObject mainMenuCard;
    public List<MainMenuCardController> mainCards;
    List<GameObject> startCards;
    GameObject StageCardPrefab;
    List<GameObject> levelLayoutGroups;
    [SerializeField] GameObject[] levelSelectTiles;

    [SerializeField] float levelSelectMovingTime = 1.5f;
    [SerializeField] float cardDealingSpeed = 0.2f;

    private void Start()
    {
        StageCardPrefab = Resources.Load("Prefabs/Cards/03.LevelCard/StageCard") as GameObject;
        startCards = new List<GameObject>();
        levelLayoutGroups = new List<GameObject>();
        StartCardsInit();
        CardStart();
        StartCoroutine(LevelGroudsSetUp());
    }

    private void StartCardsInit()
    {
        startCards.Add(mainMenuCard);
        for (int i = 1; i < GameDataManager.GetInstance.LevelDataDic.Count + 1; i++)
        {
            int inputNum = i;
            GameObject cardPrefab = Instantiate(StageCardPrefab);
            cardPrefab.name = inputNum.ToString() + "StageCard";
            cardPrefab.GetComponent<StageNameController>().StageNumSetUp(inputNum);
            startCards.Add(cardPrefab);
        }
    }

    void CardStart()
    {
        StartCoroutine(DealCard());
    }

    private void SetUpStageCards()
    {
        for (int i = 5; i < startCards.Count; i++)
        {
            if( i % 5 == 0)
            {
                string objName = ((i / 5) + 1).ToString() + "page";
                GameObject levelLayoutGroup = new GameObject(objName);
                levelLayoutGroup.transform.parent = GameObject.Find("LevelSelectCards").transform;
                levelLayoutGroups.Add(levelLayoutGroup);
                levelLayoutGroup.SetActive(false);
            }

            int alignNum = i % 5;
            var cardObject = startCards[i];
            cardObject.transform.position = mainCards[alignNum].transform.position;
            cardObject.transform.rotation = mainCards[alignNum].transform.rotation;
            cardObject.transform.parent = levelLayoutGroups[(i / 5) - 1].transform;
        }
    }

    float ranNum;
    IEnumerator LevelGroudsSetUp()
    {
        for (int i = 0; i < levelSelectTiles.Length; i++)
        {
            ranNum = Random.Range(0f, 2f);
            levelSelectTiles[i].SetActive(true);
            levelSelectTiles[i].transform.DOMove(levelSelectTiles[i].transform.position + new Vector3(0f, 15f, 0), levelSelectMovingTime + ranNum);
            yield return null;
        }

    }

    //시작 카드를 딜링해주는 메서드 
    IEnumerator DealCard()
    {
        CardManager.GetInstance.isCardDealingDone = false;
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 5; i++)
        {
            MainMenuAddCard(startCards[i]);

            yield return new WaitForSeconds(cardDealingSpeed);
        }
        yield return new WaitForSeconds(2f);
        SetUpStageCards();
        CardManager.GetInstance.isCardDealingDone = true;
    }



    //카드를 생성하는 메서드 
    void MainMenuAddCard(GameObject cardPrefab)
    {
        if (cardPrefab.name == "MainCard")
        {
            var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Quaternion.identity);
            var card = cardObject.GetComponent<MainMenuCardController>();
            mainCards.Add(card);
            cardObject.transform.parent = GameObject.Find("LevelSelectCards").transform.GetChild(0).transform;
        }
        else
        {
            var cardObject = cardPrefab;
            cardObject.transform.position = cardSpawnPoint.position;
            cardObject.transform.rotation = Quaternion.identity;
            var card = cardObject.GetComponent<MainMenuCardController>();
            mainCards.Add(card);
            cardObject.transform.parent = GameObject.Find("LevelSelectCards").transform.GetChild(0).transform;
        }
        MainCardAlignment();
    }


    //메인화면 카드를 정렬하는 메서드 
    void MainCardAlignment()
    {
        List<PRS> originCardPRSs = new List<PRS>();
        originCardPRSs = RoundAlignment(cardHolderLeft, cardHolderRight, mainCards.Count, new Vector3(1f, 1f, 1f));

        for (int i = 0; i < mainCards.Count; i++)
        {
            var targetCard = mainCards[i];

            targetCard.originPRS = originCardPRSs[i];
            targetCard.originPRS.rot = cardHolderPoint.transform.rotation;
            targetCard.MoveTransform(targetCard.originPRS, true, 2f);
        }
    }

    //카드 정렬하는 메서드
    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);

        switch (objCount)
        {
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.24f, 0.73f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
            case 4: objLerps = new float[] { 0.1f, 0.37f, 0.64f, 0.9f }; break;
            case 5: objLerps = new float[] { 0.05f, 0.275f, 0.5f, 0.725f, 0.95f }; break;
            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                {
                    objLerps[i] = interval * i;
                }
                break;
        }

        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Quaternion.identity;
            if (objCount >= 4)
            {
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }
            results.Add(new PRS(targetPos, targetRot, scale));
        }
        return results;
    }

}
