using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GooglePlayConnect : MonoBehaviour
{
    public void GooglePlayLogin()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        GetGooglePlayUserID();
    }

    public void GetGooglePlayUserID()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated == false)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    GameManager.GetInstance.userId = Social.localUser.id;
                    Debug.Log("Success Login " + Social.localUser.id);
                }
                else
                {
                    GameManager.GetInstance.userId = "111111";
                    Debug.Log("Failed Login");
                }
            });
        }
    }

    public void GooglePlayLogout()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        Debug.Log("Logout");
    }
}
