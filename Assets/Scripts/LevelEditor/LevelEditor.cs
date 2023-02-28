using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public enum EditState
{
    SetSize = 0,
    MakeStage,
    TestPlay
}

public enum EMapSize
{
    SmallSize = 0,
    NormalSize = 1,
    BigSize = 2
}

public enum ETile
{
    Null = 0,
    Glass,
    Ground,
    Sand,
    Snow,
}

public enum EBlockType
{
    Null = -1,
    Object,
    Tile,
    NameCard,
    AdjCard,
    Player
}

[System.Serializable]
public struct Block
{
    public EBlockType type;
    public int idx;

    public Block (EBlockType type, int idx)
    {
        this.type = type;
        this.idx = idx;
    }
}

[System.Serializable]
public struct StartCards
{
    public ECardType type;
    public int idx;

    public StartCards (ECardType type, int idx)
    {
        this.type = type;
        this.idx = idx;
    }
}

public class LevelEditor : MonoBehaviour
{
    #region values
    [Header("버튼 세팅")]
    [SerializeField] Transform pointer;
    [SerializeField] GameObject blankBlock;
    [SerializeField] Button leftBtn;
    [SerializeField] Button rightBtn;
    [SerializeField] Button upBtn;
    [SerializeField] Button downBtn;
    [SerializeField] Button selectBtn;
    [SerializeField] Button cancelBtn;
    [SerializeField] Button[] sizeButtons;
    [SerializeField] Button hideBtn;
    [SerializeField] Button cardBtn;
    [SerializeField] Button playBtn;
    [SerializeField] Slider heightSlider;
    [SerializeField] Button blockLeftBtn;
    [SerializeField] Button blockRightBtn;
    [SerializeField] Text handlerValue;
    [SerializeField] Button[] startCardBtns;

    [Header("게임 창")]
    [SerializeField] Image selectSizePanel;
    [SerializeField] GameObject blocksPanel;
    [SerializeField] GameObject sidePanel;
    [SerializeField] GameObject heightPanel;
    [SerializeField] GameObject cardPanel;
    [SerializeField] GameObject warningResetPanel;
    [SerializeField] GameObject warningPlayPanel;
    [SerializeField] Text[] startCardTexts;
    [SerializeField] GameObject encyclopedia;

    [Header("맵 크기에 따른 사이즈 설정")]
    private int selectedSize = 1;
    [SerializeField] private int[] maxX;
    [SerializeField] private int[] maxY;
    [SerializeField] private int[] maxZ;

    [Header("색 지정")]
    [SerializeField] private Color normalColor;
    [SerializeField] private Color selectedColor;

    [Header("경로 지정")]
    [SerializeField] private string tilePrefabPath;
    [SerializeField] private string objectPrefabPath;
    [SerializeField] private string nameCardPrefabPath;
    [SerializeField] private string adjCardPrefabPath;

    [Header("플레이어 프리팹")]
    [SerializeField] private GameObject playerPrefab;

    private List<GameObject> tilePrefabs = new List<GameObject>();
    private List<GameObject> objPrefabs = new List<GameObject>();
    private List<GameObject> nameCardPrefabs = new List<GameObject>();
    private List<GameObject> adjCardPrefabs = new List<GameObject>();

    private GameObject[] heights;
    private GameObject[] blankHeights;
    private List<EditorBlock> setCards = new List<EditorBlock>();
    private SCardView startCards;
    [SerializeField] public static Block[,,] preBlocks;
    [SerializeField] public static StartCards[] preStartCards;
    private int curY = 0;
    private int curX = 0;
    private int curZ = 0;
    public int blockNum = 0;
    public int selectedStartCard = 0;
    private int preNum = 0;
    public bool isCard = false;
    private int blockLine = 0;

    [Header("한 라인에 나오는 블록 최대 개수")]
    [SerializeField] private int maxBlock = 7;

    [Header("set blocks")]
    [SerializeField] Transform blocksParent;
    [SerializeField] private EditorBlock[] blockBtns;
    [SerializeField] private EditorBlock[] cardBtns;

