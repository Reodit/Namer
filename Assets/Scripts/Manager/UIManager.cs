using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject ingameCanvas;  // 인스펙터에서 프리펩 직접 할당
    public GameObject pauseUIPanel;

    public bool isShowNameKeyPressed;
    public bool isPauseKeyPressed;
    
    void Start()
    {
        isShowNameKeyPressed = false;
        GameManager.GetInstance.KeyAction += UIInputKeyCheck;
    }
    
    void Update()
    {
        UIOnOff();
    }

    private void UIInputKeyCheck()
    {
        #region ShowNameKeyCheck
        if (Input.GetKeyDown(GameManager.GetInstance.showNameKey))
        {
            isShowNameKeyPressed = true;
        }

        if (Input.GetKeyUp(GameManager.GetInstance.showNameKey))
        {
            isShowNameKeyPressed = false;
        }
        #endregion

        #region PauseKeyCheck
        if (Input.GetKeyDown(GameManager.GetInstance.pauseKey))
        {
            isPauseKeyPressed = true;
        }

        else
        {
            isPauseKeyPressed = false;
        }
        #endregion
    }
    
    void UIOnOff()
    {
        if (isPauseKeyPressed && 
            GameManager.GetInstance.currentState != GameStates.Pause && 
            GameManager.GetInstance.currentState != GameStates.Lobby)
        {
            pauseUIPanel.SetActive(true);
            GameManager.GetInstance.ChangeGameState(GameStates.Pause);
            GameManager.GetInstance.SetTimeScale(0);
        }
        
        else if (isPauseKeyPressed && 
                 GameManager.GetInstance.currentState == GameStates.Pause &&
                 GameManager.GetInstance.currentState != GameStates.Lobby)
        {
            pauseUIPanel.SetActive(false);
            GameManager.GetInstance.ReturnPreviousState();
            GameManager.GetInstance.SetTimeScale(1);
        }
    }
}
