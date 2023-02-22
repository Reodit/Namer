using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField] private EName objectName;
    [Range(0, 2)][SerializeField] private int[] countAdj = new int[20];
    [SerializeField] GameObject popUpName;

    // Todo 후에 체크하는 로직이 다른 곳으로 이동하면, 수정 예정
    Vector3 objectPos;

    // Todo 테스트 완료 후, Test 관련 코드 삭제 예정
    #region [Test] Change Count To Inspector

        private int[] initCountAdj = new int[20];
        private bool isCard = false;
        
    #endregion
    
    // Level Edit Mode
    private bool isFinishMapSetting = false;
    
    // object's name = adjective card's ui texts + name card's ui text
    private string addNameText;
    private Queue<EAdjective> addAdjectiveTexts;
    private int[] countNameAdj;
    
    // Max Value of Adjective Count
    private int maxAdjCount = 2;

    // added adjective functions(interface)
    private IAdjective[] adjectives;
    public IAdjective[] Adjectives { get { return  adjectives; } }
    
    // object's information
    public SObjectInfo objectInfo;
    public int GetObjectID()
    {
        return objectInfo.objectID;
    }
    public EName GetObjectName()
    {
        return objectName;
    }
    
    // manager to get card data 
    private GameDataManager gameData;
    
    private void OnEnable()
    {
        if (!gameObject.CompareTag("InteractObj"))
        {
            Debug.LogError("태그를 InteractObj로 설정해주세요!");
        }

        gameData = GameDataManager.GetInstance;
        addAdjectiveTexts = new Queue<EAdjective>();
        countNameAdj = new int[gameData.Adjectives.Count];
        adjectives = new IAdjective[gameData.Adjectives.Count];

        if (GameManager.GetInstance.CurrentState == GameStates.LevelEditMode)
        {
            // objectName = EName.Null;
            isFinishMapSetting = false;
        }
    }

    private void Start()
    {
        if (GameManager.GetInstance.CurrentState == GameStates.InGame)
        {
            objectName = objectInfo.nameType;

            SetAdjectiveFromData(gameData.Names[objectName].adjectives, false);
            SetAdjectiveFromData(objectInfo.adjectives);
        }
        addNameText = gameData.Names[objectName].uiText;
        
        // Test
        countAdj.CopyTo(initCountAdj, 0);
        //
    }

    private void SetAdjectiveFromData(EAdjective[] addedAdjectives, bool isAdjective = true)
    {
        if (addedAdjectives == null || addedAdjectives.Length <= 0)
        {
            return;
        }

        for (int i = 0; i < addedAdjectives.Length; i++)
        {
            if (isAdjective && countAdj.Sum() - countNameAdj.Sum() >= maxAdjCount)
            {
                Debug.LogError("꾸밈 성질은 2개만 부여할 수 있어요");
            }
            
            SetAdjective(addedAdjectives[i], isAdjective);
        }
    }

