using UnityEngine;
using GooglePlayGames;
using UnityEngine.SceneManagement;
using System.Collections;

public class GooglePlayConnect : MonoBehaviour
{
    [SerializeField] public string userID;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        GooglePlayLogin();
    }

    public void GooglePlayLogin()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        GetGooglePlayUserID();
    }

    public void GetGooglePlayUserID()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                userID = Social.localUser.id;
                
                StartCoroutine(GetID());
            }
            else
            {
                userID = "111111";
                StartCoroutine(GetID());
                // Application.Quit();
            }
        });
    }

    private IEnumerator GetID()
    {
        while (userID == null)
        {
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene("MainScene");
    }

    public void GooglePlayLogout()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        Debug.Log("Logout");
    }
}