    GameObject[,,] blocks;
    List<Transform> tiles = new List<Transform>();
    List<InteractiveObject> objects = new List<InteractiveObject>();
    static Vector3 stageStartPoint;
    EditState curState = EditState.SetSize;
    GameObject parentGrounds;
    GameObject parentObjects;
    GameObject parentBlanks;
    static bool isSaved = false;
    Transform sPoint = null;
    #endregion

    #region Init
    void Awake()
    {
        Destroy(DetectManager.GetInstance);
        GameManager.GetInstance.ChangeGameState(GameStates.LevelEditMode);
        
        SetAllBtnListener();
        Init();
    }

    void SetAllBtnListener()
    {
        selectSizePanel.gameObject.SetActive(true);
        leftBtn.onClick.AddListener(() => OnClickHorizontalBtn(-1));
        rightBtn.onClick.AddListener(() => OnClickHorizontalBtn(1));
        selectBtn.onClick.AddListener(OnClickCardBtn);
        cancelBtn.onClick.AddListener(OnClickCancelBtn);
        upBtn.onClick.AddListener(() => OnClickVerticalBtn(1));
        downBtn.onClick.AddListener(() => OnClickVerticalBtn(-1));

        sizeButtons[0].onClick.AddListener(() => SetSize((EMapSize)0));
        sizeButtons[1].onClick.AddListener(() => SetSize((EMapSize)1));
        sizeButtons[2].onClick.AddListener(() => SetSize((EMapSize)2));

        blockLeftBtn.onClick.AddListener(() => OnClickArrow(-1));
        blockRightBtn.onClick.AddListener(() => OnClickArrow(1));

        heightSlider.onValueChanged.AddListener((v) =>
        {
            curY = Mathf.RoundToInt(v);
            ViewCurY();
        });

        for (int i = 0; i < startCardBtns.Length; i++)
        {
            int idx = i;
            startCardBtns[idx].onClick.AddListener(() =>
            {
                selectedStartCard = idx;
                OnClickStartCard(idx);
            });
        }
    }

    void Init()
    {
        GameManager.GetInstance.SetCustomLevel(GameDataManager.GetInstance.CustomLevelDataDic.Count);
        
        stageStartPoint = new Vector3(0, 2, 0);
        startCards = new SCardView(new List<EName>(), new List<EAdjective>());
        tilePrefabs = new List<GameObject>();
        objPrefabs = new List<GameObject>();
        nameCardPrefabs = new List<GameObject>();
        adjCardPrefabs = new List<GameObject>();
        SetPrefabs(EBlockType.Object);
        SetPrefabs(EBlockType.Tile);
        SetPrefabs(EBlockType.NameCard);
        SetPrefabs(EBlockType.AdjCard);

        if (isSaved)
        {
            curState = EditState.SetSize;
            UpdateValuesInNewState(curState);
            LoadPreMap();
            LoadPreCards();
        }
        else
        {
            curState = EditState.SetSize;
            UpdateValuesInNewState(curState);
        }
    }
    #endregion

    public void LoadPreMap()
    {
        LoadArray();
        SetStartPoint(stageStartPoint);
        isSaved = false;
    }

    private void LoadPreCards()
    {
        for (int i = 0; i < preStartCards.Length; i++)
        {
            StartCards sCard = preStartCards[i];
            ECardType type = sCard.type;
            int cardIdx = type == ECardType.Name ? sCard.idx : sCard.idx - GetCount(EBlockType.NameCard);
            AddStartCard(i, cardBtns[cardIdx]);
        }
    }

