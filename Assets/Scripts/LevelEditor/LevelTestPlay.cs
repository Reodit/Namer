using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTestPlay : MonoBehaviour
{
    [SerializeField]private Button levelDesignButton;
    [SerializeField]private Button cancelButton;
    private int currentLevel;

    private SLevelData customLevelData;
    
    private void Awake()
    {
        currentLevel = GameManager.GetInstance.CustomLevel;
        GameManager.GetInstance.ChangeGameState(GameStates.LevelEditorTestPlay);
    }

    private void Start()
    {
        GameDataManager.GetInstance.CreateCustomLevelMap();
        DetectManager.GetInstance.Init();
        
        UIManager.GetInstance.ingameCanvas = GameObject.Find("IngameCanvas");
        UIManager.GetInstance.pauseUIPanel = UIManager.GetInstance.ingameCanvas.transform.Find("PauseUI Panel").gameObject;
        
        CardManager.GetInstance.startCards = GameDataManager.GetInstance.GetCardPrefabs(GameDataManager.GetInstance
            .CustomLevelDataDic[currentLevel + 1].cardView);

        SetButton();
    }

    void SetButton()
    {
        levelDesignButton = GameObject.Find("LevelDesignSaveButton").GetComponent<Button>();
        levelDesignButton.onClick.AddListener(SaveCustomLevelMap);
        
        cancelButton = GameObject.Find("LevelDesignCancelButton").GetComponent<Button>();
        cancelButton.onClick.AddListener(CancelCustomLevelMap);
    }

    private void SaveCustomLevelMap()
    {
        if (currentLevel < GameManager.GetInstance.CustomLevel)
        {
            return;
        }
        
        GameManager.GetInstance.PlusCustomLevel();
        GameDataManager.GetInstance.CreateFile();
        
        SceneManager.LoadScene("MainScene");
    }

    private void CancelCustomLevelMap()
    {
        GameDataManager.GetInstance.DeleteCustomLevelData(currentLevel + 1);
        
        // TODO 레벨 에디터로 이동
    }
}
