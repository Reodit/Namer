using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardManager : Singleton<CardManager>
{
    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] Transform cardHolderPoint;
    [SerializeField] Transform cardHolderLeft;
    [SerializeField] Transform cardHolderRight;
    public List<CardController> myCards;
    public List<MainMeneCardController> mainCards;
    [SerializeField] GameObject[] startCards;

    //타겟 상호작용 오브젝트 
    //[HideInInspector]
    public GameObject target;

    public bool isPickCard = false;
    public bool ableCardCtr = true;
    public bool isEncyclopedia = false;

    private void Start()
    {
        CardStart();
    }

    void CardStart()
    {
        StartCoroutine(DealCard());
    }

    //시작 카드를 딜링해주는 메서드 
    IEnumerator DealCard()
    {
        var scene = SceneManager.GetActiveScene();
        yield return new WaitForSeconds(0.1f);
        
        if(scene.name == "MainScene")
        {
            for (int i = 0; i < startCards.Length; i++)
            {
                MainMenuAddCard(startCards[i]);
                yield return new WaitForSeconds(0.5f);
            }
        }
        else
        {
            GameDataManager gameData = GameDataManager.GetInstance;
            int level = FindObjectOfType<LevelInfos>().LevelNumber;
            GameObject[] cards = gameData.GetCardPrefabs(gameData.LevelDataDic[level].cardView);
            
            for (int i = 0; i < cards.Length; i++)
            {
                AddCard(cards[i]);
                yield return new WaitForSeconds(0.5f);
            }
            
            // 테스트 시 위의 코드를 주석처리하고, 아래의 함수를 사용해주세요.
            // for (int i = 0; i < startCards.Length; i++)
            // {
            //     AddCard(startCards[i]);
            //     yield return new WaitForSeconds(0.5f);
            // }
        }
    }

    //카드를 생성하는 메서드 
    [ContextMenu("AddCard")]
    void AddCard(GameObject cardPrefab)
    {
        var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Quaternion.identity);
        var card = cardObject.GetComponent<CardController>();
        myCards.Add(card);
        CardAlignment();
    }

    void MainMenuAddCard(GameObject cardPrefab)
    {
        var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Quaternion.identity);
        var card = cardObject.GetComponent<MainMeneCardController>();
        var scene = SceneManager.GetActiveScene();
        cardObject.transform.parent = GameObject.Find("MainMenuCards").transform;
        mainCards.Add(card);
        MainCardAlignment();
    }

    //카드를 정렬하는 메서드 
    public void CardAlignment()
    {
        List<PRS> originCardPRSs = new List<PRS>();
        originCardPRSs = RoundAlignment(cardHolderLeft, cardHolderRight, myCards.Count, new Vector3(1f, 1f, 1f));

        for (int i = 0; i < myCards.Count; i++)
        {
            var targetCard = myCards[i];
            
            targetCard.originPRS = originCardPRSs[i];
            targetCard.originPRS.rot = cardHolderPoint.transform.rotation;
            targetCard.MoveTransform(targetCard.originPRS, true, 2f);
        }
    }
    //메인화면 카드를 정렬하는 메서드 
    public void MainCardAlignment()
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

    //카드를 둘글게 정렬하는 메서드
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

public class PRS
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;

    public PRS(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
    }
}
