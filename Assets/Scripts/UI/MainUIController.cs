using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum MainMenuState
{
    Title = 0,
    Main = 1,
    Level = 2,
    Encyclopedia,
    Credit,
    Edit,
}

public class MainUIController : MonoBehaviour
{
    [SerializeField] GameObject pressAnyKeyTxt;
    [SerializeField] GameObject title;
    [SerializeField] GameObject cardHolder;
    [SerializeField] GameObject informationTxt;
    [SerializeField] GameObject levelSelectCardHolder;
    [SerializeField] GameObject levelEditCardHolder;
    [SerializeField] GameObject settingBtn;
    [SerializeField] GameObject returnBtn;
    [SerializeField] GameObject pauseBtn;
    [SerializeField] GameObject goBtn;
    [SerializeField] GameObject creditObject;
    [SerializeField] GameObject[] mainMenuGrounds;
    [SerializeField] GameObject encyclopedia;
    [SerializeField] GameObject mainMenucards;
    [SerializeField] GameObject titlePanel;
    [SerializeField] GameObject optionPanel;
    [SerializeField] GameObject levelSelectBtnPanel;
    [SerializeField] GameObject levelSelectBtnPanelLeftBtn;
    [SerializeField] GameObject levelSelectBtnPanelRightBtn;
    [SerializeField] GameObject levelSelectCards;
    [SerializeField] GameObject levelEditBtnPanel;
    [SerializeField] GameObject levelEditBtnPanelLeftBtn;
    [SerializeField] GameObject levelEditBtnPanelRightBtn;
    [SerializeField] GameObject levelEditCards;
    [SerializeField] GameObject infoPopUp;
    public GameObject mainRose;
    public GameObject levelRose;
    public GameObject editRose;
    [SerializeField] Text infoPopUpTxt;
    GameObject levelInformationTxt;

    [SerializeField] float titleMovingTime = 1f;
    [SerializeField] float cameraMovingTime = 1f;
    [SerializeField] float levelSelectMovingTime = 1.5f;
    [SerializeField] float menuTileMovingTime = 1f;
    bool isPressAnyKey;
    public bool isSelectStart, isEditStart;
    float currentTime;
    float speed = 2f;
    float length = 15f;

    GameDataManager gameDataManager;

    public MainMenuState state;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        StartCoroutine(PressAnyKeyFloat());
        state = MainMenuState.Title;
        SoundManager.GetInstance.ChangeMainBGM(state);
        if (GameManager.GetInstance.CurrentState == GameStates.LevelSelect)
        {
            DirectLevelSelect();
        }

        if (GameManager.GetInstance.CurrentState == GameStates.LevelEditorTestPlay)
        {
            DirectEditSelect();
        }

