using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Storage;
using UnityEngine.Networking;
using System.IO;
using System.Threading;

public class Test : MonoBehaviour
{
    FirebaseStorage storage;
    StorageReference storageReference;
    private StorageReference userFile;

    private void OnEnable()
    {
        // DataList<SUserData> dataList = new DataList<SUserData>();
        // dataList.dataList = new List<SUserData>();
        // foreach (var data in GameDataManager.GetInstance.UserDataDic)
        // {
        //     dataList.dataList.Add(data.Value);
        // }
        
        // string json = JsonUtility.ToJson(dataList);
        // Debug.Log(json);
        //
        // storage = FirebaseStorage.DefaultInstance;
        // storageReference = storage.GetReferenceFromUrl("gs://namer-c0bfb.appspot.com");
        //
        // string filePath = "Assets/Resources/Data/SaveLoad/JSON/user.json";
        // MetadataChange newMetadata = new MetadataChange();
        // newMetadata.ContentType = "application/json";
        //
        // userFile = storageReference.Child("user.json");
        // userFile.PutFileAsync(filePath, newMetadata, null, CancellationToken.None).ContinueWith(task =>
        // {
        //     if (!task.IsFaulted && !task.IsCanceled)
        //     {
        //         StorageMetadata metadata = task.Result;
        //     }
        //     else
        //     {
        //         Debug.LogError(task.Exception.ToString());
        //     }
        // });
        
        // userFile.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        // {
        //     if (!task.IsFaulted && !task.IsCanceled)
        //     {
        //         StartCoroutine(UpLoadFile(Convert.ToString(task.Result), json));
        //         StartCoroutine(DownLoadFile(Convert.ToString(task.Result)));
        //     }
        //     else
        //     {
        //         Debug.LogError(task.Exception.ToString());
        //     }
        // });
    }

    private IEnumerator DownLoadFile(string url)
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
                Debug.Log(request.downloadHandler.text);
            }
        }
    }

    private IEnumerator UpLoadFile(string url, string data)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(url, data))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(data);
            request.uploadHandler.Dispose();
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            // request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
    
}
