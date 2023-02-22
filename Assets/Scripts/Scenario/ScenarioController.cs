using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor.VersionControl;
using UnityEditor.Experimental.GraphView;

public enum ERequireType
{
    Null = -1,
    PlayerPos,
    AddCard,
    MouseClick,
    Victory,
    InputKey,
    time,
    PressBtn
}

[System.Serializable]
public struct Scenario
{
    public ERequireType type;
    public SPosition destinationPos;
    public SPosition targetPos;
    public SPosition prePos;
    public SPosition nextPos;
    public string requiredName;
    public string key;
    public float time;
    public bool isDialog;
    public string message;
    public string funcName;
    public bool isFocus;
    public bool canSkip;
    public bool showArrow;
}

[System.Serializable]
public struct Word
{
    public string id;
    public string context;
}

public class ScenarioController : MonoBehaviour
{
    [Header("시나리오 추가할 때에 넣는 부분")]
    [SerializeField] Scenario[] scenarioList;
    private static Queue<Scenario> scenarios = new Queue<Scenario>();
    private Scenario curScenario;
    private int scenarioCount = 0;
    private Transform player;
    private CameraController cameraController;
    private float restartTime = 20f;
    private float nextSenarioTime = -3f;
    private bool isStart = false;

    [Header("필수로 등록해야 하는 부분")]
    [SerializeField] GameObject logBox;
    [SerializeField] Text logText;
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] GameObject stageClearPanel;
    [SerializeField] GameObject targetPoint;
    [SerializeField] Image arrow;
    [SerializeField] GameObject skipBtn;
    [SerializeField] Camera uiCam;

    [System.NonSerialized] public bool logOpened = false;
    [System.NonSerialized] public bool dialogOpened = false;
    [System.NonSerialized] public bool isUI = false;

    [Header("조정값")]
    [SerializeField] private float delayWinUI = 2f;
    [SerializeField] private float delayDealingTime = 3f;

    [Header("PC, Android 명명")]
    [SerializeField] Word[] PCwords;
    Dictionary<string, string> PCword = new Dictionary<string, string>();
    [SerializeField] Word[] Andwords;
    Dictionary<string, string> Andword = new Dictionary<string, string>();

    private void Awake()
    {
        GameManager.GetInstance.scenarioController = this;

        foreach (Word Pword in PCwords)
        {
            if (PCword.Keys.Contains(Pword.id)) return;
            PCword[Pword.id] = Pword.context;
        }

        foreach (Word Aword in Andwords)
        {
            if (Andword.Keys.Contains(Aword.id)) return;
            Andword[Aword.id] = Aword.context;
        }
    }

    public void Init()
    {
        curScenario = new Scenario();
        curScenario.type = ERequireType.Null;
        logBox.SetActive(false);
        logOpened = false;
        dialogBox.SetActive(false);
        dialogOpened = false;

        skipBtn = dialogBox.transform.Find("SkipBtn").gameObject;
        stageClearPanel =
            GameObject.Find("IngameCanvas").transform.
            Find("StageClearPanel").gameObject;

        isUI = false;
        isStart = false;
        GameManager.GetInstance.isPlayerCanInput = false;

        scenarioCount = 0;
        scenarios = new Queue<Scenario>();
        restartTime = 20f;

        cameraController = Camera.main.transform.parent.GetComponent<CameraController>();
        foreach (Scenario scenario in GameDataManager.GetInstance.LevelDataDic[GameManager.GetInstance.Level].scenario)
        {
            scenarios.Enqueue(scenario);
            scenarioCount++;
        }

        player = GameObject.Find("Player").transform;

        StartCoroutine(WaitDealing());
    }

    private IEnumerator WaitDealing()
    {
        yield return new WaitForSeconds(delayDealingTime);
        isStart = true;
        GameManager.GetInstance.isPlayerCanInput = true;
        NextScenario();
        StartScenario();
    }

