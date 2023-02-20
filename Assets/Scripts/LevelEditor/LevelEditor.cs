using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    enum MapSize
    {
        SmallSize = 0,
        NormalSize = 1,
        BigSize = 2
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
    [SerializeField] Text sizeText;

    private int selectedSize = 1;
    [SerializeField] private int[] maxX;
    [SerializeField] private int[] maxY;
    [SerializeField] private int[] maxZ;

    private GameObject[] heights;
    private int curY = 0;
    private int curX = 0;
    private int curZ = 0;

    void Awake()
    {
        selectSizePanel.gameObject.SetActive(true);
        leftBtn.onClick.AddListener(() => OnClickHorizontalBtn(-1));
        rightBtn.onClick.AddListener(() => OnClickHorizontalBtn(1));
        selectBtn.onClick.AddListener(OnClickSelectBtn);
        upBtn.onClick.AddListener(() => OnClickVerticalBtn(1));
        downBtn.onClick.AddListener(() => OnClickVerticalBtn(-1));
    }

    private void OnClickHorizontalBtn(int i)
    {
        if (selectSizePanel.gameObject.activeSelf)
        {
            int newSize = selectedSize + i;
            if (newSize > 2) newSize = 2;
            else if (newSize < 0) newSize = 0;
            selectedSize = newSize;
        }
        else
        {

        }
    }

    private void OnClickVerticalBtn(int i)
    {
        if (selectSizePanel.gameObject.activeSelf)
        {
            int newSize = selectedSize + i;
            if (newSize > 2) newSize = 2;
            else if (newSize < 0) newSize = 0;
            selectedSize = newSize;
        }
        else
        {
            curY += i;
            if (curY > maxY[selectedSize] - 1) curY = maxY[selectedSize] - 1;
            else if (curY < 0) curY = 0;

            ViewCurY();
            pointer.position = new Vector3(pointer.position.x, curY, pointer.position.z);
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

    private void OnClickSelectBtn()
    {
        heights = new GameObject[maxY[selectedSize]];
        if (selectSizePanel.gameObject.activeSelf)
        {
            for (int y = 0; y < maxY[selectedSize]; y++)
            {
                GameObject newY = new GameObject("Blank " + y.ToString() + "F");
                heights[y] = newY;
                for (int x = 0; x < maxX[selectedSize]; x++)
                {
                    for (int z = 0; z < maxZ[selectedSize]; z++)
                    {
                        GameObject blank = GameObject.Instantiate(blankBlock, newY.transform);
                        blank.transform.position = new Vector3(x, y, z);
                    }
                }
            }
            blankBlock.SetActive(false);
            selectSizePanel.gameObject.SetActive(false);
            ViewCurY();
        }
        else
        {

        }
    }

    void Update()
    {
        sizeText.text = ((MapSize)selectedSize).ToString();
    }
}
