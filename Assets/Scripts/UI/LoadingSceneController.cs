using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;
    public AsyncOperation op;
    [SerializeField] Image progressBar;
    [SerializeField] float fakeLoadTime = 0.7f;

    static bool isCustom = false;

    public static void LoadScene(string sceneName, bool isCustomLevel = false)
    {
        SoundManager.GetInstance.Play("GameStart2");
        nextScene = sceneName;
        SoundManager.GetInstance.bgmSound.Stop();
        SceneManager.LoadScene("LoadingScene");
        
        isCustom = isCustomLevel;
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            if (progressBar.fillAmount < 0.85f)
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0f, 0.85f, timer * fakeLoadTime);
            }
            else if (progressBar.fillAmount < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else if (progressBar.fillAmount == 1f)
            {
                op.allowSceneActivation = true;
                yield return null;
            }
            else if (op.progress == 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer * fakeLoadTime);
            }
        }
        
        Destroy(this.gameObject);
        yield return null;
    }

    private void OnDestroy()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.name == "LevelEditor")
        {
            GameManager.GetInstance.ChangeGameState(GameStates.LevelEditMode);
            return;
        }
        else if (scene.name == "DemoPlay")
        {
            GameManager.GetInstance.ChangeGameState(GameStates.InGame);
        }

        UIManager.GetInstance.ingameCanvas = GameObject.Find("IngameCanvas");
        UIManager.GetInstance.pauseUIPanel = UIManager.GetInstance.ingameCanvas.transform.Find("PauseUI Panel").gameObject;
        
        GameManager.GetInstance.LoadMap(isCustom);
    }
}