#region LogMessage
    public void LogOnOff(bool isOn)
    {
        switch (isOn)
        {
            case (true):
                logBox.SetActive(logOpened);
                dialogBox.SetActive(dialogOpened);
                break;
            case (false):
                logOpened = logBox.activeSelf;
                dialogOpened = dialogBox.activeSelf;
                logBox.SetActive(false);
                dialogBox.SetActive(false);
                break;
        }
    }

    private void SystemLog(string message, bool isClick = false)
    {
        if (message.Contains("#"))
        {
            message = ChangeIDinPlatform(message);
        }
        logBox.SetActive(true);
        logText.text = message;
        logBox.GetComponent<LogText>().SetTime();
        skipBtn.GetComponentInChildren<Text>().text = isClick ? "Skip" : "Close";
        restartTime = 20f;
    }

    private void LogError(string message)
    {
        string errorText = $"<color=red>{message}</color>";
        SystemLog(errorText);
    }

    private void Log(string message, string objName, bool isClick = false)
    {
        if (message.Contains("#"))
        {
            message = ChangeIDinPlatform(message);
        }
        string dialogMessage = $"<color=red>[{objName}]</color>\n{message}";
        dialogBox.SetActive(true);
        skipBtn.GetComponentInChildren<Text>().text = isClick ? "Skip" : "Close";
        dialogText.text = dialogMessage;
        dialogBox.GetComponent<LogText>().SetTime();
    }

    private string ChangeIDinPlatform(string msg)
    {
        int idx = msg.IndexOf("#");
        string id = msg.Substring(idx, 3);
#if UNITY_ANDROID
        if (!Andword.Keys.Contains(id))
        {
            msg = msg.Replace("#", "");
        }
        msg = msg.Replace(id, Andword[id]);
#else
        if (!PCword.Keys.Contains(id))
        {
            message.Replace("#", "");
        }
        message.Replace(id, PCword[id]);
#endif
        return msg;
    }

    public IEnumerator SkipLog()
    {
        int curNum = scenarioCount;
        while (curScenario.canSkip)
        {
            if (curNum != scenarioCount)
                StartScenario();
            else
                yield return new WaitForSeconds(0.02f);
        }
    }
#endregion

