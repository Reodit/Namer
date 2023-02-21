using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;
using System.Xml;
using UnityEngine.Networking;
using Firebase.Extensions;
using Firebase.Storage;

public class SaveLoadFileInServer
{
    // Todo 젠킨스로 서버 변경 시, 삭제 예정
    FirebaseStorage storage;
    StorageReference storageReference;
    
    public void FirebaseInfo()
    {
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://namer-c0bfb.appspot.com");
    }
    //
    
    public void UploadFile(string filePath)
    {
        FirebaseInfo();
        
        string fileType = filePath.Split('.').ToArray()[1];
        
        MetadataChange newMetadata = new MetadataChange();
        newMetadata.ContentType = "application/" + fileType;

        string uploadFilePath = filePath.Replace("Assets/Resources/", "");
        StorageReference uploadFile = storageReference.Child(uploadFilePath);

        uploadFile.PutFileAsync(filePath, newMetadata, null, CancellationToken.None).ContinueWith(task =>
        {
            if (task.IsFaulted && task.IsCanceled)
            {
                Debug.LogError(task.Exception.ToString());
            }
            else
            {
                StorageMetadata metadata = task.Result;
            }
        });
    }
    
    public void DownloadFile(string downloadFilePath, int index = -1)
    {
        FirebaseInfo();
        
        StorageReference downloadFile = storageReference.Child(downloadFilePath);
        
        downloadFile.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted && task.IsCanceled)
            {
                Debug.LogError(task.Exception.ToString());
            }
            else
            {
                GameDataManager.GetInstance.StartCoroutine(DownloadData(Convert.ToString(task.Result), index));
            }
        });
    }
    
    private IEnumerator DownloadData(string url, int index)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string data = request.downloadHandler.text;
                if (request.downloadHandler.text[0] != '{')
                {
                    data = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data, 3, request.downloadHandler.data.Length - 3);
                }

                if (url.Contains("user"))
                {
                    GameDataManager.GetInstance.UserDataDic = JsonToDictionary<string, SUserData>(data);
                }
                else if (url.Contains("level"))
                {
                    GameDataManager.GetInstance.LevelDataDic = JsonToDictionary<int, SLevelData>(data);
                }
                else if (url.Contains("Card"))
                {
                    XmlDocument xmlFile = new XmlDocument(); 
                    xmlFile.LoadXml(data);
                    
                    if (index == 0)
                    {
                        GameDataManager.GetInstance.Names = GetCardData<EName, SNameInfo>(xmlFile.SelectNodes("root/worksheet"), index);
                    }
                    else if (index == 1)
                    {
                        GameDataManager.GetInstance.Adjectives = GetCardData<EAdjective, SAdjectiveInfo>(xmlFile.SelectNodes("root/worksheet"), index);
                    }
                }
            }
        }
    }

    private Dictionary<TK, TV> JsonToDictionary<TK, TV>(string json)
    {
        Dictionary<TK, TV> dataDic = new Dictionary<TK, TV>();
        
        DataList<TV> data = JsonUtility.FromJson<DataList<TV>>(json);
        
        Type typeTV = typeof(TV);
        foreach (TV info in data.dataList)
        {
            switch (typeTV.FullName)
            {
                case "SUserData" :
                    SUserData userData = (SUserData)(object)info;
                    TK userID = (TK)(object)userData.userID;
                    if (!dataDic.ContainsKey(userID))
                    {
                        dataDic.Add(userID, info);
                    }
                    break;
                case "SLevelData" :
                    SLevelData levelData = (SLevelData)(object)info;
                    TK level = (TK)(object)levelData.level;
                    if (!dataDic.ContainsKey(level))
                    {
                        dataDic.Add(level, info);
                    }
                    break;
                case "SObjectInfo" :
                    SObjectInfo objectInfo = (SObjectInfo)(object)info;
                    TK objectID = (TK)(object)objectInfo.objectID;
                    if (!dataDic.ContainsKey(objectID))
                    {
                        dataDic.Add(objectID, info);
                    }
                    break;
            }
        }
        
        return dataDic;
    }

    public Dictionary<TK, TV> GetCardData<TK, TV>(XmlNodeList data, int index)
    {
        if (data == null)
        {
            Debug.Log("XML 데이터를 제대로 가져오지 못헀습니다. 확인 부탁요!");
            return null;
        }
        
        Dictionary<TK, TV> dataDic = new Dictionary<TK, TV>();
        
        // index == 0 : Name, index == 1 : Adjective
        XmlNodeList nodeList = data.Item(index).SelectNodes("Row");
        foreach (XmlNode item in nodeList)
        {
            if (index == 0)
            {
                int priority = Convert.ToInt32(item["우선순위"]?.InnerText);
                string uiText = item["UI표시텍스트"]?.InnerText;
                EName itemName = (EName)Enum.Parse(typeof(EName), item["이름명"]?.InnerText);
            
                string adjText = item["보유꾸밈성질"]?.InnerText;
                EAdjective[] adjNames = null;
                if (adjText != "")
                {
                    adjNames = adjText.Split(", ").Select(item => (EAdjective)Enum.Parse(typeof(EAdjective), item)).ToArray();
                }
                
                string cardPrefabName = item["카드프리팹이름"]?.InnerText;
                string content = item["설명"]?.InnerText;

                TK key = (TK)(object)itemName;
                if (!dataDic.ContainsKey(key))
                {
                    TV value = (TV)(object)new SNameInfo(priority, uiText, itemName, adjNames, cardPrefabName, content);
                    dataDic.Add(key, value);
                }
            }
            else if (index == 1)
            {
                int priority = Convert.ToInt32(item["우선순위"]?.InnerText);
                string uiText = item["UI표시텍스트"]?.InnerText;
                EAdjective adjName = (EAdjective)Enum.Parse(typeof(EAdjective), item["이름명"]?.InnerText);
                string cardPrefabName = item["카드프리팹이름"]?.InnerText;
                string content = item["설명"]?.InnerText;
                    
                TK key = (TK)(object)adjName;
                if (!dataDic.ContainsKey(key))
                {
                    TV value = (TV)(object)new SAdjectiveInfo(priority, uiText, adjName, cardPrefabName, content);
                    dataDic.Add(key, value);
                }
            }
        }

        return dataDic;
    }
}