        UIManager.GetInstance.isShowNameKeyPressed = false;
        CardManager.GetInstance.pickCard = null;
        CardManager.GetInstance.isPickCard = false;
        CardManager.GetInstance.target = null;
    }

    private void DirectEditSelect()
    {
        isPressAnyKey = true;
        TitleMove(0f);
        pressAnyKeyTxt.SetActive(false);
        StartCoroutine(MainMenuGroudsSetUp());
        Invoke("CardholderStart", 0.1f);

        Camera.main.transform.position = new Vector3(-10, 7, -3.17f);
        Camera.main.transform.rotation = Quaternion.Euler(60, -90, 0);

        state = MainMenuState.Edit;
        title.transform.DOMove(new Vector3(Screen.width / 12f, Screen.height / 1.08f, 0f), levelSelectMovingTime);
        title.transform.DOScale(new Vector3(0.2f, 0.2f, 1f), levelSelectMovingTime);
        levelEditCardHolder.SetActive(true);
        LevelEditBtnController();
        SettingBtnOn();
        CardManager.GetInstance.isMenuLevel = true;
    }

    private void DirectLevelSelect()
    {
        isPressAnyKey = true;
        TitleMove(0f);
        pressAnyKeyTxt.SetActive(false);
        StartCoroutine(MainMenuGroudsSetUp());
        Invoke("CardholderStart", 0.1f);

        Camera.main.transform.position = new Vector3(10, 7, -3.17f);
        Camera.main.transform.rotation = Quaternion.Euler(60, 90, 0);

        state = MainMenuState.Level;
        title.transform.DOMove(new Vector3(Screen.width / 12f, Screen.height / 1.08f, 0f), levelSelectMovingTime);
        title.transform.DOScale(new Vector3(0.2f, 0.2f, 1f), levelSelectMovingTime);
        levelSelectCardHolder.SetActive(true);
        SettingBtnOn();
        CardManager.GetInstance.isMenuLevel = true;
    }

    void Update()
    {
        if (GameManager.GetInstance.CurrentState != GameStates.LevelSelect)
        {
            PressAnyKey();
        }
        informationTxtOnOff();

    }

    //안내 문구를 상황에 따라 키고 끕니다 
    void informationTxtOnOff()
    {
        if (state == MainMenuState.Encyclopedia) return;

        if (CardManager.GetInstance.isPickCard && informationTxt.activeSelf == false)
        {
            informationTxt.SetActive(true);

        }
        else if (!CardManager.GetInstance.isPickCard && informationTxt.activeSelf == true)
        {
            informationTxt.SetActive(false);
        }
    }

    //타이틀 화면을 터치3면 메인메뉴 화면으로 이동 
    void PressAnyKey()
    {
        if (!isPressAnyKey && Input.touchCount > 0)
        {
            isPressAnyKey = true;
            TitleMove(titleMovingTime);
            Destroy(pressAnyKeyTxt);
            CameraMoving(cameraMovingTime);
            StartCoroutine(MainMenuGroudsSetUp());
            state = MainMenuState.Main;
            Invoke("CardholderStart", 0.1f);
            Invoke("SettingBtnOn", titleMovingTime);
        }
    }

    void SettingBtnOn()
    {
        settingBtn.SetActive(true);
    }

    // 게임 타이틀을 메인메뉴에 알맞게 이동
    void TitleMove(float titleMovingTime)
    {
        title.transform.DOMove(new Vector3(Screen.width/2f, Screen.height/1.161f, 0f), titleMovingTime);
        title.transform.DOScale(new Vector3(0.35f, 0.35f, 1f), titleMovingTime);
    }

    void CameraMoving(float cameraMovingTime)
    {
        Camera.main.transform.DORotate(new Vector3(60f, 0f, 0f), cameraMovingTime);
    }

    void CardholderStart()
    {
        cardHolder.SetActive(true);
    }

    // 아무 키나 누르세요에 둥둥 효과를 줌
    IEnumerator PressAnyKeyFloat()
    {
        Vector3 currentPos = pressAnyKeyTxt.transform.localPosition;
        while (!isPressAnyKey)
        {
            currentTime += Time.deltaTime * speed;
            pressAnyKeyTxt.transform.
                localPosition = new Vector3(pressAnyKeyTxt.transform.localPosition.x,
                currentPos.y + Mathf.Sin(currentTime) * length,
                pressAnyKeyTxt.transform.localPosition.z);
            yield return null;
        }
    }

    //메인메뉴 타일들을 밑에서 생성해서 랜덤한 속도로 제자리에 배치됨
    float ranNum;
    IEnumerator MainMenuGroudsSetUp()
    {
        for (int i = 0; i < mainMenuGrounds.Length; i++)
        {
             ranNum = Random.Range(0f, 2f);
             mainMenuGrounds[i].SetActive(true);
             mainMenuGrounds[i].transform.DOMove(mainMenuGrounds[i].transform.position + new Vector3(0f, 15f, 0), menuTileMovingTime + ranNum);
             yield return null;
        }

    }

    //레벨 에디트 화면으로 넘어감
    public void LevelEditScene()
    {
        if (isEditStart)
        {
            LevelEditPanelOn();
        }
        state = MainMenuState.Edit;
        Camera.main.transform.DOMove(new Vector3(-10f, 7f, -3.17f), levelSelectMovingTime);
        Camera.main.transform.DORotate(new Vector3(60f, -90f, 0f), levelSelectMovingTime);
        title.transform.DOMove(new Vector3(Screen.width / 12f, Screen.height / 1.08f, 0f), levelSelectMovingTime);
        title.transform.DOScale(new Vector3(0.2f, 0.2f, 1f), levelSelectMovingTime);
        levelEditCardHolder.SetActive(true);
        LevelEditBtnController();
    }

    //레벨 셀렉트 화면으로 넘어감 
    public void LevelSelectScene()
    {
        if (isSelectStart)
        {
            LevelSelectPanelOn();
        }
        state = MainMenuState.Level;
        Camera.main.transform.DOMove(new Vector3(10f, 7f, -3.17f), levelSelectMovingTime);
        Camera.main.transform.DORotate(new Vector3(60f, 90f, 0f), levelSelectMovingTime);
        title.transform.DOMove(new Vector3(Screen.width / 12f, Screen.height / 1.08f, 0f), levelSelectMovingTime);
        title.transform.DOScale(new Vector3(0.2f, 0.2f, 1f), levelSelectMovingTime);
        levelSelectCardHolder.SetActive(true);
        SoundManager.GetInstance.ChangeMainBGM(state);
    }

    //메인 메뉴 화면으로 넘어감
    public void MainMenuScene()
    {
        if (state == MainMenuState.Level)
        {
            levelSelectCardHolder.SetActive(false);
            levelSelectBtnPanel.SetActive(false);
        }
        else if (state == MainMenuState.Edit)
        {
            levelEditCardHolder.SetActive(false);
            levelEditBtnPanel.SetActive(false);
        }
        else if (state == MainMenuState.Credit)
        {
            creditObject.transform.position = new Vector3(0, -10, 0);
            creditObject.SetActive(false);
            returnBtn.SetActive(false);
            goBtn.SetActive(false);
            pauseBtn.SetActive(false);
        }
        state = MainMenuState.Main;
        Camera.main.transform.DOMove(new Vector3(0f, 7f, -3.17f), levelSelectMovingTime);
        Camera.main.transform.DORotate(new Vector3(60f, 0f, 0f), levelSelectMovingTime);
        title.transform.DOScale(new Vector3(0.35f, 0.35f, 1f), levelSelectMovingTime);
        title.transform.DOMove(new Vector3(Screen.width / 2f, Screen.height / 1.161f, 0f), levelSelectMovingTime);
        SoundManager.GetInstance.ChangeMainBGM(state);
    }

    //도감 화면으로 넘어감 
    public void EncyclopediaScene()
    {
        state = MainMenuState.Encyclopedia;
        informationTxt.SetActive(false);
        encyclopedia.SetActive(true);
        titlePanel.SetActive(false);
        mainMenucards.SetActive(false);
        mainRose.SetActive(false);
        settingBtn.SetActive(false);
        mainRose.transform.GetChild(0).gameObject.SetActive(false);
        CardManager.GetInstance.isEncyclopedia = true;
        SoundManager.GetInstance.ChangeMainBGM(state);
    }

    public void CreditScene()
    {
        state = MainMenuState.Credit;
        Camera.main.transform.DORotate(new Vector3(-30f, 0f, 0f), 3f);
        title.transform.DOMove(new Vector3(Screen.width / 2f, -200, 0f), 2f);
        title.transform.DOScale(new Vector3(0.3f, 0.3f, 1f), 2f);
        Invoke("CreditObjOn", 3f);
        SoundManager.GetInstance.ChangeMainBGM(state);
    }

    void CreditObjOn()
    {
        creditObject.SetActive(true);
        returnBtn.SetActive(true);
        pauseBtn.SetActive(true);
    }


    public void EncyclopediaReturnBtn()
    {
        state = MainMenuState.Main;
        encyclopedia.SetActive(false);
        titlePanel.SetActive(true);
        mainMenucards.SetActive(true);
        mainRose.SetActive(true);
        settingBtn.SetActive(true);
        MainMenuCardController card =
        GameObject.Find("MainMenuCards").transform.Find("EncyclopediaCard(Clone)").
        GetComponent<MainMenuCardController>();
        card.CardReturn();
        CardManager.GetInstance.isEncyclopedia = false;
        SoundManager.GetInstance.ChangeMainBGM(state);
    }

    public void ReturnBtn()
    {
        if (state == MainMenuState.Credit)
        {
            GameManager.GetInstance.SetTimeScale(1);
            MainMenuScene();
        }
    }

    public void PauseBtn()
    {
        GameManager.GetInstance.SetTimeScale(0);
        pauseBtn.SetActive(false);
        goBtn.SetActive(true);
    }

    public void GoBtn()
    {
        GameManager.GetInstance.SetTimeScale(1);
        goBtn.SetActive(false);
        pauseBtn.SetActive(true);
    }

    public void OptionPanelOpen()
    {
        optionPanel.SetActive(true);
        CardManager.GetInstance.ableCardCtr = false;
    }

    public void OptionPanelClose()
    {
        CardManager.GetInstance.ableCardCtr = true;
        optionPanel.SetActive(false);
    }

    public void GameOff()
    {
        Application.Quit();
    }

    public void GameUpdateInfo()
    {
        CardManager.GetInstance.ableCardCtr = false;
        infoPopUpTxt.text = "다음 스테이지는\n곧 업데이트 됩니다.";
        infoPopUp.SetActive(true);
        infoPopUp.transform.Find("Buttons").gameObject.SetActive(false);
    }

    public void GameResetConfirm()
    {
        CardManager.GetInstance.ableCardCtr = false;
        infoPopUpTxt.text = "게임 데이터를\n정말 삭제하시겠습니까?\n";
        infoPopUp.SetActive(true);
        infoPopUp.transform.Find("Buttons").gameObject.SetActive(true);
    }

    public void InfoStagePopUp()
    {
        CardManager.GetInstance.ableCardCtr = false;
        infoPopUpTxt.text = "이전 스테이지를\n클리어해주세요";
        infoPopUp.SetActive(true);
        infoPopUp.transform.Find("Buttons").gameObject.SetActive(false);
    }

    public void InfoPopUpOff()
    {
        CardManager.GetInstance.ableCardCtr = true;
        infoPopUp.SetActive(false);
    }

    public void GameReset()
    {
        GameDataManager.GetInstance.ResetUserData(GameManager.GetInstance.userId);
        infoPopUp.transform.Find("Buttons").gameObject.SetActive(false);
        infoPopUpTxt.text = "게임 데이터가 \n초기화됐습니다.";
        SceneManager.LoadScene("MainScene");
    }

    #region Level&EditButtonPanel
    public void LevelEditPanelOn()
    {
        levelEditBtnPanel.SetActive(true);
    }

    public void LevelSelectPanelOn()
    {
        levelSelectBtnPanel.SetActive(true);
    }

    int selectPageCount = 0;
    int selectMaxPage = 0;
    public void LevelSelectPanelRightBtn()
    {
        levelSelectCards.transform.GetChild(selectPageCount).gameObject.SetActive(false);
        selectPageCount++;
        levelSelectCards.transform.GetChild(selectPageCount).gameObject.SetActive(true);
        LevelSelectBtnController();
    }

    public void LevelSelectPanelLeftBtn()
    {
        levelSelectCards.transform.GetChild(selectPageCount).gameObject.SetActive(false);
        selectPageCount--;
        levelSelectCards.transform.GetChild(selectPageCount).gameObject.SetActive(true);
        LevelSelectBtnController();
    }

    void LevelSelectBtnController()
    {
        selectMaxPage = levelSelectCards.transform.childCount;

        if (selectPageCount == 0 && selectMaxPage == 1)
        {
            levelEditBtnPanelLeftBtn.SetActive(false);
            levelEditBtnPanelRightBtn.SetActive(false);
        }
        if (selectPageCount <= 0)
        {
            levelSelectBtnPanelLeftBtn.SetActive(false);
            levelSelectBtnPanelRightBtn.SetActive(true);
        }
        else if(selectPageCount >= selectMaxPage - 1)
        {
            levelSelectBtnPanelRightBtn.SetActive(false);
            levelSelectBtnPanelLeftBtn.SetActive(true);
        }
        else
        {
            levelSelectBtnPanelLeftBtn.SetActive(true);
            levelSelectBtnPanelRightBtn.SetActive(true);
        }
    }

    int editPageCount = 0;
    int editMaxPage = 0;
    public void LevelEditPanelRightBtn()
    {
        levelEditCards.transform.GetChild(editPageCount).gameObject.SetActive(false);
        editPageCount++;
        levelEditCards.transform.GetChild(editPageCount).gameObject.SetActive(true);
        LevelEditBtnController();
    }

    public void LevelEditPanelLeftBtn()
    {
        levelEditCards.transform.GetChild(editPageCount).gameObject.SetActive(false);
        editPageCount--;
        levelEditCards.transform.GetChild(editPageCount).gameObject.SetActive(true);
        LevelEditBtnController();
    }

    void LevelEditBtnController()
    {
        editMaxPage = levelEditCards.transform.childCount;

        if (editPageCount == 0 && editMaxPage == 1)
        {
            levelEditBtnPanelLeftBtn.SetActive(false);
            levelEditBtnPanelRightBtn.SetActive(false);
        }
        else if (editPageCount == 0)
        {
            levelEditBtnPanelLeftBtn.SetActive(false);
            levelEditBtnPanelRightBtn.SetActive(true);
        }
        else if (editPageCount == editMaxPage - 1)
        {
            levelEditBtnPanelRightBtn.SetActive(false);
            levelEditBtnPanelLeftBtn.SetActive(true);
        }
        else
        {
            levelEditBtnPanelLeftBtn.SetActive(true);
            levelEditBtnPanelRightBtn.SetActive(true);
        }
    }

    #endregion
}
