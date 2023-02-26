using UnityEngine;
using UnityEngine.UI;

public class LevelTestPlay : MonoBehaviour
{
    [SerializeField]private Button levelDesignButton;

    private SLevelData customLevelData;
    
    private void Awake()
    {
        GameManager.GetInstance.ChangeGameState(GameStates.LevelEditorTestPlay);
    }

    private void Start()
    {
        GameDataManager.GetInstance.CreateLevelTestMap();
        DetectManager.GetInstance.Init();
        
        UIManager.GetInstance.ingameCanvas = GameObject.Find("IngameCanvas");
        UIManager.GetInstance.pauseUIPanel = UIManager.GetInstance.ingameCanvas.transform.Find("PauseUI Panel").gameObject;

        CardManager.GetInstance.startCards = GameDataManager.GetInstance.GetCardPrefabs(GameDataManager.GetInstance
            .CustomLevelDataDic[GameManager.GetInstance.CustomLevel + 1].cardView);

        SetButton();
    }

    void SetButton()
    {
        levelDesignButton = GameObject.Find("LevelDesignSaveButton").GetComponent<Button>();
        levelDesignButton.onClick.AddListener(GameManager.GetInstance.PlusCustomLevel);
        levelDesignButton.onClick.AddListener(GameDataManager.GetInstance.CreateFile);
    }
}
