using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorBlock : MonoBehaviour
{
    LevelEditor levelEditor;
    public int idx = 0;
    public EBlockType type = EBlockType.Object;
    private Button thisButton;

    int objectCount;
    int tileCount;
    int nameCardCount;
    int adjCardCount;

    int blockCount;
    int cardCount;

    void Awake()
    {
        thisButton = this.gameObject.GetComponent<Button>();
        idx = transform.GetSiblingIndex();
        levelEditor = GameObject.FindObjectOfType<LevelEditor>() as LevelEditor;
    }

    private void Start()
    {
        tileCount = levelEditor.GetCount(EBlockType.Tile);
        objectCount = levelEditor.GetCount(EBlockType.Object);
        nameCardCount = levelEditor.GetCount(EBlockType.NameCard);
        adjCardCount = levelEditor.GetCount(EBlockType.AdjCard);

        blockCount = objectCount + tileCount - 1;
        cardCount = nameCardCount + adjCardCount - 1;

        thisButton.onClick.AddListener(OnClickButton);
    }

    void OnClickButton()
    {
        levelEditor.blockNum = idx;

        if (levelEditor.selectedStartCard != -1)
        {
            levelEditor.AddStartCard(levelEditor.selectedStartCard, this);
        }
        else
        {
            levelEditor.CreateBlock();
        }
    }

    void Update()
    {
        if (levelEditor.isCard)
        {
            if (idx > cardCount)
            {
                this.gameObject.SetActive(false);
                return;
            }

            if (idx < nameCardCount)
            {
                type = EBlockType.NameCard;
            }
            else
            {
                type = EBlockType.AdjCard;
            }
            this.transform.GetChild(1).gameObject.SetActive(false);
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            if (idx > blockCount)
            {
                this.gameObject.SetActive(false);
                return;
            }

            if (idx < tileCount)
            {
                type = EBlockType.Tile;
            }
            else
            {
                type = EBlockType.Object;
            }
            this.transform.GetChild(1).gameObject.SetActive(true);
            this.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
