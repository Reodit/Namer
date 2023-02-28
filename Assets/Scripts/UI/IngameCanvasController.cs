using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameCanvasController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameObject encyclopedia;
    [SerializeField] GameObject topButtons;
    [SerializeField] GameObject bottomButtons;
    [SerializeField] GameObject joyStick;
    [SerializeField] GameObject pediaBtn;
    [SerializeField] GameObject optionBtn;
    [SerializeField] GameObject gameOptionPanel;
    [SerializeField] GameObject topPanel;
    [SerializeField] GameObject cardBtnImg;
    [SerializeField] GameObject tutorialArrow;
    [SerializeField] Text stageName;
    [SerializeField] Image interactionImg;
    [SerializeField] Image cameraViewImg;
    PlayerMovement playerMovement;
    Canvas canvas;

    bool isCardVisible = true;
    #region ResetRelatedVal
    [SerializeField] GameObject LoadingImg;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Update()
    {
        CardToggleOnOff();
#if UNITY_ANDROID
        CameraViewBtnOnOff();
#endif
    }

    void Init()
    {
        canvas = this.gameObject.GetComponent<Canvas>();
        canvas.worldCamera =
            Camera.main.transform.Find("UICamera").gameObject.GetComponent<Camera>();
        StageNameSetUp();
        encyclopedia = Camera.main.gameObject.transform.GetChild(2).gameObject;
        UIManager.GetInstance.isShowNameKeyPressed = false;
        CardManager.GetInstance.pickCard = null;
        CardManager.GetInstance.isPickCard = false;
        CardManager.GetInstance.target = null;
    }

    bool isArrowOff;
    public void EncyclopediaOpen()
    {
        if (GameManager.GetInstance.CurrentState == GameStates.Victory ||
            CardManager.GetInstance.isCasting)
        {
            EncyclopediaClose();
        }
        if (tutorialArrow.activeInHierarchy)
        {
            tutorialArrow.SetActive(false);
            isArrowOff = true;
        }
        GameManager.GetInstance.isPlayerCanInput = false;
        encyclopedia.SetActive(true);
        topButtons.SetActive(false);
        bottomButtons.SetActive(false);
        joyStick.SetActive(false);
        topPanel.SetActive(false);
    }

    public void EncyclopediaClose()
    {
        if (isArrowOff)
        {
            tutorialArrow.SetActive(true);
            isArrowOff = false;
        }
        if (!GameManager.GetInstance.cameraController.isFocused)
        {
            GameManager.GetInstance.isPlayerCanInput = true;
        }
        encyclopedia.SetActive(false);
#if UNITY_ANDROID
        joyStick.SetActive(true);
        bottomButtons.SetActive(true);
#endif
        topButtons.SetActive(true);
        topPanel.SetActive(true);
        GameManager.GetInstance.ChangeGameState(GameStates.InGame);
        CardManager.GetInstance.CardsUp();
        isCardVisible = true;
        cardBtnImg.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OptionBtn()
    {
        SoundManager.GetInstance.Play("BtnPress");
        UIManager.GetInstance.UIOn();
    }

    public void StartBtn()
    {
        SoundManager.GetInstance.Play("BtnPress");
        UIManager.GetInstance.UIOff();
    }

    public void RestartBtn()
    {
        SoundManager.GetInstance.Play("BtnPress");
        bottomButtons.SetActive(false);
        UIManager.GetInstance.UIOff();
        GameManager.GetInstance.ResetCurrentLvl();
    }

    public void ReturnLobby()
    {
        SoundManager.GetInstance.Play("BtnPress");
        UIManager.GetInstance.UIOff();
        GameManager.GetInstance.ChangeGameState(GameStates.LevelSelect);
        SceneManager.LoadScene("MainScene");
    }

    public void GameOff()
    {
        Application.Quit();
    }

    public void GameOptionPanelOn()
    {
        gameOptionPanel.SetActive(true);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (GameObject obj in eventData.hovered)
        {
            if (obj.name == "Dialog")
            {
                OnPointerExit(eventData);
                return;
            }
        }

        GameManager.GetInstance.scenarioController.isUI = true;
    } 

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.GetInstance.scenarioController.isUI = false;
    }

    public void SetLoadingImage(float fillValue)
    {
        LoadingImg.GetComponent<Image>().fillAmount = fillValue;
    }

    public void TurnOnAndOffLoadingImg(bool switchTurn)
    {
        LoadingImg.SetActive(switchTurn);
    }

    void StageNameSetUp()
    {
        string currentName =
            GameDataManager.GetInstance.GetLevelName(GameManager.GetInstance.Level);

        if (currentName == "")
        {
            stageName.text = $"{(GameManager.GetInstance.Level)}" + " Stage";
        }
        else if (currentName != "")
        {
            stageName.text = currentName;
        }
    }

    public void PediaButton()
    {
        if (GameManager.GetInstance.CurrentState == GameStates.Victory ||
        CardManager.GetInstance.isCasting)
        return;
        GameManager.GetInstance.ChangeGameState(GameStates.Encyclopedia);
        SoundManager.GetInstance.Play("BtnPress");
        if (isCardVisible)
        {
            CardManager.GetInstance.CardsDown();
            isCardVisible = false;
            cardBtnImg.transform.localScale = new Vector3(1, -1, 1);
        }
        Invoke("EncyclopediaOpen", 0.5f);
    }
    private void CardToggleOnOff()
    {
        if (!CardManager.GetInstance.isCardDealingDone) return;
        if (CardManager.GetInstance.isAligning) return;

        if (Input.GetKeyDown(GameManager.GetInstance.cardToggleKey))
        {
            if (isCardVisible)
            {
                CardManager.GetInstance.CardsDown();
                isCardVisible = false;
                cardBtnImg.transform.localScale = new Vector3(1, -1, 1);

            }
            else
            {
                CardManager.GetInstance.CardsUp();
                isCardVisible = true;
                cardBtnImg.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

#if UNITY_ANDROID
    public void CardToggleButton()
    {
        if (isCardVisible)
        {
            CardManager.GetInstance.CardsDown();
            isCardVisible = false;
            cardBtnImg.transform.localScale = new Vector3(1, -1, 1);

        }
        else
        {
            CardManager.GetInstance.CardsUp();
            isCardVisible = true;
            cardBtnImg.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void InteractionButton()
    {
        playerMovement = GameManager.GetInstance.localPlayerMovement;
        playerMovement.PlayInteraction();
    }

    public void InteractionBtnOn()
    {
        interactionImg.gameObject.SetActive(true);
    }

    public void InteractionBtnOff()
    {
        interactionImg.gameObject.SetActive(false);
    }

    public void BtnPressedSound()
    {
        SoundManager.GetInstance.Play("BtnPress");
    }

    void CameraViewBtnOnOff()
    {
        if (CardManager.GetInstance.isAligning)
        {
            cameraViewImg.gameObject.SetActive(false);
        }
        else
        {
            cameraViewImg.gameObject.SetActive(true);
        }
    }
#endif

}