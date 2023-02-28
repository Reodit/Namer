using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : Singleton<GameDataManager>
{
    // Test
    private string firstUserID = "";
    
    // map information - 3 dimensional space array
    private GameObject[,,] initTiles;
    public GameObject[,,] InitTiles { get { return initTiles; } set { initTiles = value; } }
    
    private GameObject[,,] initObjects;
    public GameObject[,,] InitObjects { get { return initObjects; } set { initObjects = value; } }
    private Dictionary<int, SCardView> cardEncyclopedia = new Dictionary<int, SCardView>();
    public Dictionary<int, SCardView> CardEncyclopedia { get { return cardEncyclopedia; } }

    // user information dictionary
    private Dictionary<string, SUserData> userDataDic = new Dictionary<string, SUserData>();
    public Dictionary<string, SUserData> UserDataDic { get { return userDataDic; }}
    
    // level information dictionary
    private Dictionary<int, SLevelData> levelDataDic = new Dictionary<int, SLevelData>();
    public Dictionary<int, SLevelData> LevelDataDic { get { return levelDataDic; } }
    // custom level information dictionary
    private Dictionary<int, SLevelData> customLevelDataDic = new Dictionary<int, SLevelData>();
    public Dictionary<int, SLevelData> CustomLevelDataDic { get { return customLevelDataDic; } }
    
    // card information dictionary
    private Dictionary<EName, SNameInfo> names = new Dictionary<EName, SNameInfo>();
    public Dictionary<EName, SNameInfo> Names { get { return names; } }

    private Dictionary<EAdjective, SAdjectiveInfo> adjectives = new Dictionary<EAdjective, SAdjectiveInfo>();
    public Dictionary<EAdjective, SAdjectiveInfo> Adjectives { get { return adjectives; } }
    
    // file information
    private string filePath;
    private string tileMapFileName;
    private string objectMapFileName;
    private string objectInfoFileName;
    private string userDataFileName;
    private string levelDataFileName;
    private string customLevelDataFileName;
    
    // MapData
    private SMapData mapData;
    
    private void FilePathInfo()
    {
        filePath = Application.persistentDataPath + "/Data/";

        tileMapFileName = "tileMapData.csv";
        objectMapFileName = "objectMapData.csv";
        objectInfoFileName = "objectInfoData.json";

        userDataFileName = "user.json";
        levelDataFileName = "levels.json";
        customLevelDataFileName = GameManager.GetInstance.userId + "Levels.json";
    }

#region Map(tile, object) Data

    public void ReadMapData()
    {
        MapReader mapReader = FindObjectOfType<MapReader>();
        if (!mapReader)
        {
            mapReader = gameObject.AddComponent<MapReader>();
        }
        
        mapData = mapReader.GetMapData();
        // Test
        initTiles = (GameObject[,,])mapData.tiles.Clone();
        initObjects = (GameObject[,,])mapData.objects.Clone();
        // 
    }

    public void CreateFile()
    {
        FilePathInfo();

        if (mapData.tiles == null)
        {
            Debug.LogError("ReadMapData() 함수 사용했는지 확인해주세요!");
        }

        string sceneName = "";
        if (GameManager.GetInstance.CurrentState == GameStates.LevelEditorTestPlay)
        {
            sceneName = GameManager.GetInstance.userId + "/" + CustomLevelDataDic[GameManager.GetInstance.CustomLevel].sceneName;
        }
        else
        {
            sceneName = SceneManager.GetActiveScene().name;
        }

        SaveLoadFile saveFile = new SaveLoadFile();
        saveFile.CreateCsvFile(mapData.tileMapData, "Assets/Resources/Data/" + sceneName, tileMapFileName);
        saveFile.CreateCsvFile(mapData.objectMapData, "Assets/Resources/Data/" + sceneName, objectMapFileName);
        saveFile.CreateJsonFile(mapData.objectInfoData, "Assets/Resources/Data/" + sceneName, objectInfoFileName);
    }

    public void CreateMap(int level, bool isCustomLevel = false)
    {
        FilePathInfo();
        
        string levelFilePath = "", sceneName = "";
        if (!isCustomLevel)
        {
            levelFilePath = filePath;
            sceneName = levelDataDic[level].sceneName;
        }
        else
        {
            levelFilePath = "Assets/Resources/Data/" + GameManager.GetInstance.userId + "/";
            sceneName = customLevelDataDic[level].sceneName;
        }
        
        if (sceneName == null || sceneName == "")
        {
            Debug.LogError("Load Level를 입력해주세요");
            return;
        }

        SaveLoadFile loadFile = new SaveLoadFile();
        StringReader tileMapData = loadFile.ReadCsvFile(levelFilePath + sceneName, tileMapFileName);
        StringReader objectMapData = loadFile.ReadCsvFile(levelFilePath + sceneName, objectMapFileName);
        Dictionary<int, SObjectInfo> objectInfoDic = loadFile.ReadJsonFile<int, SObjectInfo>(levelFilePath + sceneName, objectInfoFileName);

        MapCreator mapCreator = FindObjectOfType<MapCreator>();
        if (!mapCreator)
        {
            mapCreator = gameObject.AddComponent<MapCreator>();
        }
        initTiles = mapCreator.CreateTileMap(tileMapData);
        initObjects = mapCreator.CreateObjectMap(objectMapData, objectInfoDic);

        SCardView cardView = isCustomLevel ? customLevelDataDic[level].cardView : levelDataDic[level].cardView;
        SetCardEncyclopedia(cardView);
    }

    public void CreateCustomLevelMap()
    {
        MapCreator mapCreator = FindObjectOfType<MapCreator>();
        if (!mapCreator)
        {
            mapCreator = gameObject.AddComponent<MapCreator>();
        }

        initTiles = mapCreator.CreateTileMap(new StringReader(mapData.tileMapData.ToString()));
        initObjects = mapCreator.CreateObjectMap(new StringReader(mapData.objectMapData.ToString()),
            mapData.objectInfoData.ToDictionary(item => item.objectID, item => item));
    }

    #endregion

#region User And Level Data

    public void GetUserAndLevelData()
    {
        FilePathInfo();
        
        SaveLoadFile loadFile = new SaveLoadFile();
        userDataDic = loadFile.ReadJsonFile<string, SUserData>(filePath + "SaveLoad", userDataFileName);
        levelDataDic = loadFile.ReadJsonFile<int, SLevelData>(filePath + "SaveLoad", levelDataFileName);
        customLevelDataDic = loadFile.ReadJsonFile<int, SLevelData>(filePath + "SaveLoad", customLevelDataFileName);
    }

    public void AddUserData(string userID)
    {
        if (!userDataDic.ContainsKey("000000"))
        {
            Debug.LogError("GameDataManager.GetInstance.GetUserAndLevelData()를 해주세요!");
            return;
        }
        
        if (!userDataDic.ContainsKey(userID))
        {
            SUserData userData = new SUserData(userID);
            userDataDic.Add(userID, userData);
            
            SaveLoadFile saveFile = new SaveLoadFile();
            saveFile.UpdateDicDataToJsonFile(userDataDic, filePath + "SaveLoad", userDataFileName);
            
            GameManager.GetInstance.userId = userID;
            firstUserID = userID;
        }
        
        // Test
        if (GameManager.GetInstance.userId == "000000")
        {
            GameManager.GetInstance.userId = userID;
            firstUserID = userID;
        }
        //
    }

    public void ResetUserData(string userID)
    {
        if (!UserDataDic.ContainsKey(userID))
        {
            Debug.LogError(userID + " 사용자 ID를 가진 사용자가 없습니다. 확인해주세요!");
        }
        
        SUserData userData = new SUserData(userID);
        userDataDic[userID] = userData;
    }
    
    public string GetUserID()
    {
        return firstUserID;
    }

    public void SetLevelName(string levelName)
    {
        string userID = GameManager.GetInstance.userId;

        if (GameManager.GetInstance.CurrentState == GameStates.LevelEditorTestPlay)
        {
            List<SLevelName> levelNames = userDataDic[userID].customLevelNames;
            SetLevelNameByState(GameManager.GetInstance.CustomLevel, levelName, ref levelNames);
        }
        else
        {
            List<SLevelName> levelNames = userDataDic[userID].levelNames;
            SetLevelNameByState(GameManager.GetInstance.Level, levelName, ref levelNames);
        }
    }

    private void SetLevelNameByState(int level, string levelName, ref List<SLevelName> levelNames)
    {
        bool hasLevelName = false;
        
        if (levelNames != null)
        {
            for (int i = 0; i < levelNames.Count; i++)
            {
                if (levelNames[i].level == level)
                {
                    levelNames[i] = new SLevelName(level, levelName);
                    hasLevelName = true;
                    break;
                }
            }
        }

        if (!hasLevelName)
        {
            levelNames.Add(new SLevelName(level, levelName));
        }
    }

    public string GetLevelName(int level)
    {
        string userID = GameManager.GetInstance.userId;

        if (UserDataDic[userID].levelNames != null)
        {
            foreach (var levelName in UserDataDic[userID].levelNames)
            {
                if (levelName.level == level)
                {
                    return levelName.levelName;
                }
            }
        }
        
        return "";
    }

    public void UpdateUserData(bool isLevelClear)
    {
        if (isLevelClear)
        {
            string userID = GameManager.GetInstance.userId;
            int level = GameManager.GetInstance.Level;
            
            SUserData userData = userDataDic[userID];
            userData.clearLevel = level;
            userDataDic[userID] = userData;
        }

        SaveLoadFile saveFile = new SaveLoadFile();
        saveFile.UpdateDicDataToJsonFile(userDataDic, filePath + "SaveLoad", userDataFileName);
    }

    public void SetGameSetting(SGameSetting gameSetting)
    {
        string userID = GameManager.GetInstance.userId;
        
        SUserData userData = userDataDic[userID];
        userData.gameSetting = gameSetting;
        userDataDic[userID] = userData;
    }
    

    public void AddCustomLevelData(int level, SLevelData levelData)
    {
        if (CustomLevelDataDic.ContainsKey(level))
        {
            CustomLevelDataDic[level] = levelData;
        }
        else
        {
            CustomLevelDataDic.Add(level, levelData);
        }

        SaveLoadFile saveFile = new SaveLoadFile();
        saveFile.UpdateDicDataToJsonFile(customLevelDataDic, filePath + "SaveLoad", customLevelDataFileName);
    }

    public void DeleteCustomLevelData(int level)
    {
        CustomLevelDataDic.Remove(level);
    }

#endregion

#region Get/Set Card Data & Create Card Prefabs

    public void GetCardData()
    {
        SaveLoadFile loadFile = new SaveLoadFile();
        XmlNodeList data = loadFile.ReadXmlFile(filePath + "SaveLoad", "root/worksheet");
        
        names = loadFile.GetCardData<EName, SNameInfo>(data, 0);
        adjectives = loadFile.GetCardData<EAdjective, SAdjectiveInfo>(data, 1);
        
        int interactionPriority = 0;
        int normalPriority = 0;

        for (int i = 0; i < adjectives.Count; i++)
        {
            SAdjectiveInfo adjectiveInfo = adjectives[(EAdjective)i];

            if (adjectiveInfo.adjectiveName == EAdjective.Null)
            {
                continue;
            }

            if (adjectiveInfo.adjective.GetAdjectiveType() == EAdjectiveType.Interaction)
            {
                adjectiveInfo.uiPriority = ++interactionPriority;
            }
            else
            {
                adjectiveInfo.uiPriority = ++normalPriority;
            }

            adjectives[(EAdjective)i] = adjectiveInfo;
        }
    }

    public void SetCardEncyclopedia(SCardView cardView)
    {
        int level = GameManager.GetInstance.Level;
        
        if (!cardEncyclopedia.ContainsKey(level))
        {
            SCardView sCardView = new SCardView();
            sCardView.nameRead = new List<EName>();
            sCardView.adjectiveRead = new List<EAdjective>();
            
            cardEncyclopedia.Add(level, sCardView);
        }

        List<EAdjective> adjectiveReads = new List<EAdjective>();
        for (int i = 0; i < cardView.nameRead.Count; i++)
        {
            if (!cardEncyclopedia[level].nameRead.Contains(cardView.nameRead[i]))
            {
                cardEncyclopedia[level].nameRead.Add(cardView.nameRead[i]);
            }
            
            if (Names[cardView.nameRead[i]].adjectives != null)
            {
                adjectiveReads.AddRange(Names[cardView.nameRead[i]].adjectives);
            }
        }
        
        if (cardView.adjectiveRead.Count > 0)
        {
            adjectiveReads.AddRange(cardView.adjectiveRead);
            adjectiveReads.Distinct().ToList();
        }
        
        for (int i = 0; i < adjectiveReads.Count; i++)
        {
            if (!cardEncyclopedia[level].adjectiveRead.Contains(adjectiveReads[i]))
            {
                cardEncyclopedia[level].adjectiveRead.Add(adjectiveReads[i]);
            }
        }
    }
    
    // Create Card Prefabs
    public GameObject[] GetIngameCardEncyclopedia()
    {
        return GetCardPrefabs(cardEncyclopedia[GameManager.GetInstance.Level]);
    }

    public GameObject[] GetMainCardEncyclopedia()
    {
        return GetCardPrefabs(UserDataDic[GameManager.GetInstance.userId].cardView);
    }

    public int GetRewardCardCount()
    {
        int level = GameManager.GetInstance.Level;
        string userID = GameManager.GetInstance.userId;

        SCardView mainCards = UserDataDic[userID].cardView;
        SCardView ingameCards = CardEncyclopedia[level];

        List<EName> nameCards = new List<EName>();
        List<EAdjective> adjectiveCards = new List<EAdjective>();

        for (int i = 0; i < ingameCards.nameRead.Count; i++)
        {
            bool isCheck = false;
            for (int j = 0; j < mainCards.nameRead.Count; j++)
            {
                if (mainCards.nameRead[j] == ingameCards.nameRead[i])
                {
                    isCheck = true;
                    break;
                }
            }

            if (!isCheck)
            {
                nameCards.Add(ingameCards.nameRead[i]);
            }
        }
        
        for (int i = 0; i < ingameCards.adjectiveRead.Count; i++)
        {
            bool isCheck = false;
            for (int j = 0; j < mainCards.adjectiveRead.Count; j++)
            {
                if (mainCards.adjectiveRead[j] == ingameCards.adjectiveRead[i])
                {
                    isCheck = true;
                    break;
                }
            }

            if (!isCheck)
            {
                adjectiveCards.Add(ingameCards.adjectiveRead[i]);
            }
        }
        
        return nameCards.Count + adjectiveCards.Count;
    }

    public GameObject[] GetRewardCardEncyclopedia()
    {
        int level = GameManager.GetInstance.Level;
        string userID = GameManager.GetInstance.userId;
        
        HashSet<EName> nameReads = new HashSet<EName>();
        HashSet<EAdjective> adjectiveReads = new HashSet<EAdjective>();
        
        foreach (EName name in CardEncyclopedia[level].nameRead)
        {
            if (!UserDataDic[userID].cardView.nameRead.Contains(name))
            {
                nameReads.Add(name);

                if (Names[name].adjectives != null)
                {
                    foreach (var adjective in Names[name].adjectives)
                    {
                        if (!UserDataDic[userID].cardView.adjectiveRead.Contains(adjective))
                        {
                            adjectiveReads.Add(adjective);
                        }
                    }
                }
            }
        }

        foreach (var adjective in CardEncyclopedia[level].adjectiveRead)
        {
            if (!UserDataDic[userID].cardView.adjectiveRead.Contains(adjective))
            {
                adjectiveReads.Add(adjective);
            }
        }
        
        nameReads = Enumerable.ToHashSet(nameReads.OrderBy(item => Names[item].priority));
        adjectiveReads = Enumerable.ToHashSet(adjectiveReads.OrderBy(item => Adjectives[item].priority));

        // update user data
        SUserData userData = UserDataDic[userID];
        userData.cardView.nameRead.AddRange(nameReads);
        userData.cardView.nameRead.OrderBy(item => Names[item].priority).ToList();
        userData.cardView.adjectiveRead.AddRange(adjectiveReads);
        userData.cardView.adjectiveRead.OrderBy(item => Adjectives[item].priority).ToList();
        // 
        
        return GetCardPrefabs(new SCardView(nameReads.ToList(), adjectiveReads.ToList()));
    }
    
    public GameObject[] GetCardPrefabs(SCardView cardView)
    {
        if (cardView.nameRead.Count == 0 && cardView.adjectiveRead.Count == 0)
        {
            return null;
        }
        
        List<EName> nameReads = cardView.nameRead.OrderBy(item => names[item].priority).ToList();
        List<EAdjective> adjectiveReads = cardView.adjectiveRead.OrderBy(item => adjectives[item].priority).ToList();
        
        List<GameObject> cards = new List<GameObject>();
        for (int i = 0; i < nameReads.Count; i++)
        {
            if (names[nameReads[i]].name == EName.Null)
            {
                continue;   
            }
            
            if ((int)nameReads[i] > names.Count - 1)
            {
                Debug.LogError("입력할 수 있는 네임 카드 번호를 벗어났어요!");
            }

            GameObject cardPrefab = Resources.Load("Prefabs/Cards/01. NameCard/" + names[nameReads[i]].cardPrefabName) as GameObject;
            cards.Add(cardPrefab);
        }

        List<GameObject> normalAdjCards = new List<GameObject>();
        List<GameObject> InteractionAdjCards = new List<GameObject>();

        // sort adjective cards to ui priority
        for (int i = 0; i < adjectiveReads.Count; i++)
        {
            if (adjectives[adjectiveReads[i]].adjectiveName == EAdjective.Null)
            {
                continue;   
            }
            
            if ((int)adjectiveReads[i] > adjectives.Count - 1)
            {
                Debug.LogError("입력할 수 있는 꾸밈 성질 카드 번호를 벗어났어요!");
            }

            GameObject cardPrefab = Resources.Load("Prefabs/Cards/02. AdjustCard/" + adjectives[adjectiveReads[i]].cardPrefabName) as GameObject;
            if (adjectives[adjectiveReads[i]].adjective.GetAdjectiveType() == EAdjectiveType.Interaction)
            {
                InteractionAdjCards.Add(cardPrefab);
            }
            else
            {
                normalAdjCards.Add(cardPrefab);
            }
        }

        cards.AddRange(InteractionAdjCards);
        cards.AddRange(normalAdjCards);

        return cards.ToArray();
    }

#endregion


}