using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogText : MonoBehaviour
{
    [SerializeField] Button skipBtn;
    [SerializeField] bool isSystemlog;
    [SerializeField] float activeTime;
    public float curTime;

    public void SetTime()
    {
        curTime = activeTime;
        if (skipBtn != null)
            skipBtn.onClick.AddListener(OnClickBtn);
    }

    void Start()
    {

    }

    void OnClickBtn()
    {
        Text btnText = skipBtn.GetComponentInChildren<Text>();
        if (btnText.text == "Skip")
        {
            SkipScenario();
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    void SkipScenario()
    {
        ScenarioController sc = GameObject.Find("LevelInfos").GetComponent<ScenarioController>();
        sc.StartCoroutine("SkipLog");
    }

    void OnEnable()
    {
        SetTime();
    }

    void Update()
    {
        if (!isSystemlog) return;

        curTime -= Time.unscaledDeltaTime;
        if (curTime <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
