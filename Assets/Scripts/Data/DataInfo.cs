using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;

#region Card Information Struct

public struct SNameInfo
{
    public readonly int priority;
    public readonly string uiText;
    public readonly EName name;
    public readonly EAdjective[] adjectives;
    public readonly string cardPrefabName;
    public readonly string contentText;

    public SNameInfo(int priority, string uiText, EName name, EAdjective[] adjectives, string cardPrefabName, string contentText)
    {
        this.priority = priority;
        this.uiText = uiText;
        this.name = name;
        this.adjectives = adjectives;
        this.cardPrefabName = cardPrefabName;
        this.contentText = contentText;
    }
}

public struct SAdjectiveInfo
{
    public readonly int priority;
    public readonly string uiText;
    public readonly EAdjective adjectiveName;
    public readonly IAdjective adjective;
    public readonly string cardPrefabName;
    public readonly string contentText;
    public int uiPriority;

    public SAdjectiveInfo(int priority, string uiText, EAdjective adjectiveName, string cardPrefabName, string contentText)
    {
        this.priority = priority;
        this.uiText = uiText;
        this.adjectiveName = adjectiveName;
        
        Type type = Type.GetType(adjectiveName + "Adj");
        var adjFunc = Activator.CreateInstance(type) as IAdjective;
        this.adjective = adjFunc;
        
        this.cardPrefabName = cardPrefabName;
        this.contentText = contentText;
        this.uiPriority = 0;
    }
}

#endregion

#region Object Information Struct

[Serializable]
public struct SObjectInfo
{
    public string prefabName;
    public int objectID;
    public EName nameType;
    public EAdjective[] adjectives;
}

#endregion

#region Game Information(Map, User, Level) Struct

[Serializable]
public struct SMapData
{
    public readonly GameObject[,,] tiles;
    public readonly GameObject[,,] objects;
    public readonly StringBuilder tileMapData;
    public readonly StringBuilder objectMapData;
    public readonly List<SObjectInfo> objectInfoData;

    public SMapData(GameObject[,,] tiles, GameObject[,,] objects, StringBuilder tileMapData, StringBuilder objectMapData, List<SObjectInfo> objectInfoData)
    {
        this.tiles = tiles;
        this.objects = objects;
        this.tileMapData = tileMapData;
        this.objectMapData = objectMapData;
        this.objectInfoData = objectInfoData;
    }
}

public struct SMapGameObjects
{
    public readonly GameObject[,,] tiles;
    public readonly GameObject[,,] objects;
    
    public SMapGameObjects(GameObject[,,] tiles, GameObject[,,] objects)
    {
        this.tiles = tiles;
        this.objects = objects;
    }
}

[Serializable]
public struct SUserData
{
    public string userID;
    public string nickName;
    public int clearLevel;
    public List<SLevelName> levelNames;
    public List<SLevelName> customLevelNames;
    public SCardView cardView;
    public SGameSetting gameSetting;

    public SUserData(string userID)
    {
        this.userID = userID;
        this.nickName = "";
        this.clearLevel = -1;
        this.levelNames = new List<SLevelName>();
        this.customLevelNames = new List<SLevelName>();
        this.cardView = new SCardView(new[] { EName.Rose }.ToList(), new[] { EAdjective.Win }.ToList());
        this.gameSetting = new SGameSetting(0, 0, 
            0.5f, 0.5f, 0.5f, 
            false, false, false, false);
    }
}

[Serializable]
public struct SLevelData
{
    public int level;
    public string sceneName;
    public Scenario[] scenario;
    public SPosition playerPosition;
    public SCardView cardView;

    public SLevelData(int level, string sceneName, SPosition playerPosition, SCardView cardView)
    {
        this.level = level;
        this.sceneName = sceneName;
        this.scenario = null;
        this.playerPosition = playerPosition;
        this.cardView = cardView;
    }
}

[Serializable]
public struct SLevelName
{
    public int level;
    public string levelName;

    public SLevelName(int level, string levelName)
    {
        this.level = level;
        this.levelName = levelName;
    }
}

[Serializable]
public struct SCardView
{
    public List<EName> nameRead;
    public List<EAdjective> adjectiveRead;

    public SCardView(List<EName> nameRead, List<EAdjective> adjectiveRead)
    {
        this.nameRead = nameRead;
        this.adjectiveRead = adjectiveRead;
    }
}

[Serializable]
public struct SGameSetting
{
    public int resolution;
    public int maxFrame;
    public float volume;
    public float backgroundVolume;
    public float soundEffects;
    public bool isfullScreen;
    public bool isborderlessFullScreen;
    public bool isMute;
    public bool isMuteInBackground;

    public SGameSetting(int resolution, int maxFrame, float volume, float backgroundVolume, float soundEffects,
        bool isfullScreen, bool isborderlessFullScreen, bool isMute, bool isMuteInBackground)
    {
        this.resolution = resolution;
        this.maxFrame = maxFrame;
        this.volume = volume;
        this.backgroundVolume = backgroundVolume;
        this.soundEffects = soundEffects;
        this.isfullScreen = isfullScreen;
        this.isborderlessFullScreen = isborderlessFullScreen;
        this.isMute = isMute;
        this.isMuteInBackground = isMuteInBackground;
    }
}

[Serializable]
public struct SPosition
{
    public float x;
    public float y;
    public float z;

    public SPosition(Vector3 position)
    {
        x = position.x;
        y = position.y;
        z = position.z;
    }
}

#endregion

[Serializable]
public class DataList<T>
{
    public List<T> dataList;
}

public class DataInfo
{
    
}