    private void LoadArray()
    {
        int maxLength = preBlocks.GetLength(0);
        selectedSize = maxLength <= maxX[0] ? 0 : (maxLength <= maxX[1] ? 1 : 2);
        SetSize((EMapSize)selectedSize);
        ViewCurY();

        for (int x = 0; x < preBlocks.GetLength(0); x++)
        {
            for (int y = 0; y < preBlocks.GetLength(1); y++)
            {
                for (int z = 0; z < preBlocks.GetLength(2); z++)
                {
                    if (preBlocks[x, y, z].type == EBlockType.Null)
                        continue;

                    int idx = preBlocks[x, y, z].idx;
                    if (idx == 0)
                        continue;
                    
                    if (preBlocks[x, y, z].type == EBlockType.Object)
                    {
                        GameObject block = GameObject.Instantiate(objPrefabs[idx]);
                        objects.Add(block.GetComponent<InteractiveObject>());
                        block.transform.position = new Vector3(x, y, z);
                        Destroy(block.GetComponent<Rigidbody>());
                        block.SetActive(true);
                        block.transform.parent = parentObjects.transform;
                        blocks[x, y, z] = block;
                    }
                    else
                    {
                        GameObject block = GameObject.Instantiate(tilePrefabs[idx]);
                        tiles.Add(block.transform);
                        block.transform.position = new Vector3(x, y, z);
                        block.SetActive(true);
                        block.transform.parent = heights[y].transform;
                        blocks[x, y, z] = block;
                    }
                }
            }
        }
    }

    public void GoTestPlay()
    {
        if (objects.Count == 0 || tiles.Count == 0)
        {
            return;
        }

        isSaved = true;
        ShowPointer(false);
        ShowBlanks(false);
        GameDataManager.GetInstance.ReadMapData();

        int level = GameManager.GetInstance.CustomLevel + 1;
        SLevelData customLevelData = new SLevelData(level, "CustomLevel" + level, new SPosition(stageStartPoint), SetStartCards()); GameDataManager.GetInstance.AddCustomLevelData(level, customLevelData);
        
        SceneBehaviorManager.LoadScene(Scenes.LevelDesign, LoadSceneMode.Single);
    }

    public int GetCount(EBlockType type)
    {
        switch (type)
        {
            case (EBlockType.Object):
                return objPrefabs.Count;
            case (EBlockType.Tile):
                return tilePrefabs.Count;
            case (EBlockType.NameCard):
                return nameCardPrefabs.Count;
            case (EBlockType.AdjCard):
                return adjCardPrefabs.Count;
            case (EBlockType.Null):
            default:
                return 0;
        }
    }

    private GameObject GetSelectedObj(EBlockType selectedType, int idx)
    {
        switch (selectedType)
        {
            case (EBlockType.Object):
                return objPrefabs[idx];
            case (EBlockType.Tile):
                return tilePrefabs[idx];
            case (EBlockType.NameCard):
                return nameCardPrefabs[idx];
            case (EBlockType.AdjCard):
                return adjCardPrefabs[idx];
            case (EBlockType.Null):
            default:
                return null;
        }
    }

    private void MakeBlanks(int size)
    {
        parentBlanks = new GameObject("Blanks");
        parentGrounds = new GameObject("Grounds");
        parentObjects = new GameObject("Objects");
        for (int y = 0; y < maxY[selectedSize]; y++)
        {
            GameObject newY = new GameObject("Blank " + y.ToString() + "F");
            GameObject blockY = new GameObject(y.ToString() + "F");
            newY.transform.parent = parentBlanks.transform;
            blockY.transform.parent = parentGrounds.transform;
            blankHeights[y] = newY;
            heights[y] = blockY;
            for (int x = 0; x < maxX[selectedSize]; x++)
            {
                for (int z = 0; z < maxZ[selectedSize]; z++)
                {
                    GameObject blank = GameObject.Instantiate(blankBlock, newY.transform);
                    blank.transform.position = new Vector3(x, y + 0.5f, z);
                    blank.SetActive(true);
                }
            }
        }

        heightSlider.maxValue = maxY[selectedSize] - 1;
    }

    private void OnClickHorizontalBtn(int i)
    {
        switch (curState)
        {
            case (EditState.SetSize):
                // can't touch
                break;
            case (EditState.MakeStage):
                curX += i;
                if (curX > maxX[selectedSize] - 1) curX = maxX[selectedSize] - 1;
                else if (curX < 0) curX = 0;
                pointer.transform.position = new Vector3(curX, pointer.position.y, pointer.position.z);
                break;
            case (EditState.TestPlay):
                // todo testPlay
                break;
        }
    }

