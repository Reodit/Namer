using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelEditor : MonoBehaviour
{
    enum MapSize
    {
        SmallSize = 0,
        NormalSize = 1,
        BigSize = 2
    }

    enum EditState
    {
        SetSize = 0,
        SetHeight,
        SetPosition,
        SetBlock
    }

    [SerializeField] Transform pointer;
    [SerializeField] GameObject blankBlock;
    [SerializeField] Button leftBtn;
    [SerializeField] Button rightBtn;
    [SerializeField] Button upBtn;
    [SerializeField] Button downBtn;
    [SerializeField] Button selectBtn;
    [SerializeField] Button cancelBtn;
    [SerializeField] Image selectSizePanel;
    [SerializeField] GameObject blocksPanel;
    [SerializeField] Button[] sizeButtons;

    private int selectedSize = 1;
    [SerializeField] private int[] maxX;
    [SerializeField] private int[] maxY;
    [SerializeField] private int[] maxZ;

    private GameObject[] heights;
    private int curY = 0;
    private int curX = 0;
    private int curZ = 0;
    private int blockNum = 0;

    GameObject[,,] blocks;
    EditState curState = EditState.SetSize;

    void Awake()
    {
        GameManager.GetInstance.ChangeGameState(GameStates.LevelEditMode);
        
        selectSizePanel.gameObject.SetActive(true);
        leftBtn.onClick.AddListener(() => OnClickHorizontalBtn(-1));
        rightBtn.onClick.AddListener(() => OnClickHorizontalBtn(1));
        selectBtn.onClick.AddListener(OnClickSelectBtn);
        cancelBtn.onClick.AddListener(OnClickCancelBtn);
        upBtn.onClick.AddListener(() => OnClickVerticalBtn(1));
        downBtn.onClick.AddListener(() => OnClickVerticalBtn(-1));

        sizeButtons[0].onClick.AddListener(() => SetSize(0));
        sizeButtons[1].onClick.AddListener(() => SetSize(1));
        sizeButtons[2].onClick.AddListener(() => SetSize(2));

        Init();
    }

    void Init()
    {
        curState = EditState.SetSize;
        curX = 0;
        curY = 0;
        curZ = 0;

        pointer.position = new Vector3(curX, curY + 0.5f, curZ);

        selectSizePanel.gameObject.SetActive(true);
        blocksPanel.SetActive(false);
        blockNum = 0;
    }

    void SetSize(int i)
    {
        if (curState != EditState.SetSize) return;

        selectedSize = i;
        heights = new GameObject[maxY[selectedSize]];
        for (int y = 0; y < maxY[selectedSize]; y++)
        {
            GameObject newY = new GameObject("Blank " + y.ToString() + "F");
            heights[y] = newY;
            for (int x = 0; x < maxX[selectedSize]; x++)
            {
                for (int z = 0; z < maxZ[selectedSize]; z++)
                {
                    GameObject blank = GameObject.Instantiate(blankBlock, newY.transform);
                    blank.transform.position = new Vector3(x, y + 0.5f, z);
                }
            }
        }
        blankBlock.SetActive(false);
        curState = EditState.SetHeight;
        selectSizePanel.gameObject.SetActive(false);
        ViewCurY();
        blocks = new GameObject[maxX[selectedSize], maxY[selectedSize], maxZ[selectedSize]];
    }

    private void OnClickHorizontalBtn(int i)
    {
        if (curState == EditState.SetSize)
        {
            int newSize = selectedSize + i;
            if (newSize > 2) newSize = 2;
            else if (newSize < 0) newSize = 0;
            selectedSize = newSize;
        }
        else if (curState == EditState.SetPosition)
        {
            curX += i;
            if (curX > maxX[selectedSize] - 1) curX = maxX[selectedSize] - 1;
            else if (curX < 0) curX = 0;

            pointer.transform.position = new Vector3(curX, pointer.position.y, pointer.position.z);
        }
        else if (curState == EditState.SetBlock)
        {
            ChangeColorBlock(Color.white);
            pointer.GetChild(blockNum).gameObject.SetActive(false);
            blockNum += i;
            if (blockNum > blocksPanel.transform.childCount - 1) blockNum = blocksPanel.transform.childCount - 1;
            else if (blockNum < 0) blockNum = 0;

            ChangeColorBlock(Color.red);
            pointer.GetChild(blockNum).gameObject.SetActive(true);
        }
    }

    private void OnClickVerticalBtn(int i)
    {
        if (curState == EditState.SetSize)
        {
            int newSize = selectedSize + i;
            if (newSize > 2) newSize = 2;
            else if (newSize < 0) newSize = 0;
            selectedSize = newSize;
        }
        else if (curState == EditState.SetHeight)
        {
            curY += i;
            if (curY > maxY[selectedSize] - 1) curY = maxY[selectedSize] - 1;
            else if (curY < 0) curY = 0;

            ViewCurY();
            pointer.position = new Vector3(pointer.position.x, curY + 0.5f, pointer.position.z);
        }
        else if (curState == EditState.SetPosition)
        {
            curZ += i;
            if (curZ > maxZ[selectedSize] - 1) curZ = maxZ[selectedSize] - 1;
            else if (curZ < 0) curZ = 0;

            pointer.transform.position = new Vector3(pointer.position.x, pointer.position.y, curZ);
        }
    }

    private void OnClickCancelBtn()
    {
        if (curState == EditState.SetSize)
        {
            SceneManager.LoadScene(0);
        }
        else if (curState == EditState.SetHeight)
        {
            SceneManager.LoadScene(this.gameObject.scene.name);
        }
        else if (curState == EditState.SetPosition)
        {
            curState = EditState.SetHeight;
        }
        else if (curState == EditState.SetBlock)
        {
            curState = EditState.SetPosition;
            pointer.transform.GetChild(blockNum).gameObject.SetActive(false);
            ChangeColorBlock(Color.white);
            blockNum = 0;
            pointer.transform.GetChild(blockNum).gameObject.SetActive(true);
            blocksPanel.SetActive(false);
            ChangeColorBlock(Color.red);
        }
    }

    private void OnClickSelectBtn()
    {
        if (curState == EditState.SetSize)
        {
            SetSize(selectedSize);
        }
        else if (curState == EditState.SetHeight)
        {
            curState = EditState.SetPosition;
        }
        else if (curState == EditState.SetPosition)
        {
            curState = EditState.SetBlock;
            blocksPanel.SetActive(true);
            pointer.GetChild(blockNum).gameObject.SetActive(true);
            ChangeColorBlock(Color.red);
        }
        else if (curState == EditState.SetBlock)
        {
            pointer.GetChild(blockNum).gameObject.SetActive(false);
            if (blocks[curX, curY, curZ] != null) Destroy(blocks[curX, curY, curZ]);
            ChangeColorBlock(Color.white);
            if (blockNum != 0)
            {
                Transform block = GameObject.Instantiate(pointer.GetChild(blockNum));
                block.gameObject.SetActive(true);
                block.position = new Vector3(curX, curY, curZ);
                blocks[curX, curY, curZ] = block.gameObject;
            }
            blockNum = 0;
            pointer.GetChild(blockNum).gameObject.SetActive(true);
            curState = EditState.SetPosition;
            ChangeColorBlock(Color.red);
        }
    }

    private void ViewCurY()
    {
        for (int idx = 0; idx < heights.Length; idx++)
        {
            if (idx == curY)
            {
                heights[idx].SetActive(true);
            }
            else
            {
                heights[idx].SetActive(false);
            }
        }
    }

    private void ChangeColorBlock(Color color)
    {
        Transform pointerBlock = blocksPanel.transform.GetChild(blockNum);
        pointerBlock.GetComponent<Image>().color = color;
    }

    void Update()
    {
        
    }
}