#region Scenario Functions
    [ContextMenu("SaveNewScenario")]
    public void SaveScenario()
    {
        //테스트 완료 후, JSON 파일 저장하는 함수
        if (scenarioList == null || scenarioList.Length == 0)
        {
            SystemLog("[에러]리스트에 추가할 시나리오를 1개 이상 추가하세요.");
            return;
        }
        SaveLoadFile saveFile = new SaveLoadFile();
        saveFile.CreateJsonFile(scenarioList.ToList(), "Assets/Resources/Data/SaveLoad", "Level0" + GameManager.GetInstance.Level + "Scenario.json");
    }

    private void StartScenario()
    {
        scenarioCount = scenarios.Count;
        DoScenario();
    }

    private void NextScenario()
    {
        if (scenarios.Count != 0)
        {
            curScenario = scenarios.Dequeue();
        }
        else
        {
            curScenario = new Scenario();
            curScenario.type = ERequireType.Null;
        }
        restartTime = 20f;
    }

    private void DoScenario()
    {
        nextSenarioTime = -3;
        if (cameraController == null) cameraController = Camera.main.transform.parent.GetComponent<CameraController>();
        if (curScenario.isDialog)
        {
            if (curScenario.message != null && curScenario.message != "")
            {
                if (curScenario.isFocus)
                {
                    Vector3 curScenarioPos = new Vector3(curScenario.targetPos.x, curScenario.targetPos.y, curScenario.targetPos.z);
                    Dictionary<Vector3, GameObject> objDict = DetectManager.GetInstance.GetArrayObjects(curScenarioPos);
                    Vector3 vec = Vector3Int.FloorToInt(curScenarioPos);
                    if (objDict[vec] == null || objDict.Count == 0)
                        cameraController.FocusOn(player, false);
                    else
                        cameraController.FocusOn(objDict[vec].transform, false);
                }
                else
                {
                    if (cameraController.isFocused)
                    {
                        cameraController.FocusOff();
                    }
                }

                if (curScenario.message.Contains("[System]"))
                {
                    SystemLog(curScenario.message.Replace("[System]", ""), scenarios.Peek().type == ERequireType.MouseClick);
                }
                else
                {
                    if (!curScenario.message.Contains(":"))
                    {
                        LogError("[에러]json의 message에 ':'으로 이름과 메세지를 구분하세요.");
                        return;
                    }
                    string[] curMessage = curScenario.message.Split(":");
                    Log(curMessage[1], curMessage[0], scenarios.Peek().type == ERequireType.MouseClick);
                }
            }
        }

        if (curScenario.funcName != null && curScenario.funcName != "")
        {
            try
            {
                Invoke(curScenario.funcName, 0);
            }
            catch (NullReferenceException e)
            {
                LogError("[에러]json에 실제로 존재하는 함수명을 입력하세요.");
                Debug.LogError(e);
                return;
            }
        }
        else
        {
            NextScenario();
        }
    }

    private bool CheckScenarioCount()
    {
        bool checkedValue = (curScenario.type != ERequireType.Null && scenarioCount != scenarios.Count);
        if (scenarioCount == 0)
        {
            // 승리 ui 실행 
            StartCoroutine(OpenClearPanel());
            scenarioCount = -1;
        }
        else if (GameManager.GetInstance.CurrentState == GameStates.Victory)
        {
            curScenario = new Scenario();
            curScenario.type = ERequireType.Null;
            scenarioCount = 0;
            // 승리 ui 실행 
            StartCoroutine(OpenClearPanel());
            scenarioCount = -1;
            return true;
        }
        return checkedValue;
    }

    private void CheckScenarioCondition()
    {
        switch (curScenario.type)
        {
            case (ERequireType.PlayerPos):
                Vector3 playerPos = new Vector3(Mathf.Round(player.position.x), Mathf.Round(player.position.y), Mathf.Round(player.position.z));
                Vector3 requireScenarioPos = new Vector3(curScenario.destinationPos.x, curScenario.destinationPos.y, curScenario.destinationPos.z);
                targetPoint.transform.position = requireScenarioPos + (Vector3.up * 0.5f);
                targetPoint.SetActive(true);
                if (curScenario.showArrow)
                    SetArrowPos(targetPoint.transform);
                if (playerPos == requireScenarioPos)
                {
                    targetPoint.SetActive(false);
                    StartScenario();
                }
                break;
            case (ERequireType.AddCard):
                InteractiveObject tarObj = GetIObj();
                if (tarObj == null) return;
                string objName = tarObj.GetCurrentName();
                if (objName == null) return;
                if (objName.Contains(curScenario.requiredName))
                {
                    StartScenario();
                }
                break;
            case (ERequireType.MouseClick):
                if (Input.GetMouseButtonDown(0) && !isUI)
                {
                    StartScenario();
                }
                break;
            case (ERequireType.Victory):
                if (GameManager.GetInstance.CurrentState == GameStates.Victory)
                {
                    StartScenario();
                }
                break;
            case (ERequireType.InputKey):
                KeyCode key = GetKeyCode(curScenario.key);
                if (key == KeyCode.None || (Input.GetKeyDown(key) && !isUI))
                {
                    StartScenario();
                }
                break;
            case (ERequireType.time):
                if (nextSenarioTime == -3)
                    nextSenarioTime = curScenario.time;
                if (nextSenarioTime <= 0)
                {
                    StartScenario();
                }
                break;
            case (ERequireType.PressBtn):
                Button btn = GetButton(curScenario.key);
                Color pressedColor = btn.colors.pressedColor;
                if (curScenario.showArrow)
                    SetArrowPos(btn.transform);
                if (btn.image.color == pressedColor)
                {
                    StartScenario();
                }
                break;
            default:
                break;
        }
        
    }
