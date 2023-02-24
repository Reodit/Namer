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
    private Image highLight;

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
        highLight = transform.Find("HighLight").GetComponent<Image>();
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

        levelEditor.CreateBlock();
    }

    void Update()
    {
        if (levelEditor.blockNum != this.idx && highLight.gameObject.activeSelf)
            highLight.gameObject.SetActive(false);
        else if (levelEditor.blockNum == this.idx && !highLight.gameObject.activeSelf)
            highLight.gameObject.SetActive(true);

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
                // todo image 변화
            }
            else
            {
                type = EBlockType.AdjCard;
                // todo image 변화
            }
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
                // todo image 변화
            }
            else
            {
                type = EBlockType.Object;
            }
        }
    }
}