#region Run When Inspector Data Change & Test
    private void Update()
    {
        // todo 후에 배열 포지션 체크하는 로직이 이동하면, 수정 예정
        if (!(GameManager.GetInstance.CurrentState == GameStates.InGame || GameManager.GetInstance.CurrentState == GameStates.LevelEditMode || GameManager.GetInstance.CurrentState == GameStates.LevelEditorTestPlay))
        {
            return;
        }
        
        if (DetectManager.GetInstance.GetObjectsData() != null && objectPos != Vector3Int.RoundToInt(this.gameObject.transform.position))
        {
            DetectManager.GetInstance.CheckValueInMap(this.gameObject);
            objectPos = Vector3Int.RoundToInt(this.gameObject.transform.position);

            // 물체가 중력에 의해 떨어진 후 디텍팅 하는 로직, 급하게 짜느라 문제가 있을 수 있음
            DetectManager.GetInstance.StartDetector(new[] { this.gameObject }.ToList());
        }

        AllPopUpNameCtr();
        AdjectiveTest();
        
        if (DetectManager.GetInstance.GetObjectsData() != null && GameManager.GetInstance.CurrentState == GameStates.LevelEditMode && !isFinishMapSetting)
        {
            isFinishMapSetting = true;
            SetCard();
        }
    }
    
    private void AdjectiveTest()
    {
        if (!isCard && countAdj.Sum() != initCountAdj.Sum())
        {
            for (int i = 0; i < countAdj.Length; i++)
            {
                if (countAdj[i] < initCountAdj[i])
                {
                    int diff = initCountAdj[i] - countAdj[i];
                    while (diff > 0)
                    {
                        TestSubtractAdjective((EAdjective)i);
                        diff--;
                    }
            
                    initCountAdj[i] = countAdj[i];
                }
                else if (countAdj[i] > initCountAdj[i])
                {
                    int diff = countAdj[i] - initCountAdj[i];
                    while (diff > 0)
                    {
                        if (initCountAdj.Sum() - countNameAdj.Sum() >= maxAdjCount)
                        {
                            EAdjective subtractAdjective = addAdjectiveTexts.Dequeue();
                            TestSubtractAdjective(subtractAdjective);
                            countAdj[(int)subtractAdjective]--;
                        }
                        
                        TestSetAdjective((EAdjective)i);
                        diff--;
                    }
            
                    initCountAdj[i] = countAdj[i];
                }
            }
        }
    }
    
    private void TestSetAdjective(EAdjective addAdjective)
    {
        int adjIndex = (int)addAdjective;
        SAdjectiveInfo adjectiveInfo = gameData.Adjectives[addAdjective];
        if (adjectives[adjIndex] == null)
        {
            adjectives[adjIndex] = adjectiveInfo.adjective.DeepCopy();
        }

        adjectives[adjIndex].SetCount(1);
        addAdjectiveTexts.Enqueue(addAdjective);

        // Todo 다른 곳으로 이동해야하는 IAdjective 함수?
        adjectives[adjIndex].Execute(this);

        // todo 카드 넣었을 때에 검출 테스트
        var target = (new[] { this.gameObject }).ToList();
        DetectManager.GetInstance.StartDetector(target);
    }
    
    private void TestSubtractAdjective(EAdjective subtractAdjective)
    {
        int adjIndex = (int)subtractAdjective;
        if (adjectives[adjIndex] == null)
        { 
            return;
        }
        
        // Todo 다른 곳으로 이동해야하는 IAdjective 함수?
        adjectives[adjIndex].Abandon(this);
        //
        
        if (countAdj[adjIndex] == 0)
        {
            adjectives[adjIndex] = null;
        }
        else
        {
            adjectives[adjIndex].SetCount(-1);
        }
        
        Queue<EAdjective> temp = new Queue<EAdjective>();
        while (true)
        {
            if (addAdjectiveTexts.Count == 0)
            {
                break;
            }

            EAdjective adjective = addAdjectiveTexts.Dequeue();
            if (adjective != subtractAdjective)
            {
                Debug.Log(adjective);
                temp.Enqueue(adjective);
            }
        }
        
        addAdjectiveTexts = temp;
    }

#endregion

#region LevelEdit Mode
    
    public void AddName(EName addedName)
    {
        objectName = addedName;
    }
    
    public bool AddAdjective(EAdjective addedAdjective)
    {
        if (countAdj.Sum() >= maxAdjCount)
        {
            return false;
        }
        
        int adjIndex = GameDataManager.GetInstance.Adjectives[addedAdjective].priority;
        countAdj[adjIndex]++;

        return true;
    }

    private void SetCard()
    {
        // 테스트 완료 후 살릴 예정
        // AddName(objectName); 
        
        AddNameCard(objectName);

        if (countAdj.Sum() != initCountAdj.Sum() && countAdj.Sum() - countNameAdj.Sum() > 0)
        {
            for (int i = 0; i < GameDataManager.GetInstance.Adjectives.Count; i++)
            {
                if (countAdj.Sum() - countNameAdj.Sum() > 0)
                {
                    for (int j = 0; j < countAdj[i] - countNameAdj[i]; j++)
                    {
                        // 테스트 완료 후 살릴 예정
                        // AddAdjective((EAdjective)i);
                        AddAdjectiveCard((EAdjective)i);
                    }
                }
            }
        }
    }
    
#endregion

