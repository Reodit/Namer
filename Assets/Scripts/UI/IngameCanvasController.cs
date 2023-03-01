using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameCanvasController : MonoBehaviour
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
        CameraViewBtnOnOff();
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
        
        if (GameManager.GetInstance.IsCustomLevel)
        {
            topButtons = GameObject.Find("IngameCanvas").transform.GetChild(12).gameObject;
        }
        else
        {
            topButtons = GameObject.Find("IngameCanvas").transform.GetChild(1).gameObject;
        }
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
        bottomButtons.SetActive(true);
        joyStick.SetActive(true);
        topButtons.SetActive(true);
        bottomButtons.SetActive(true);
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
        if (!GameManager.GetInstance.IsCustomLevel)
        {
            GameManager.GetInstance.ChangeGameState(GameStates.LevelSelect);
        }
        else
        {
            GameManager.GetInstance.ChangeGameState(GameStates.LevelEditMode);
        }
        
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
        int level = 0;
        string defaultName = "";
        if (!GameManager.GetInstance.IsCustomLevel)
        {
            level = GameManager.GetInstance.Level;
            defaultName = " Stage";
        }
        else
        {
            level = GameManager.GetInstance.CustomLevel;
            defaultName = " Star";
        }

        if (GameManager.GetInstance.CurrentState == GameStates.LevelEditMode || GameManager.GetInstance.CurrentState == GameStates.LevelEditorTestPlay)
        {
            stageName.text = (level + 1) + defaultName;
            return;
        }
        
        string currentName = GameDataManager.GetInstance.GetLevelName(level);
        if (currentName == "")
        {
            stageName.text = $"{(level)}" + defaultName;
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
}