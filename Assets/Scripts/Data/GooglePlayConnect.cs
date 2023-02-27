using UnityEngine;
using GooglePlayGames;
using UnityEngine.SceneManagement;
using System.Collections;
using GooglePlayGames.BasicApi;

public class GooglePlayConnect : MonoBehaviour
{
    public string userID;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        GooglePlayLogin();
    }

    public void GooglePlayLogin()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
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
                SceneManager.LoadScene("MainScene");
            }
            else
            {
                Application.Quit();
            }
        });
    }

    public void GooglePlayLogout()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        Debug.Log("Logout");
    }
}