#region  InGame Mode

    public void AddNameCard(EName? addedName)
    {
        // Test
        isCard = true;
        //
        
        SubtractName(objectName);
        
        // Check Error
        if (addedName == null)
        {
            Debug.LogError("Card의 Name 정보를 확인해주세요!");
            return;
        }
        
        EAdjective[] addAdjectives = gameData.Names[(EName)addedName].adjectives;
        if (addAdjectives != null)
        {
            foreach (EAdjective addAdjective in addAdjectives)
            {
                SetAdjective(addAdjective, false);
            }
        }
        
        objectName = (EName)addedName;
        addNameText = gameData.Names[(EName)addedName].uiText;
    }

    public void AddAdjectiveCard(EAdjective addAdjective)
    {
        // Test
        isCard = true;
        //

        if (addAdjectiveTexts.Count >= maxAdjCount)
        {
            SubtractAdjective(addAdjectiveTexts.Dequeue());
        }
        
        if (addAdjective == EAdjective.Null)
        {
            Debug.Log("Null 꾸밈 성질 카드가 맞으시죠? 확인 부탁해요!");
        }
        
        SetAdjective(addAdjective);
    }

    private void SetAdjective(EAdjective addAdjective, bool isAdjective = true)
    {
        int adjIndex = (int)addAdjective;
        
        if (adjectives[adjIndex] == null)
        {
            adjectives[adjIndex] = gameData.Adjectives[addAdjective].adjective.DeepCopy();
        }

        if (isAdjective)
        {
            addAdjectiveTexts.Enqueue(addAdjective);
        }
        else
        {
            ++countNameAdj[adjIndex];
        }

        adjectives[adjIndex].SetCount(1);
        ++countAdj[adjIndex];
        // Test
        ++initCountAdj[adjIndex];
        //
        
        // Todo 다른 곳으로 이동해야하는 IAdjective 함수?
        adjectives[adjIndex].Execute(this);
        //
        
        // Test
        isCard = false;
        //

        // todo 카드 넣었을 때에 검출 테스트
        var target = (new[] { this.gameObject }).ToList();
        DetectManager.GetInstance.StartDetector(target);
    }

    private void SubtractName(EName subtractName)
    {
        EAdjective[] subtractAdjectives = gameData.Names[subtractName].adjectives;

        if (subtractAdjectives != null)
        {
            foreach (var adjective in subtractAdjectives)
            {
                SubtractAdjective(adjective, false);
            }
        }
    }

    public void SubtractAdjective(EAdjective subtractAdjective, bool isAdjective = true)
    {
        int adjIndex = (int)subtractAdjective;
        if (adjectives[adjIndex] == null)
        { 
            return;
        }
        
        // Todo 다른 곳으로 이동해야하는 IAdjective 함수?
        adjectives[adjIndex].Abandon(this);
        //
        
        --countAdj[adjIndex];
        // Test
        --initCountAdj[adjIndex];
        //
        if (countAdj[adjIndex] <= 0)
        {
            adjectives[adjIndex] = null;
        }
        else
        {
            adjectives[adjIndex].SetCount(-1);
        }
        
        if (!isAdjective)
        {
            --countNameAdj[adjIndex];
        }
        
        // Test
        isCard = false;
        //
    }
    
    public bool CheckAdjective(EAdjective checkAdjective)
    {
        if (adjectives[(int)checkAdjective] != null)
        {
            return true;
        }

        return false;
    }
    
    private bool CheckCountAdjective(EAdjective checkAdjective)
    {
        if (countAdj[(int)checkAdjective] >= maxAdjCount)
        {
            return false;
        }

        return true;
    }
    
#endregion

#region Method for UI
    
    // 오브젝트 이름을 리턴하는 함수
    public string GetCurrentName()
    {
        string currentObjectName = null;

        foreach (var adjective in addAdjectiveTexts)
        {
            currentObjectName += gameData.Adjectives[adjective].uiText + " ";
        }
        
        currentObjectName += addNameText;
        
        return currentObjectName;
    }

    public int GetAddAdjectiveCount()
    {
        return addAdjectiveTexts.Count;
    }

    //카드를 선택한 상태에서 오브젝트를 호버링하면 카드의 타겟으로 설정
    //오브젝트의 이름을 화면에 띄움
    bool isHoverling = false;
    private void OnMouseOver()
    {
        if (GameManager.GetInstance.CurrentState == GameStates.Pause) return;
        if (GameManager.GetInstance.CurrentState == GameStates.Victory &&
            this.name != "PlanetObj") return;
        isHoverling = true;
        if (this.gameObject.CompareTag("InteractObj") && CardManager.GetInstance.isPickCard)
        {
            CardManager.GetInstance.target = this.gameObject;
            
            if (!CheckCountAdjective(CardManager.GetInstance.pickCard.GetComponent<CardController>().GetAdjectiveTypeOfCard()))
            {
                CardManager.GetInstance.ableAddCard = false;
                return;
            }
        }
        if (this.gameObject.CompareTag("InteractObj"))
        {
            PopUpNameOn();
        }
    }

    //마우스가 떠나면 카드의 타겟은 다시 null로 설정
    //오브젝트의 이름을 화면에서 가림 
    private void OnMouseExit()
    {
        if (GameManager.GetInstance.CurrentState == GameStates.Pause) return;
        isHoverling = false;
        CardManager.GetInstance.target = null;
        popUpName.SetActive(false);
        if (this.gameObject.CompareTag("InteractObj"))
        {
            PopUpNameOff();
            CardManager.GetInstance.ableAddCard = true;
        }
    }

    //오브젝트 현재 이름 팝업을 띄움 
    void PopUpNameOn()
    {
        if (GameManager.GetInstance.CurrentState == GameStates.Encyclopedia)
            return;
        popUpName.SetActive(true);
    }

    //오브젝트 현재 이름 팝업을 지움 
    void PopUpNameOff()
    {
        popUpName.SetActive(false);
    }

    //탭키에 따라 모든 네임 팝업을 띄움
     private void AllPopUpNameCtr()
     {
         if (UIManager.GetInstance.isShowNameKeyPressed && !popUpName.activeSelf)
         {
             PopUpNameOn();
         }
         if (!UIManager.GetInstance.isShowNameKeyPressed && popUpName.activeSelf && !isHoverling)
         {
             PopUpNameOff();
         }
     }
#endregion

}