    private void OnClickVerticalBtn(int i)
    {
        switch (curState)
        {
            case (EditState.SetSize):
                
                break;
            case (EditState.MakeStage):
                curZ += i;
                if (curZ > maxZ[selectedSize] - 1) curZ = maxZ[selectedSize] - 1;
                else if (curZ < 0) curZ = 0;

                pointer.transform.position = new Vector3(pointer.position.x, pointer.position.y, curZ);
                break;
            case (EditState.TestPlay):
                // todo testPlay
                break;
        }
    }

    private void OnClickCancelBtn()
    {
        switch (curState)
        {
            case (EditState.SetSize):

                break;
            case (EditState.MakeStage):
                if (isCard)
                {
                    if (selectedStartCard == -1)
                    {
                        InteractiveObject io;
                        blocks[curX, curY, curZ].TryGetComponent<InteractiveObject>(out io);
                        if (io != null)
                            ClearName(io);
                    }
                    else
                    {
                        AddStartCard(selectedStartCard, null);
                    }
                }
                else
                {
                    SetNullInTransform(new Vector3(curX, curY, curZ));
                }
                break;
            case (EditState.TestPlay):
                // todo testPlay
                break;
        }
    }

    private void OnClickCardBtn()
    {
        switch (curState)
        {
            case (EditState.SetSize):
                // no function in this state
                break;
            case (EditState.MakeStage):
                SwapBlockORCard();
                break;
            case (EditState.TestPlay):
                // todo testPlay
                break;
        }
    }

    private void OnClickArrow(int dir)
    {
        int preLine = blockLine;
        blockLine += dir;
        int maxLineNum;
        if (isCard)
        {
            maxLineNum = Mathf.FloorToInt((float)(cardBtns.Length) / (float)maxBlock) - 1;
            if (Mathf.RoundToInt((float)(cardBtns.Length) % (float)maxBlock) > 0)
                maxLineNum += 1;
        }
        else
        {
            maxLineNum = Mathf.FloorToInt((float)(blockBtns.Length) / (float)maxBlock) - 1;
            if (Mathf.RoundToInt((float)(blockBtns.Length) % (float)maxBlock) > 0)
                maxLineNum += 1;
        }

        if (blockLine > maxLineNum)
        {
            blockLine = maxLineNum;
        }
        else if (blockLine < 0)
        {
            blockLine = 0;
        }

        ShowBlockLine(preLine, blockLine);
    }

    public void OnClickEncyclopediaBtn()
    {
        GameManager.GetInstance.ChangeGameState(GameStates.Encyclopedia);
        encyclopedia.SetActive(true);
    }

    public void OnClickStartCard(int idx)
    {
        for (int c = 0; c < startCardBtns.Length; c++)
        {
            if (c == idx)
                startCardBtns[c].transform.Find("HighLight").gameObject.SetActive(true);
            else
                startCardBtns[c].transform.Find("HighLight").gameObject.SetActive(false);
        }
    }

    public void ReturnLobby()
    {
        SoundManager.GetInstance.Play("BtnPress");
        GameManager.GetInstance.ChangeGameState(GameStates.LevelSelect);
        SceneManager.LoadScene("MainScene");
    }

    public void ShowInfos()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ShowBlockLine(int preLine, int curLine)
    {
        for (int i = (preLine * maxBlock); i < ((preLine + 1) * maxBlock); i++)
        {
            if (isCard)
            {
                if (cardBtns.Length > i)
                    cardBtns[i].gameObject.SetActive(false);
            }
            else
            {
                if (blockBtns.Length > i)
                    blockBtns[i].gameObject.SetActive(false);
            }
        }

        for (int i = (curLine * maxBlock); i < ((curLine + 1) * maxBlock); i++)
        {
            if (isCard)
            {
                if (cardBtns.Length > i)
                    cardBtns[i].gameObject.SetActive(true);
            }
            else
            {
                if (blockBtns.Length > i)
                    blockBtns[i].gameObject.SetActive(true);
            }
        }
    }