#endregion

#region ETC Functions
    private void MoveObject()
    {
        Vector3 curScenarioPos = new Vector3(curScenario.prePos.x, curScenario.prePos.y, curScenario.prePos.z);
        Vector3 nextScenarioPos = new Vector3(curScenario.nextPos.x, curScenario.nextPos.y, curScenario.nextPos.z);
        GameObject target = DetectManager.GetInstance.GetArrayObjects(curScenarioPos)[curScenarioPos];
        DetectManager.GetInstance.SwapBlockInMap(curScenarioPos, nextScenarioPos);
        target.transform.position = nextScenarioPos;

        NextScenario();
    }

    private InteractiveObject GetIObj()
    {
        Vector3 curScenarioPos = new Vector3(curScenario.targetPos.x, curScenario.targetPos.y, curScenario.targetPos.z);
        Dictionary<Vector3, GameObject> objDict = DetectManager.GetInstance.GetArrayObjects(curScenarioPos);
        if (objDict.Keys.Count <= 0) return null;
        Vector3 vec = Vector3Int.FloorToInt(curScenarioPos);
        if (objDict[vec] == null) return null;
        return objDict[vec].GetComponent<InteractiveObject>();
    }

    IEnumerator OpenClearPanel()
    {
        yield return new WaitForSeconds(delayWinUI);
        stageClearPanel.SetActive(true);
    }

    private void SetArrowPos(RectTransform targetUI)
    {
        RectTransform arrowT = arrow.rectTransform;
        arrowT = targetUI;
        arrowT.anchoredPosition += new Vector2(-0.5f, 0.5f);
    }

    private void SetArrowPos(Transform targetObj)
    {
        Vector3 pos = uiCam.WorldToScreenPoint(targetObj.position);
        arrow.gameObject.SetActive(true);
        RectTransform arrowT = arrow.rectTransform;
        arrowT.anchorMax = new Vector2(0.5f, 0.5f);
        arrowT.anchorMin = new Vector2(0.5f, 0.5f);
        arrowT.pivot = new Vector2(0.5f, 0.5f);
        arrowT.anchoredPosition = pos + new Vector3(-0.3f, 0.3f, 0f);
    }

    private KeyCode GetKeyCode(string c)
    {
        KeyCode key = KeyCode.None;
        switch (c)
        {
            case ("Q"):
            case ("q"):
                key = KeyCode.Q;
                break;
            case ("Tab"):
            case ("tab"):
                key = KeyCode.Tab;
                break;
            case ("Space"):
            case ("space"):
                key = KeyCode.Space;
                break;
            case ("E"):
            case ("e"):
                key = KeyCode.E;
                break;
            case ("R"):
            case ("r"):
                key = KeyCode.R;
                break;
            case ("ESC"):
            case ("esc"):
                key = KeyCode.Escape;
                break;
        }
        return key;
    }

    private Button GetButton(string btn)
    {
        Button button = GameObject.Find(btn).GetComponent<Button>();
        return button;
    }
#endregion

    private void Update()
    {
        if (!isStart) return;
        if (!((GameManager.GetInstance.CurrentState == GameStates.InGame) || (GameManager.GetInstance.CurrentState == GameStates.Victory))) return;
        if (nextSenarioTime != -3f) nextSenarioTime -= Time.deltaTime;
        if (restartTime > 0) restartTime -= Time.deltaTime;
        else
        {
#if UNITY_ANDROID
            LogError("오른쪽 위에 톱니 버튼을 누르고 재시작 할 수 있습니다.");
#else
            LogError("R키를 꾹 눌러서 재시작 할 수 있습니다.");
#endif
            restartTime = 10f;
        }

        if (!curScenario.showArrow && arrow.gameObject.activeSelf)
            arrow.gameObject.SetActive(false);

        if (CheckScenarioCount())
        {
            CheckScenarioCondition();
        }
    }
}
