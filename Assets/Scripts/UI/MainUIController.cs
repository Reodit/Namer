using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

enum MainMenuState
{
    Title = 0,
    Main = 1,
    Level = 2,
    Credit = 3,
}

public class MainUIController : MonoBehaviour
{
    [SerializeField] GameObject pressAnyKeyTxt;
    [SerializeField] GameObject title;
    [SerializeField] GameObject cardHolder;
    [SerializeField] GameObject informationTxt;
    [SerializeField] GameObject levelSelectCardHolder;
    [SerializeField] GameObject returnBtn;
    [SerializeField] GameObject pauseBtn;
    [SerializeField] GameObject goBtn;
    [SerializeField] GameObject creditObject;
    [SerializeField] GameObject[] mainMenuGrounds;
    GameObject levelInformationTxt;

    [SerializeField] float titleMovingTime = 1f;
    [SerializeField] float cameraMovingTime = 1f;
    [SerializeField] float levelSelectMovingTime = 1.5f;
    [SerializeField] float menuTileMovingTime = 1f;
    bool isPressAnyKey;
    float currentTime;
    [SerializeField] float speed = 2f;
    [SerializeField] float length = 15f;

    MainMenuState state;


    private void Start()
    {
        StartCoroutine(PressAnyKeyFloat());
        state = MainMenuState.Title;
    }

    void Update()
    {
        PressAnyKey();
        informationTxtOnOff();

    }

    //안내 문구를 상황에 따라 키고 끕니다 
    void informationTxtOnOff()
    {
        if (CardManager.GetInstance.isPickCard && informationTxt.activeSelf == false)
        {
            informationTxt.SetActive(true);

        }
        else if (!CardManager.GetInstance.isPickCard && informationTxt.activeSelf == true)
        {
            informationTxt.SetActive(false);
        }
    }

    //타이틀 화면에서 아무키나 입력하면 메인메뉴 화면으로 이동 
    void PressAnyKey()
    {
        if (!isPressAnyKey && Input.anyKeyDown)
        {
            isPressAnyKey = true;
            TitleMove(titleMovingTime);
            pressAnyKeyTxt.SetActive(false);
            CameraMoving(cameraMovingTime);
            StartCoroutine(MainMenuGroudsSetUp());
            Invoke("CardholderStart", 0.1f);

        }
    }

    // 게임 타이틀을 메인메뉴에 알맞게 이동
    void TitleMove(float titleMovingTime)
    {
        title.transform.DOMove(new Vector3(Screen.width/2f, Screen.height/1.161f, 0f), titleMovingTime);
        title.transform.DOScale(new Vector3(0.5f, 0.5f, 1f), titleMovingTime);
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
        while (true)
        {
            currentTime += Time.deltaTime * speed;
            pressAnyKeyTxt.transform.
                localPosition = new Vector3(pressAnyKeyTxt.transform.localPosition.x,
                currentPos.y + Mathf.Sin(currentTime) * length,
                pressAnyKeyTxt.transform.localPosition.z);
            yield return null;
        }
    }

    //메인메뉴 타일들을 밑에서부터 랜덤하게 생성해서 제자리로 이동시/
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

    //레벨 셀렉트 화면으로 넘어감 
    public void LevelSelectScene()
    {
        state = MainMenuState.Level;
        Camera.main.transform.DOMove(new Vector3(10f, 7f, -3.17f), levelSelectMovingTime);
        Camera.main.transform.DORotate(new Vector3(60f, 90f, 0f), levelSelectMovingTime);
        title.transform.DOMove(new Vector3(Screen.width / 12f, Screen.height / 1.08f, 0f), levelSelectMovingTime);
        title.transform.DOScale(new Vector3(0.2f, 0.2f, 1f), levelSelectMovingTime);
        levelSelectCardHolder.SetActive(true);

    }

    //메인 메뉴 화면으로 넘어감
    public void MainMenuScene()
    {
        if (state == MainMenuState.Level)
        {
            levelSelectCardHolder.SetActive(false);
        }
        if(state == MainMenuState.Credit)
        {
            creditObject.transform.position =new Vector3(0, -10, 0);
            creditObject.SetActive(false);
            returnBtn.SetActive(false);
            goBtn.SetActive(false);
            pauseBtn.SetActive(false);
        }
        state = MainMenuState.Main;
        Camera.main.transform.DOMove(new Vector3(0f, 7f, -3.17f), levelSelectMovingTime);
        Camera.main.transform.DORotate(new Vector3(60f, 0f, 0f), levelSelectMovingTime);
        title.transform.DOScale(new Vector3(0.5f, 0.5f, 1f), levelSelectMovingTime);
        title.transform.DOMove(new Vector3(Screen.width / 2f, Screen.height / 1.161f, 0f), levelSelectMovingTime);
    }

    public void CreditScene()
    {
        state = MainMenuState.Credit;
        Camera.main.transform.DORotate(new Vector3(-30f, 0f, 0f), 3f);
        title.transform.DOMove(new Vector3(Screen.width / 2f, -200, 0f), 2f);
        title.transform.DOScale(new Vector3(0.3f, 0.3f, 1f), 2f);
        Invoke("CreditObjOn", 3f);
        returnBtn.SetActive(true);
        pauseBtn.SetActive(true);
    }

    void CreditObjOn()
    {
        creditObject.SetActive(true);
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


}