    private void ShowBlockLine(int curLine)
    {
        for (int i = 0; i < blocksParent.childCount; i++)
        {
            blocksParent.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = (curLine * maxBlock); i < ((curLine + 1) * maxBlock); i++)
        {
            if (isCard)
            {
                if (cardBtns.Length > i)
                    cardBtns[i].gameObject.SetActive(true);
            }
            else
            {
                if (blockBtns.Length > i)
                    blockBtns[i].gameObject.SetActive(true);
            }
        }
    }

    private void ViewCurY()
    {
        for (int idx = 0; idx < blankHeights.Length; idx++)
        {
            if (idx == curY)
            {
                blankHeights[idx].SetActive(true);
            }
            else
            {
                blankHeights[idx].SetActive(false);
            }
        }
        pointer.position = new Vector3(curX, curY + 0.5f, curZ);
    }

    public void ShowPointer(bool show)
    {
        pointer.gameObject.SetActive(show);
    }

    public void ShowBlanks(bool show)
    {
        if (show)
        {
            ViewCurY();
        }
        else
        {
            foreach (GameObject blank in blankHeights)
            {
                blank.SetActive(false);
            }
        }
    }

    #region GetPrefabs
    private void SetPrefabs(EBlockType type)
    {
        System.Type enumType = typeof(ETile);
        switch (type)
        {
            case (EBlockType.Object):
                enumType = typeof(EName);
                SetValueInObjects(System.Enum.GetValues(enumType).Length);
                break;
            case (EBlockType.NameCard):
                enumType = typeof(EName);
                SetValueInNameCards(System.Enum.GetValues(enumType).Length);
                break;
            case (EBlockType.Tile):
                enumType = typeof(ETile);
                SetValueInTiles(System.Enum.GetValues(enumType).Length);
                break;
            case (EBlockType.AdjCard):
                enumType = typeof(EAdjective);
                SetValueInAdjCards(System.Enum.GetValues(enumType).Length);
                break;
            case (EBlockType.Player):
                // todo 플레이어 추가 
                break;
            case (EBlockType.Null):
            default:
                return;
        }
    }

