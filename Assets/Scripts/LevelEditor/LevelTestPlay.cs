using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTestPlay : MonoBehaviour
{
    [SerializeField]private Button saveButton;
    [SerializeField]private Button closeButton;
    [SerializeField]private Button cancelButton;
    
    private int currentLevel;

    private SLevelData customLevelData;
    
    private void Awake()
    {
        currentLevel = GameManager.GetInstance.CustomLevel;
        GameManager.GetInstance.ChangeGameState(GameStates.LevelEditorTestPlay);
        
        GameDataManager.GetInstance.CreateCustomLevelMap();
    }

    private void Start()
    {
        DetectManager.GetInstance.Init();
        ScenarioController scenarioController = FindObjectOfType<ScenarioController>();
        scenarioController.Init();
        
        UIManager.GetInstance.ingameCanvas = GameObject.Find("IngameCanvas");
        UIManager.GetInstance.pauseUIPanel = UIManager.GetInstance.ingameCanvas.transform.Find("PauseUI Panel").gameObject;
        
        CardManager.GetInstance.startCards = GameDataManager.GetInstance.GetCardPrefabs(GameDataManager.GetInstance
            .CustomLevelDataDic[currentLevel + 1].cardView);

        SetButton();
    }

    void SetButton()
    {
        saveButton.onClick.AddListener(SaveCustomLevelMap);
        closeButton.onClick.AddListener(CancelCustomLevelMap);
        
        cancelButton.onClick.AddListener(CancelCustomLevelMap);
    }

    private void SaveCustomLevelMap()
    {
        if (currentLevel < GameManager.GetInstance.CustomLevel)
        {
            return;
        }
        
        GameManager.GetInstance.SetCustomLevel(GameManager.GetInstance.CustomLevel + 1);
        GameDataManager.GetInstance.UpdateCustomLevel();
        GameDataManager.GetInstance.CreateFile();
        
        GameManager.GetInstance.ChangeGameState(GameStates.LevelEditMode);
        SceneManager.LoadScene("MainScene");
    }

    private void CancelCustomLevelMap()
    {
        GameDataManager.GetInstance.DeleteCustomLevelData(currentLevel + 1);
        SceneBehaviorManager.LoadScene(Scenes.LevelEditor, LoadSceneMode.Single);
    }
}
