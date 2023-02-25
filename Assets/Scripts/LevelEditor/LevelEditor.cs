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

    [Header("게임 창")]
    [SerializeField] Image selectSizePanel;
    [SerializeField] GameObject blocksPanel;
    [SerializeField] GameObject sidePanel;
    [SerializeField] GameObject heightPanel;
    [SerializeField] GameObject cardPanel;
    [SerializeField] GameObject warningPanel;

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

    private List<GameObject> tilePrefabs = new List<GameObject>();
    private List<GameObject> objPrefabs = new List<GameObject>();
    private List<GameObject> nameCardPrefabs = new List<GameObject>();
    private List<GameObject> adjCardPrefabs = new List<GameObject>();

    private GameObject[] heights;
    private GameObject[] blankHeights;
    private List<GameObject> startCards = new List<GameObject>();
    private int curY = 0;
    private int curX = 0;
    private int curZ = 0;
    public int blockNum = 0;
    private int preNum = 0;
    public bool isCard = false;
    private int blockLine = 0;

    [Header("한 라인에 나오는 블록 최대 개수")]
    [SerializeField] private int maxBlock = 7;

    [Header("set blocks")]
    [SerializeField] Transform blocksParent;
    [SerializeField] private EditorBlock[] blockBtns;

    GameObject[,,] blocks;
    EditState curState = EditState.SetSize;
    GameObject parentGrounds;
    GameObject parentObjects;

    #endregion

    #region Init
    void Awake()
    {
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
    }

    void Init()
    {
        startCards = new List<GameObject>();
        tilePrefabs = new List<GameObject>();
        objPrefabs = new List<GameObject>();
        nameCardPrefabs = new List<GameObject>();
        adjCardPrefabs = new List<GameObject>();
        SetPrefabs(EBlockType.Object);
        SetPrefabs(EBlockType.Tile);
        SetPrefabs(EBlockType.NameCard);
        SetPrefabs(EBlockType.AdjCard);

        curState = EditState.SetSize;
        UpdateValuesInNewState(curState);
    }
    #endregion

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
        parentGrounds = new GameObject("Grounds");
        parentObjects = new GameObject("Objects");
        for (int y = 0; y < maxY[selectedSize]; y++)
        {
            GameObject newY = new GameObject("Blank " + y.ToString() + "F");
            GameObject blockY = new GameObject(y.ToString() + "F");
            blockY.transform.parent = parentGrounds.transform;
            blankHeights[y] = newY;
            heights[y] = blockY;
            for (int x = 0; x < maxX[selectedSize]; x++)
            {
                for (int z = 0; z < maxZ[selectedSize]; z++)
                {
                    GameObject blank = GameObject.Instantiate(blankBlock, newY.transform);
                    blank.transform.position = new Vector3(x, y + 0.5f, z);
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
                SetBlockInTransform(new Vector3(curX, curY, curZ), EName.Null);
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
        int maxLineNum = Mathf.RoundToInt((float)(blockBtns.Length) / (float)maxBlock) - 1;

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

    public void CreateBlock()
    {
        Vector3 pos = new Vector3(curX, curY, curZ);
        EditorBlock block = blockBtns[blockNum];
        if (isCard)
        {
            bool isName = block.type == EBlockType.NameCard;
            InteractiveObject target = null;
            blocks[curX, curY, curZ].TryGetComponent<InteractiveObject>(out target);
            if (target == null)
            {
                // todo 타일에는 부여 불가 알림 
                return;
            }
            if (isName)
            {
                AddName(target, (EName)block.idx);
            }
            else
            {
                AddAdjective(target, (EAdjective)(block.idx - nameCardPrefabs.Count + 1));
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

    private void ShowBlockLine(int preLine, int curLine)
    {
        for (int i = (preLine * maxBlock); i < ((preLine + 1) * maxBlock); i++)
        {
            blockBtns[i].gameObject.SetActive(false);
        }

        for (int i = (curLine * maxBlock); i < ((curLine + 1) * maxBlock); i++)
        {
            blockBtns[i].gameObject.SetActive(true);
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

        MakeBlanks(selectedSize);
        blankBlock.SetActive(false);

        selectSizePanel.gameObject.SetActive(false);
        ChangeEditState(EditState.MakeStage);
    }

    public void SetBlockInTransform(Vector3 pos, EName block)
    {
        pos = Vector3Int.RoundToInt(pos);
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;

        if (blocks[x, y, z] != null)
        {
            Destroy(blocks[x, y, z]);
        }

        if (block != EName.Null)
        {
            Transform newBlock = GameObject.Instantiate(objPrefabs[(int)block]).transform;
            Destroy(newBlock.GetComponent<Rigidbody>());
            Destroy(newBlock.GetComponent<BoxCollider>());
            newBlock.gameObject.SetActive(true);
            newBlock.position = new Vector3(x, y, z);
            newBlock.transform.parent = parentObjects.transform;
            blocks[x, y, z] = newBlock.gameObject;
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
            Destroy(blocks[x, y, z]);
        }

        if (block != ETile.Null)
        {
            Transform newBlock = GameObject.Instantiate(tilePrefabs[(int)block]).transform;
            Destroy(newBlock.GetComponent<Rigidbody>());
            Destroy(newBlock.GetComponent<BoxCollider>());
            newBlock.gameObject.SetActive(true);
            newBlock.position = new Vector3(x, y, z);
            newBlock.transform.parent = heights[curY].transform;
            blocks[x, y, z] = newBlock.gameObject;
        }

        blockNum = (int)ETile.Null;
    }

    public void AddAdjective(InteractiveObject block, EAdjective adjective)
    {
        block.AddAdjective(adjective);
    }

    public void AddName(InteractiveObject block, EName name)
    {
        block.AddName(name);
    }

    public void ChangeEditState(EditState state)
    {
        curState = state;
        UpdateValuesInNewState(state);
    }

    public void AddStartCard(int idx, GameObject card)
    {
        if (startCards.Count - 1 < idx)
        {
            int count = startCards.Count;
            for (int i = (count - 1); i < idx; i++)
            {
                startCards.Add(null);
            }
            startCards.Add(card);
        }
        else
        {
            if (startCards[idx] != null)
            {
                // todo ui에 기존 카드 제거 
            }
            startCards[idx] = card;
        }
        // todo ui에 추가된 카드 표시 
    }

    public void AddCommand()
    {

    }

    public void SwapBlockORCard()
    {
        isCard = !isCard;

    }
    #endregion

    #region
    public void SizePanelOnOff()
    {
        if (sidePanel.activeSelf)
        {
            sidePanel.SetActive(false);
        }
        else
        {
            sidePanel.SetActive(true);
            cardPanel.SetActive(false);
            heightPanel.SetActive(true);
        }
    }

    public void CardPanelOnOff()
    {
        if (sidePanel.activeSelf)
        {
            sidePanel.SetActive(false);
        }
        else
        {
            sidePanel.SetActive(true);
            cardPanel.SetActive(true);
            heightPanel.SetActive(false);
        }
    }

    public void WarningPanelOnOff()
    {
        if (!warningPanel.activeSelf)
        {
            warningPanel.SetActive(false);
        }
        else
        {
            warningPanel.SetActive(true);
        }
    }

    public void SizePanelOn()
    {
        selectSizePanel.gameObject.SetActive(true);
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
                break;
            case (EditState.MakeStage):
                // play values init
                curX = 0;
                curY = 0;
                curZ = 0;
                ViewCurY();
                pointer.position = new Vector3(curX, curY + 0.5f, curZ);
                blocksPanel.SetActive(true);
                blockNum = 0;
                int preLine = blockLine;
                blockLine = 0;
                ShowBlockLine(preLine, blockLine);
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
        handlerValue.text = curY.ToString() + "F";
    }
}
