using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialVideoController : MonoBehaviour
{
    [SerializeField] private VideoPlayer Vplayer;
    [SerializeField] private GameObject videoPanel;
    [SerializeField] private RawImage screen;
    [SerializeField] private Text title;
    [SerializeField] private string[] clipPaths;
    [SerializeField] private Button[] arrows;
    [SerializeField] private string[] titles;

    private int[] selectedVideosIdx;
    private int selectedIdx = 0;

    void Awake()
    {
        arrows[0].onClick.AddListener(() => OnClickArrow(-1));
        arrows[1].onClick.AddListener(() => OnClickArrow(1));
    }

    public void OnStopVideo()
    {
        Vplayer.Stop();
        Vplayer.clip = null;
    }

    public void OnClickArrow(int value)
    {
        int idx = selectedIdx + value > selectedVideosIdx.Length - 1 ?
            selectedVideosIdx.Length - 1 :
            (selectedIdx + value < 0 ? 0 :
            selectedIdx + value);

        OnStopVideo();
        int num = selectedVideosIdx[idx];
        title.text = titles[num];
        Vplayer.url = clipPaths[num];

        StartCoroutine(PrepareVideo());
    }

    public void SetVideo(params int[] idx)
    {
        videoPanel.SetActive(true);
        if (idx.Length > 1)
        {
            arrows[0].gameObject.SetActive(true);
            arrows[1].gameObject.SetActive(true);
        }
        else
        {
            arrows[0].gameObject.SetActive(false);
            arrows[1].gameObject.SetActive(false);
        }
        selectedVideosIdx = idx;

        OnStopVideo();
        
        title.text = titles[idx[0]];
        Vplayer.url = clipPaths[idx[0]];

        StartCoroutine(PrepareVideo());
    }

    private IEnumerator PrepareVideo()
    {
        screen.color = new Color(1, 1, 1, 0);
        Vplayer.Prepare();

        while (!Vplayer.isPrepared)
        {
            yield return new WaitForSeconds(0.5f);
        }

        screen.color = new Color(1, 1, 1, 1);
        screen.texture = Vplayer.texture;
        Vplayer.Play();
    }

    void Update()
    {
        
    }
}