    private void SetValueInObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (i == 0)
            {
                objPrefabs.Add(null);
                continue;
            }
            GameObject obj = GetPrefab(EBlockType.Object, ((EName)i).ToString());
            objPrefabs.Add(obj);
        }
    }

    private void SetValueInTiles(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (i == 0)
            {
                tilePrefabs.Add(null);
                continue;
            }
            GameObject obj = GetPrefab(EBlockType.Tile, ((ETile)i).ToString());
            tilePrefabs.Add(obj);
        }
    }

    private void SetValueInNameCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (i == 0)
            {
                nameCardPrefabs.Add(null);
                continue;
            }
            GameObject obj = GetPrefab(EBlockType.NameCard, ((EName)i).ToString());
            nameCardPrefabs.Add(obj);
        }
    }

    private void SetValueInAdjCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (i == 0)
            {
                adjCardPrefabs.Add(null);
                continue;
            }
            GameObject obj = GetPrefab(EBlockType.AdjCard, ((EAdjective)i).ToString());
            adjCardPrefabs.Add(obj);
        }
    }

    private GameObject GetPrefab(EBlockType type, string prefabName)
    {
        string path = "";
        switch (type)
        {
            case (EBlockType.Null):
                return null;
            case (EBlockType.Object):
                path = objectPrefabPath + prefabName + "Obj";
                break;
            case (EBlockType.Tile):
                path = tilePrefabPath + prefabName + "Tile";
                break;
            case (EBlockType.NameCard):
                path = nameCardPrefabPath + prefabName + "Card";
                break;
            case (EBlockType.AdjCard):
                path = adjCardPrefabPath + prefabName + "Card";
                break;
            case (EBlockType.Player):
                // todo 플레이어 프리팹 어디있는지 몰라서 일단 비움 
                break;
            default:
                return null;
        }
        if (path != "")
        {
            return Resources.Load(path) as GameObject;
        }
        else
        {
            return null;
        }
    }
    #endregion

    #region public functions
    public void SetSize(EMapSize mapSize)
    {
        if (curState != EditState.SetSize) return;

        selectedSize = (int)mapSize;
        heights = new GameObject[maxY[selectedSize]];
        blankHeights = new GameObject[maxY[selectedSize]];
        blocks = new GameObject[maxX[selectedSize], maxY[selectedSize], maxZ[selectedSize]];
        if (!isSaved) preBlocks = new Block[maxX[selectedSize], maxY[selectedSize], maxZ[selectedSize]];

        MakeBlanks(selectedSize);
        blankBlock.SetActive(false);

        selectSizePanel.gameObject.SetActive(false);
        ChangeEditState(EditState.MakeStage);
    }

    public void SetNullInTransform(Vector3 pos)
    {
        pos = Vector3Int.RoundToInt(pos);
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;

        if (blocks[x, y, z] != null)
        {
            Transform blockT = blocks[x, y, z].transform;
            DeleteBlockInArray((blockT));
            
            Destroy(blocks[x, y, z]);
            preBlocks[x, y, z] = new Block(EBlockType.Null, 0);
        }

        blockNum = (int)EName.Null;
    }

    public void SetBlockInTransform(Vector3 pos, EName block)
    {
        pos = Vector3Int.RoundToInt(pos);
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;

        if (blocks[x, y, z] != null)
        {
            Transform blockT = blocks[x, y, z].transform;
            DeleteBlockInArray((blockT));

            if (blocks[x, y, z] != sPoint.gameObject)
                Destroy(blocks[x, y, z]);
        }

        if (block != EName.Null)
        {
            Transform newBlock = GameObject.Instantiate(objPrefabs[(int)block]).transform;
            Destroy(newBlock.GetComponent<Rigidbody>());
            newBlock.gameObject.SetActive(true);
            newBlock.position = new Vector3(x, y, z);
            newBlock.transform.parent = parentObjects.transform;
            blocks[x, y, z] = newBlock.gameObject;
            objects.Add(newBlock.GetComponent<InteractiveObject>());

            preBlocks[x, y, z] = new Block(EBlockType.Object, (int)block);

            //InteractiveObject blockIO = newBlock.GetComponent<InteractiveObject>();
            //AddName(blockIO, EName.Null);
        }
        else
        {
            SetStartPoint(pos);
        }

        blockNum = (int)EName.Null;
    }

    public void SetBlockInTransform(Vector3 pos, ETile block)
    {
        pos = Vector3Int.RoundToInt(pos);
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;

        if (blocks[x, y, z] != null)
        {
            Transform blockT = blocks[x, y, z].transform;
            DeleteBlockInArray((blockT));

            if (blocks[x,y,z] != sPoint.gameObject)
                Destroy(blocks[x, y, z]);
        }

        if (block != ETile.Null)
        {
            Transform newBlock = GameObject.Instantiate(tilePrefabs[(int)block]).transform;
            Destroy(newBlock.GetComponent<Rigidbody>());
            newBlock.gameObject.SetActive(true);
            newBlock.position = new Vector3(x, y, z);
            newBlock.transform.parent = heights[curY].transform;
            blocks[x, y, z] = newBlock.gameObject;
            tiles.Add(newBlock);

            preBlocks[x, y, z] = new Block(EBlockType.Tile, (int)block);
        }
        else
        {
            SetStartPoint(pos);
        }

        blockNum = (int)ETile.Null;
    }

    private void DeleteBlockInArray(Transform blockT)
    {
        InteractiveObject io;
        blockT.TryGetComponent<InteractiveObject>(out io);
        if (tiles.Contains(blockT))
        {
            tiles.Remove(blockT);
        }
        else if (io != null && objects.Contains(io))
        {
            objects.Remove(io);
        }
    }

    public void SetStartPoint(Vector3 pos)
    {
        Vector3Int posInt = Vector3Int.RoundToInt(pos);
        if (sPoint == null)
        {
            sPoint = GameObject.Instantiate(playerPrefab).transform;
        }
        sPoint.gameObject.SetActive(true);
        sPoint.position = posInt;
        stageStartPoint = posInt;
        blocks[posInt.x, posInt.y, posInt.z] = sPoint.gameObject;
    }

    public void CreateBlock()
    {
        Vector3 pos = new Vector3(curX, curY, curZ);
        EditorBlock block = isCard? cardBtns[blockNum] : blockBtns[blockNum];
        if (isCard)
        {
            bool isName = block.type == EBlockType.NameCard;
            InteractiveObject target = null;
            if (blocks[curX, curY, curZ] != null)
                blocks[curX, curY, curZ].TryGetComponent<InteractiveObject>(out target);
            if (target == null)
            {
                // todo 타일에는 부여 불가 알림
                // todo 사운드나 X표시 출력 
                return;
            }
            if (isName)
            {
                AddName(target, (EName)block.idx);
            }
            else if (blockNum != 0)
            {
                AddAdjective(target, (EAdjective)(block.idx - nameCardPrefabs.Count + 1));
            }
            else
            {
                AddName(target, (EName)0);
            }
        }
        else
        {
            bool isTile = block.type == EBlockType.Tile;
            if (isTile)
            {
                SetBlockInTransform(pos, (ETile)block.idx);
            }
            else
            {
                SetBlockInTransform(pos, (EName)(block.idx - tilePrefabs.Count + 1));
            }
        }
    }

    public void ClearMap()
    {
        if (parentBlanks != null) Destroy(parentBlanks);
        if (parentGrounds != null) Destroy(parentGrounds);
        if (parentObjects != null) Destroy(parentObjects);
        Vector3Int playerPos = Vector3Int.RoundToInt(stageStartPoint);
        if (blocks != null && blocks[playerPos.x, playerPos.y, playerPos.z] != null)
            Destroy(blocks[playerPos.x, playerPos.y, playerPos.z]);
    }

    public void AddAdjective(InteractiveObject block, EAdjective adjective)
    {
        block.AddAdjective(adjective);
    }

    public void AddName(InteractiveObject block, EName name)
    {
        block.AddName(name);
    }

    public void ClearName(InteractiveObject block)
    {
        block.AddName(EName.Null);

        while (true)
        {
            if (block.addAdjectiveTexts.Count == 0)
            {
                break;
            }
            
            block.SubtractAdjective(block.addAdjectiveTexts.First());
            block.addAdjectiveTexts.RemoveFirst();
        }
    }

    public void ChangeEditState(EditState state)
    {
        curState = state;
        UpdateValuesInNewState(state);
    }

    public void AddStartCard(int idx, EditorBlock card)
    {
        setCards[idx] = card;
        int nameCount = GetCount(EBlockType.NameCard);
        //string name = (card.idx >= nameCount ? ((EAdjective)(card.idx - nameCount + 1)).ToString() : ((EName)card.idx).ToString());
        Text textComponent = null;
        if (card != null) card.transform.GetChild(0).GetChild(0).TryGetComponent<Text>(out textComponent);
        string name = textComponent == null ? "???" : textComponent.text;
        startCardTexts[idx].text = name;
        ECardType cardType = card.idx > nameCount ? ECardType.Adjective : ECardType.Name;
        preStartCards[idx] = new StartCards(cardType, card.idx);
    }

    public SCardView SetStartCards()
    {
        foreach (EditorBlock card in setCards)
        {
            int nameCount = GetCount(EBlockType.NameCard);
            bool isName;
            int idx = 0;
            if (card == null)
            {
                isName = true;
                idx = 0;
            }
            else
            {
                isName = card.idx < nameCount;
                idx = card.idx;
            }

            if (isName)
            {
                if (idx == (int)EName.Null)
                {
                    continue;
                }
                startCards.nameRead.Add((EName)idx);
            }
            else
            {
                int id = card.idx - nameCount + 1;
                startCards.adjectiveRead.Add((EAdjective)id);
            }
        }

        return startCards;
    }

    public void AddCommand()
    {

    }

    public void SwapBlockORCard()
    {
        isCard = !isCard;
        blockLine = 0;
        selectedStartCard = -1;
        ShowBlockLine(blockLine);
    }
    #endregion

    #region
    public void SizePanelOnOff()
    {
        if (sidePanel.activeSelf)
        {
            selectedStartCard = -1;
            sidePanel.SetActive(false);
        }
        else
        {
            sidePanel.SetActive(true);
            cardPanel.SetActive(false);
            heightPanel.SetActive(true);
            warningResetPanel.SetActive(false);
            warningPlayPanel.SetActive(false);
            selectedStartCard = -1;
        }
    }

    public void CardPanelOnOff()
    {
        if (sidePanel.activeSelf)
        {
            selectedStartCard = -1;
            sidePanel.SetActive(false);
        }
        else
        {
            sidePanel.SetActive(true);
            cardPanel.SetActive(true);
            heightPanel.SetActive(false);
            warningResetPanel.SetActive(false);
            warningPlayPanel.SetActive(false);

            if (!isCard)
            {
                SwapBlockORCard();
                selectedStartCard = 0;
            }
        }
    }

    public void WarningResetPanelOnOff()
    {
        if (warningResetPanel.activeSelf)
        {
            warningResetPanel.SetActive(false);
        }
        else
        {
            warningResetPanel.SetActive(true);
            warningPlayPanel.SetActive(false);
            selectedStartCard = -1;
        }
    }

    public void WarningPlayPanelOnOff()
    {
        if (warningPlayPanel.activeSelf)
        {
            warningPlayPanel.SetActive(false);
        }
        else
        {
            warningPlayPanel.SetActive(true);
            warningResetPanel.SetActive(false);
            selectedStartCard = -1;
        }
    }

    public void SizePanelOn()
    {
        selectSizePanel.gameObject.SetActive(true);
        ChangeEditState(EditState.SetSize);
    }
    #endregion

    #region StateMachine
    private void UpdateValuesInNewState(EditState state)
    {
        switch (state)
        {
            case (EditState.SetSize):
                // size init, stage blocks init
                selectSizePanel.gameObject.SetActive(true);
                blocksPanel.SetActive(false);
                blockNum = 0;
                ClearMap();
                setCards = new List<EditorBlock>(4) { null, null, null, null };
                if (!isSaved)
                {
                    preStartCards = new StartCards[4];
                    for (int i = 0; i < preStartCards.Length; i++)
                    {
                        AddStartCard(i, cardBtns[0]);
                    }
                }
                break;
            case (EditState.MakeStage):
                // play values init
                curX = 0;
                curY = 0;
                curZ = 0;
                stageStartPoint = new Vector3(0, 2, 0);
                ViewCurY();
                pointer.position = new Vector3(curX, curY + 0.5f, curZ);
                blocksPanel.SetActive(true);
                blockNum = 0;
                int preLine = blockLine;
                blockLine = 0;
                ShowBlockLine(preLine, blockLine);
                selectedStartCard = -1;
                objects = new List<InteractiveObject>();
                tiles = new List<Transform>();
                break;
            case (EditState.TestPlay):
                // ??
                break;
            default:
                break;
        }
    }
    #endregion

    void Update()
    {
        handlerValue.text = (curY + 1).ToString() + "F";

        if (isCard && blocks[curX, curY, curZ] != null)
        {
            InteractiveObject io;
            blocks[curX, curY, curZ].TryGetComponent<InteractiveObject>(out io);

            foreach (InteractiveObject ioObj in objects)
            {
                if (ioObj == null) continue;
                if (ioObj != io)
                    ioObj.PopUpNameOff();
                else
                    ioObj.PopUpNameOn();
            }
        }
        else
        {
            foreach (InteractiveObject ioObj in objects)
            {
                if (ioObj != null)
                    ioObj.PopUpNameOff();
            }
        }
    }
}
